using BaseBallScene;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnim : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    public Animator handAnimator;
    public AutoGrabber autoGrabber;


    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);

        if(autoGrabber.interactor.hasSelection)
        {
            handAnimator.SetFloat("Grip", 1.0f);
        }
        else
        {

            float gripValue = gripAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Grip", gripValue);
        }
    }
}
