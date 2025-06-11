using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

namespace BaseBallScene
{
    public class AutoGrabber : MonoBehaviour
    {
        public InputActionReference autoGrabAction;
        public XRBaseInteractor interactor;
        public XRGrabInteractable grabInteractable;

        private void Start()
        {
            autoGrabAction.action.performed += onAutoGrab;
        }
        void onAutoGrab(InputAction.CallbackContext ctx)
        {
            if (ctx.ReadValueAsButton())
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