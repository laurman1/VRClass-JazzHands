using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Provides haptic feedback when an object is grabbed using XR interaction.
/// </summary>
public class HapticInteractable : MonoBehaviour
{
    [Header("Haptic Settings")]
    [Range(0, 1)]
    public float hapticAmplitude;  // Intensity of the vibration (0.0 to 1.0)
    public float hapticInterval;   // Time between haptic pulses in seconds

    [Header("Interaction Settings")]
    public float waitTime = 0.1f;  // Delay before haptic feedback starts (might be used for staggered effects)

    private XRBaseController controller;         // Reference to the controller interacting with the object
    private XRGrabInteractable grabInteractable; // Reference to the XR grab interaction component
    private bool isGrabbing = false;             // Flag to track if the object is being held

    /// <summary>
    /// Initializes the component and sets up interaction listeners.
    /// </summary>
    private void Awake()
    {
        // Get the XRGrabInteractable component on this GameObject
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Add event listeners for grab and release actions
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    /// <summary>
    /// Cleans up event listeners when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    /// <summary>
    /// Triggered when the object is grabbed. Starts the haptic feedback routine.
    /// </summary>
    /// <param name="args">Details about the grab interaction.</param>
    private void OnGrab(SelectEnterEventArgs args)
    {
        // Ensure the interactor is a controller and assign it
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            controller = controllerInteractor.xrController;
            isGrabbing = true;

            // Start the haptic feedback coroutine
            StartCoroutine(HapticFeedbackRoutine());
        }
    }

    /// <summary>
    /// Triggered when the object is released. Stops the haptic feedback routine.
    /// </summary>
    /// <param name="args">Details about the release interaction.</param>
    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbing = false; // Stop the haptic feedback routine
    }

    /// <summary>
    /// Coroutine that continuously sends haptic feedback while the object is being grabbed.
    /// </summary>
    private IEnumerator HapticFeedbackRoutine()
    {
        // Wait before starting the haptic feedback (optional delay)
        yield return new WaitForSeconds(waitTime);

        // Continue sending haptic pulses while the object is being grabbed
        while (isGrabbing && controller != null)
        {
            controller.SendHapticImpulse(hapticAmplitude, hapticInterval); // Send a haptic pulse
            yield return new WaitForSeconds(hapticInterval);              // Wait for the next pulse
        }
    }
}
