using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine;

namespace TT
{
    public class ManualPaddleGrabber : MonoBehaviour
    {
        public XRBaseInteractor rightHandInteractor;
        public XRGrabInteractable grabInteractable;
        public InputHelpers.Button grabButton = InputHelpers.Button.PrimaryButton;

        void Update()
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            bool isPressed = false;

            if (device.isValid)
            {
                InputHelpers.IsPressed(device, grabButton, out isPressed);
            }

            if (isPressed && grabInteractable)
            {
                if (!rightHandInteractor.hasSelection)
                {
                    if (grabInteractable != null)
                    {
                        var interactor = rightHandInteractor as IXRSelectInteractor;
                        grabInteractable.interactionManager.SelectEnter(interactor, grabInteractable);
                    }
                }
            }
        }
    }
}
