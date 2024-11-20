using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticInteractable : MonoBehaviour
{
    [Range(0,1)]
    public float hapticAmplitude;  // Intensity of vibration (0.0 to 1.0)
    public float hapticInterval;   // Interval between haptic pulses in seconds

    private XRBaseController controller;
    private XRGrabInteractable grabInteractable;
    private bool isGrabbing = false;
    public float waitTime = 0.1f;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
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

            controller = controllerInteractor.xrController;
            isGrabbing = true;
            StartCoroutine(HapticFeedbackRoutine());
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbing = false;
    }

    private IEnumerator HapticFeedbackRoutine()
    {
        yield return new WaitForSeconds(waitTime);
        while (isGrabbing && controller != null)
        {
            controller.SendHapticImpulse(hapticAmplitude, hapticInterval);
            yield return new WaitForSeconds(hapticInterval);
        }
    }

}
