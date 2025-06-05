using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
namespace BaseBallScene
{
    public class BatInteractEvent : MonoBehaviour
    {
        public float maxSpeed = 10.0f;
        public float duration = 0.1f;

        XRGrabInteractable interactable;

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
            var XRInputModalityManager = xrorigin.GetComponent<XRInputModalityManager>();
        }
        public void OnEvent(SelectExitEventArgs arg0)
        {

        }

        // 현재 배트 오브젝트를 선택한 XR 컨트롤러에 햅틱 피드백을 제공하는 메서드
        public void TriggerHapticsForSelector(float amplitude,float duration)
        {
            foreach (var interactor in interactable.interactorsSelecting)
            {
                var controllerInteractor = interactor as XRBaseControllerInteractor;
                var controller = controllerInteractor.xrController as ActionBasedController;
                controller.SendHapticImpulse(amplitude, duration);
            }
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