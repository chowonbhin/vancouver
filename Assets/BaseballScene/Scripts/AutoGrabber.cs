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
                // 레이로 선택한 물체가 없고, 있으면 선택
                if (!interactor.hasSelection) // 레이로 아무것도 선택하지 않았으면
                {
                    //  선택할 수 있는 경우
                    if (grabInteractable != null)
                    {
                        //  강제로 잡기
                        grabInteractable.interactionManager.SelectEnter(interactor as IXRSelectInteractor, grabInteractable);
                    }
                }
            }
        }
    }
}