using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class ScaleCubesHaptic : MonoBehaviour
{
    private Vector3 previousScale;

    public XRBaseController controllerLeft;
    public XRBaseController controllerRight;
    private XRGrabInteractable grabInteractable;

    private bool isGrabbingLeft;
    private bool isGrabbingRight;
    private bool isGrabbing;

    public GameObject smallCanvas;
    public GameObject platform;

    private float amplitude;
    private float duration;

    private Vector3 startPos;
    private quaternion startRot;
    private float yOffset;
    public string correctTag;
    private Rigidbody rb;


    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    // Start is called before the first frame update
    void Start()
    {
        isGrabbing = false;
        isGrabbingLeft = false;
        isGrabbingRight = false;
        smallCanvas.SetActive(false);

        rb = GetComponent<Rigidbody>();
        previousScale = transform.localScale;

        startRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbingLeft == true || isGrabbingRight == true)
        {
            isGrabbing = true;
        }
        if (isGrabbingLeft == false && isGrabbingRight == false)
        {
            isGrabbing = false;
        }

        smallCanvas.SetActive(isGrabbing);
        //Debug.Log("Is grabbing: " + isGrabbing);
        //Debug.Log("Left Grab: " + isGrabbingLeft);
        //Debug.Log("Right Grab: " + isGrabbingRight);

        
        amplitude = MapRange(transform.localScale.x, 0.05f, 0.2f, 0.1f, 1f);
       
        if (transform.localScale != previousScale)
        {
            controllerLeft.SendHapticImpulse(amplitude, duration);
            controllerRight.SendHapticImpulse(amplitude, duration);

            previousScale = transform.localScale;
        }

        yOffset = MapRange(transform.localScale.x, 0.05f, 0.2f, 0.035f, 0.1f);
        Debug.Log("yOffset: "+yOffset);
        startPos = new Vector3(platform.transform.localPosition.x, platform.transform.localPosition.y + yOffset, platform.transform.localPosition.z);




    }

    float MapRange(float scale, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (scale - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

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
                Debug.Log("Right Controller is released");
            }
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(correctTag))
            {
            transform.position = startPos;
            transform.rotation = startRot;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}