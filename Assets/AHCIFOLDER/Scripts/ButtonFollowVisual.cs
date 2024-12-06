using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Controls the button's movement and interaction with XR interactors like XRPokeInteractor.
/// Includes haptic feedback functionality.
/// </summary>
public class ButtonFollowVisual : MonoBehaviour
{
    [Header("Visual Settings")]
    public Transform visualTarget;         // Target transform for visual feedback
    public Vector3 localAxis;              // Axis to constrain movement
    public float resetSpeed = 5;           // Speed to reset visual position

    [Header("Haptic Feedback")]
    [Range(0, 1)]
    public float intensity;                // Haptic feedback intensity
    public float duration;                 // Haptic feedback duration
    [Range(0, 1)]
    public float contHaptic;               // Continuous haptic feedback intensity
    public XRBaseController controller;    // XR controller for haptic feedback

    private Vector3 initialLocalPos;       // Initial position of the visual target
    private Vector3 offset;                // Offset between target and interactor
    private Transform pokeAttachTransform; // Transform of the interactor
    private XRBaseInteractable interactable; // XR interactable component
    private bool isFollowing = false;      // Whether the visual is following an interactor
    private bool freeze = false;           // Whether movement is frozen

    // Called when the script is initialized
    void Start()
    {
        // Save initial local position of the visual target
        initialLocalPos = visualTarget.localPosition;

        // Get the XR interactable component and attach event listeners
        interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(Follow);
            interactable.hoverExited.AddListener(Reset);
            interactable.selectEntered.AddListener(Freeze);
        }
        else
        {
            Debug.LogWarning("No XRBaseInteractable found on this GameObject.");
        }
    }

    /// <summary>
    /// Called when an interactor enters hover state. Initiates following behavior.
    /// </summary>
    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor interactor)
        {
            isFollowing = true;
            freeze = false;
            pokeAttachTransform = interactor.transform;
            offset = visualTarget.position - pokeAttachTransform.position;
        }
    }

    /// <summary>
    /// Called when an interactor exits hover state. Stops following behavior.
    /// </summary>
    public void Reset(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
            freeze = false;
        }
    }

    /// <summary>
    /// Called when an interactor selects the object. Freezes movement and triggers haptics.
    /// </summary>
    public void Freeze(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            freeze = true;
            Debug.Log("Movement frozen.");
            TriggerHapticFeedback();
        }
    }

    /// <summary>
    /// Updates the visual target's position based on interaction states.
    /// </summary>
    void Update()
    {
        if (freeze) return;

        if (isFollowing)
        {
            // Calculate and constrain the target position along the defined axis
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);
            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);

            // Trigger continuous haptic feedback
            ContinuousTriggerHapticFeedback();
        }
        else
        {
            // Gradually reset the visual target to its initial position
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialLocalPos, Time.deltaTime * resetSpeed);
        }
    }

    /// <summary>
    /// Triggers a single haptic feedback pulse.
    /// </summary>
    public void TriggerHapticFeedback()
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(intensity, duration);
            Debug.Log("Haptic feedback triggered.");
        }
        else
        {
            Debug.LogWarning("No controller assigned for haptic feedback.");
        }
    }

    /// <summary>
    /// Triggers continuous haptic feedback.
    /// </summary>
    public void ContinuousTriggerHapticFeedback()
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(contHaptic, duration);
            Debug.Log("Continuous haptic feedback triggered.");
        }
        else
        {
            Debug.LogWarning("No controller assigned for haptic feedback.");
        }
    }
}
