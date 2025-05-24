using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
namespace BaseBallScene
{
    public class BatInteractEvent : MonoBehaviour
    {
        float maxSpeed = 10.0f; // ��ƽ �ִ� ������ ���ϴ� �ӵ�
        float duration = 0.1f;

        XRGrabInteractable interactable;
        AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            interactable = GetComponent<XRGrabInteractable>();
            if (interactable != null)
            {
                interactable.hoverEntered.AddListener(OnEvent);
                interactable.hoverExited.AddListener(OnEvent);
                interactable.selectEntered.AddListener(OnEvent);
                interactable.selectExited.AddListener(OnEvent);
            }

            var xrorigin = FindObjectOfType<XROrigin>();
            var XRInputModalityManager = xrorigin.GetComponent<XRInputModalityManager>();
        }
        public void OnEvent(SelectExitEventArgs arg0)
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            var ball = collision.gameObject.GetComponent<Ball>();
            if (ball != null)
            {
                float collisionForce = collision.relativeVelocity.magnitude;
                if (collisionForce != 0)
                {
                    float normalizedSpeed = Mathf.Clamp01(collisionForce / maxSpeed);

                    foreach (var interactor in interactable.interactorsSelecting)
                    {
                        var controllerInteractor = interactor as XRBaseControllerInteractor;
                        var controller = controllerInteractor.xrController as ActionBasedController;
                        controller.SendHapticImpulse(normalizedSpeed, duration);
                        audioSource.Play();
                    }
                }
            }

            if(collision.gameObject.tag != "BaseBall"){
                return;
            }

            if(collision.gameObject.GetComponent<RhythmData>() != null){
                if(InteractionNotifier.Instance != null){
                    InteractionNotifier.Instance.NotifyInteraction(collision.gameObject);
                }
            }

            collision.gameObject.tag = "processed";
        }

        public void OnEvent(SelectEnterEventArgs arg0)
        {
            if (arg0.interactorObject is XRBaseControllerInteractor controllerInteractor)
            {
                var controller = controllerInteractor.xrController as ActionBasedController;
                if (controller.CompareTag("Right Controller"))
                {
                    controller.SendHapticImpulse(0.05f, duration);
                }
            }
        }

        public void OnEvent(HoverExitEventArgs arg0)
        {

        }

        public void OnEvent(HoverEnterEventArgs arg0)
        {

        }
    }
}