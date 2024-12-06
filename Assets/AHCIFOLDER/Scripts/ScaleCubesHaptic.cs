using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class ScaleCubesHaptic : MonoBehaviour
{
    // Tracks the previous scale of the object
    private Vector3 previousScale;

    // References to XR controllers
    public XRBaseController controllerLeft;
    public XRBaseController controllerRight;
    private XRGrabInteractable grabInteractable;

    // Flags for detecting grabbing state
    private bool isGrabbingLeft;
    private bool isGrabbingRight;
    private bool isGrabbing;

    // UI and platform-related references
    public GameObject smallCanvas;
    public GameObject platform;

    // Variables for haptic feedback
    private float amplitude;
    private float duration;

    // Initial position and rotation for resetting the object
    private Vector3 startPos;
    private quaternion startRot;
    private float yOffset;

    // Tag used to identify correct placement areas
    public string correctTag;
    private Rigidbody rb;

    // Counter for correctly placed objects
    private int placedCounter = 0;

    // Objects to activate upon correct placement
    public GameObject hoverCube;
    public GameObject spotLight;

    // Light properties adjustment
    public float increment = 1f; // Amount to increase the spot angle per update
    public Light lightTEST;

    private void Awake()
    {
        // Set up grab interactable and event listeners
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void Start()
    {
        // Initialize variables
        isGrabbing = false;
        isGrabbingLeft = false;
        isGrabbingRight = false;
        smallCanvas.SetActive(false);

        rb = GetComponent<Rigidbody>();
        previousScale = transform.localScale;
        startRot = transform.rotation;
    }

    void Update()
    {
        // Update grabbing state based on left and right controller flags
        isGrabbing = isGrabbingLeft || isGrabbingRight;

        // Show or hide the small canvas based on grabbing state
        smallCanvas.SetActive(isGrabbing);

        // Map the current scale to haptic amplitude
        amplitude = MapRange(transform.localScale.x, 0.05f, 0.2f, 0.1f, 1f);

        // Send haptic feedback if the scale has changed
        if (transform.localScale != previousScale)
        {
            controllerLeft?.SendHapticImpulse(amplitude, duration);
            controllerRight?.SendHapticImpulse(amplitude, duration);

            previousScale = transform.localScale;
        }

        // Adjust light properties based on scale
        lightTEST.spotAngle = MapRange(transform.localScale.x, 0.05f, 0.2f, 10, 15);
        lightTEST.innerSpotAngle = lightTEST.spotAngle - 4;

        // Update Y-offset and starting position based on scale
        yOffset = MapRange(transform.localScale.x, 0.05f, 0.2f, 0.035f, 0.1f);
        startPos = new Vector3(platform.transform.localPosition.x, platform.transform.localPosition.y + yOffset, platform.transform.localPosition.z);
    }

    // Maps a value from one range to another
    float MapRange(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }

    private void OnDestroy()
    {
        // Remove event listeners to prevent memory leaks
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    // Called when the object is grabbed
    private void OnGrab(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            if (controllerInteractor.CompareTag("LeftController"))
            {
                controllerLeft = controllerInteractor.xrController;
                isGrabbingLeft = true;
                Debug.Log("Left Controller is grabbing.");
            }
            else if (controllerInteractor.CompareTag("RightController"))
            {
                controllerRight = controllerInteractor.xrController;
                isGrabbingRight = true;
                Debug.Log("Right Controller is grabbing.");
            }
        }
    }

    // Called when the object is released
    private void OnRelease(SelectExitEventArgs args)
    {
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            if (controllerInteractor.CompareTag("LeftController"))
            {
                controllerLeft = null;
                isGrabbingLeft = false;
                Debug.Log("Left Controller is released.");
            }
            else if (controllerInteractor.CompareTag("RightController"))
            {
                controllerRight = null;
                isGrabbingRight = false;
                Debug.Log("Right Controller is released.");
            }
        }
    }

    // Handles collisions with trigger objects
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(correctTag))
        {
            placedCounter++;
            ResetObjectPosition();

            if (placedCounter > 2)
            {
                hoverCube.SetActive(true);
                spotLight.SetActive(true);
            }
        }
    }

    // Handles exit from trigger objects
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(correctTag))
        {
            hoverCube.SetActive(false);
            spotLight.SetActive(false);
        }
    }

    // Resets the object position on collision with the floor
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            ResetObjectPosition();
        }
    }

    // Resets the object's position and velocity
    private void ResetObjectPosition()
    {
        transform.position = startPos;
        transform.rotation = startRot;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
