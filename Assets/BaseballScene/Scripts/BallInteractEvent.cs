using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class BallInteractEvent : MonoBehaviour
{
    float maxSpeed = 5.0f; // 햅틱 최대 강도에 응하는 속도
    float duration = 0.1f;
    XRGrabInteractable interactable;
    XRInputModalityManager XRInputModalityManager;
    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(OnEvent);
            interactable.hoverExited.AddListener(OnEvent);
            interactable.selectEntered.AddListener(OnEvent);
            interactable.selectExited.AddListener(OnEvent);
        }

        var xrorigin = FindObjectOfType<XROrigin>();
        XRInputModalityManager = xrorigin.GetComponent<XRInputModalityManager>();
    }
    public void OnEvent(SelectExitEventArgs arg0)
    {

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Left Controller"))
        {
            float collisionForce = collision.relativeVelocity.magnitude;
            if (collisionForce != 0)
            {
                float normalizedSpeed = Mathf.Clamp01(collisionForce / maxSpeed);
                var controller = XRInputModalityManager.leftController.GetComponent<ActionBasedController>();
                controller.SendHapticImpulse(normalizedSpeed, duration);
            }
        }
        if (collision.gameObject.CompareTag("Right Controller"))
        {
            float collisionForce = collision.relativeVelocity.magnitude;
            if(collisionForce != 0)
            {
                float normalizedSpeed = Mathf.Clamp01(collisionForce / maxSpeed);
                var controller = XRInputModalityManager.rightController.GetComponent<ActionBasedController>();
                controller.SendHapticImpulse(normalizedSpeed, duration);
            }
        }
    }
    public void OnEvent(SelectEnterEventArgs arg0)
    {

    }

    public void OnEvent(HoverExitEventArgs arg0)
    {

    }

    public void OnEvent(HoverEnterEventArgs arg0)
    {

    }
}
