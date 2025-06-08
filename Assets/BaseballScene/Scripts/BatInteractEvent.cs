using UnityEngine.XR.Interaction.Toolkit;
namespace BaseBallScene
{
    public class BatInteractEvent : InteractorEvent
    {
        public float maxSpeed = 10.0f;
        public float duration = 0.1f;
        

        public override void OnEvent(SelectEnterEventArgs arg)
        {
            base.OnEvent(arg);
            CurrentGrabDevice.SendHapticImpulse(0.05f, duration);
        }
    }
}