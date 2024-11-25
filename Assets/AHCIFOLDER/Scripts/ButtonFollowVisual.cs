using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonFollowVisual : MonoBehaviour
{
    public Transform visualTarget;
    public Vector3 localAxis;
    public float resetSpeed = 5;

    private bool freeze = false;

    private Vector3 initialLocalPos;

    private Vector3 offset;
    private Transform pokeAttachTransform;

    private XRBaseInteractable interactable;
    private bool isFollowing;

    [Range(0, 1)]
    public float intensity;
    public float duration;
    [Range(0, 1)]
    public float contHaptic;
    public XRBaseController controller;

    

    

    // Start is called before the first frame update
    void Start()
    {
        initialLocalPos = visualTarget.localPosition;

        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);

        interactable = GetComponent<XRBaseInteractable>();
        Debug.Log("Interable initialized: " + (interactable != null));

        interactable.selectEntered.AddListener((args) =>
        {
            Debug.Log("Select Entered Fired!");
        });

        interactable.hoverEntered.AddListener((args) =>
        {
            Debug.Log("Hover Entered Fired!");
        });

        interactable.hoverExited.AddListener((args) =>
        {
            Debug.Log("Hover Exited Fired!");
        });

        XRBaseInteractable baseInteractable = GetComponent<XRBaseInteractable>();
        
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            isFollowing = true;
            freeze = false;

            pokeAttachTransform = interactor.transform;
            offset = visualTarget.position - pokeAttachTransform.position;
        }
    }

    public void Reset(BaseInteractionEventArgs hover)
    {
        if(hover.interactorObject is XRPokeInteractor)
        {
            
            isFollowing = false;
            freeze = false;
        }
    }

    public void Freeze(BaseInteractionEventArgs hover)
    {
        
        if (hover.interactorObject is XRPokeInteractor)
        {
            
            freeze = true;
            Debug.Log("freeze");
            TriggerHapticFeedback();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (freeze) 
        
            return;

        if (isFollowing)
        {
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);

            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
            ContinuousTriggerHapticFeedback();
        }
        else
        {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialLocalPos, Time.deltaTime * resetSpeed);
        }
    }

    public void TriggerHapticFeedback()
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(intensity, duration);
            Debug.Log("Haptic feedback triggered!");
        }
        else
        {
            Debug.LogWarning("No controller assigned for haptic feedback.");
        }
    }
    public void ContinuousTriggerHapticFeedback()
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(contHaptic, duration);
            Debug.Log("Haptic feedback triggered!");
        }
        else
        {
            Debug.LogWarning("No controller assigned for haptic feedback.");
        }
    }


    /*public void TriggerHaptic(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            TriggerHaptic(controllerInteractor.xrController);
        }
    }
    public void TriggerHaptic(XRBaseController controller)
    {
        if (intensity > 0)
        controller.SendHapticImpulse(intensity, duration);
    }*/

}
