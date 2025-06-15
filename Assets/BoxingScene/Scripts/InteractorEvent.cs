using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
namespace BaseBallScene
{
    public class InteractorEvent : MonoBehaviour
    {
        XRInputModalityManager XRInputModalityManager;
        public ActionBasedController CurrentGrabDevice;
        public InputDevice? CurrentGrabInputDevice;
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
            XRInputModalityManager = xrorigin.GetComponent<XRInputModalityManager>();
        }


        public virtual void OnEvent(SelectExitEventArgs arg)
        {
            var controllerInteractor = arg.interactorObject as XRBaseControllerInteractor;
            SetCurrentGrabDevice(null);
        }

        public virtual void TriggerHapticsForSelector(float amplitude, float duration)
        {
            foreach (var interactor in interactable.interactorsSelecting)
            {
                var controllerInteractor = interactor as XRBaseControllerInteractor;
                var controller = controllerInteractor.xrController as ActionBasedController;
                controller.SendHapticImpulse(amplitude, duration);
            }
        }

        void SetCurrentGrabDevice(ActionBasedController controller)
        {
            if (controller == null)
            {
                CurrentGrabDevice = null;
                CurrentGrabInputDevice = null;
            }
            else
            {
                CurrentGrabDevice = controller;
                if (CurrentGrabDevice.gameObject == XRInputModalityManager.rightController.gameObject)
                {
                    CurrentGrabInputDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                }
                else
                {
                    CurrentGrabInputDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                }
            }
        }

        public virtual void OnEvent(SelectEnterEventArgs arg)
        {
            var controllerInteractor = arg.interactorObject as XRBaseControllerInteractor;
            if (controllerInteractor != null)
            {
                var device = controllerInteractor.xrController as ActionBasedController;
                SetCurrentGrabDevice(device);
            }
        }

        public virtual void OnEvent(HoverExitEventArgs arg)
        {

        }

        public virtual void OnEvent(HoverEnterEventArgs arg0)
        {

        }
    }
}
