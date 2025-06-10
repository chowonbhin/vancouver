using UnityEngine.XR.Interaction.Toolkit;
namespace BaseBallScene
{
    public class BatInteractEvent : InteractorEvent
    {
        
        public override void OnEvent(SelectEnterEventArgs arg)
        {
            base.OnEvent(arg);
            CurrentGrabDevice.SendHapticImpulse(0.05f, 0.05f);
        }
    }
}