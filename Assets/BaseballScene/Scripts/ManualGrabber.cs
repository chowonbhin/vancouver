using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine;

namespace HP
{
    public class ManualGrabber : MonoBehaviour
    {
        public XRBaseInteractor interactor;
        public XRNode node;
        public XRGrabInteractable grabInteractable;
        public InputHelpers.Button grabButton = InputHelpers.Button.PrimaryButton;
        void Update()
        {
            // InputDevice���� Grab ��ư ���� ��������
            InputDevice device = InputDevices.GetDeviceAtXRNode(node);
            bool isPressed = false;

            if (device.isValid)
            {
                InputHelpers.IsPressed(device, grabButton, out isPressed);
            }

            // Grab ��ư�� ������ �� ����
            if (isPressed && grabInteractable)
            {
                // ���̷� ������ ��ü�� ����, ������ ����
                if (!interactor.hasSelection) // ���̷� �ƹ��͵� �������� �ʾ�����
                {
                    //  ������ �� �ִ� ���
                    if (grabInteractable != null)
                    {
                        //  ������ ���
                        grabInteractable.interactionManager.SelectEnter(interactor as IXRSelectInteractor, grabInteractable);
                    }
                }
            }
        }
    }
}