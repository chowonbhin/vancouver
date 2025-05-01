using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine;

namespace HP
{
    public class ManualGrabber : MonoBehaviour
    {
        public XRBaseInteractor rightHandInteractor;   // 오른손 Interactor (Direct Interactor)
        public XRGrabInteractable grabInteractable;
        public InputHelpers.Button grabButton = InputHelpers.Button.PrimaryButton;

        void Update()
        {
            // 오른손 InputDevice에서 Grab 버튼 상태 가져오기
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            bool isPressed = false;

            if (device.isValid)
            {
                InputHelpers.IsPressed(device, grabButton, out isPressed);
            }

            // Grab 버튼이 눌렸을 때 동작
            if (isPressed && grabInteractable)
            {
                // 오른손이 레이로 선택한 물체가 없고, 있으면 선택
                if (!rightHandInteractor.hasSelection) // 레이로 아무것도 선택하지 않았으면
                {
                    //  선택할 수 있는 경우
                    if (grabInteractable != null)
                    {
                        //  강제로 잡기
                        var interactor =  rightHandInteractor as IXRSelectInteractor;
                        grabInteractable.interactionManager.SelectEnter(interactor, grabInteractable);
                    }
                }
            }
        }
    }
}