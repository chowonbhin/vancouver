using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine;

namespace HP
{
    public class ManualGrabber : MonoBehaviour
    {
        public XRBaseInteractor rightHandInteractor;   // ������ Interactor (Direct Interactor)
        public XRGrabInteractable grabInteractable;
        public InputHelpers.Button grabButton = InputHelpers.Button.PrimaryButton;

        void Update()
        {
            // ������ InputDevice���� Grab ��ư ���� ��������
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            bool isPressed = false;

            if (device.isValid)
            {
                InputHelpers.IsPressed(device, grabButton, out isPressed);
            }

            // Grab ��ư�� ������ �� ����
            if (isPressed && grabInteractable)
            {
                // �������� ���̷� ������ ��ü�� ����, ������ ����
                if (!rightHandInteractor.hasSelection) // ���̷� �ƹ��͵� �������� �ʾ�����
                {
                    //  ������ �� �ִ� ���
                    if (grabInteractable != null)
                    {
                        //  ������ ���
                        var interactor =  rightHandInteractor as IXRSelectInteractor;
                        grabInteractable.interactionManager.SelectEnter(interactor, grabInteractable);
                    }
                }
            }
        }
    }
}