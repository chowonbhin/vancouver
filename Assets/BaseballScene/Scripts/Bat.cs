using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR;


namespace BaseBallScene
{
    public class Bat : MonoBehaviour
    {
        public Transform batTip;
        public Transform batHandle;

        public GameObject hitVfxObj;
        AudioSource audioSource;
        VisualEffect hitVfx;
        BatInteractEvent BatInteract;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            BatInteract = GetComponent<BatInteractEvent>();
            hitVfx = hitVfxObj.GetComponentInChildren<VisualEffect>();
            if(JudgmentSystem.Instance != null)
            {
                JudgmentSystem.Instance.OnJudgment += Instance_OnJudgment;
            }
        }

        private void Instance_OnJudgment(GameObject obj, JudgmentResult arg2, float arg3)
        {
            Ball ball = obj.GetComponent<Ball>();
            if(ball != null)
            {
                if (arg2 == JudgmentResult.Good || arg2 == JudgmentResult.Perfect)
                {
                    ball.state = Ball.RhythmState.Hit;
                    ball.Impuse();
                    hitVfxObj.transform.position = ball.gameObject.transform.position;
                    hitVfx.Play();
                    audioSource.Play();
                }
                else
                {

                    ball.state = Ball.RhythmState.Miss;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var ball = other.gameObject.GetComponent<Ball>();
            if (ball == null || ball.state != Ball.RhythmState.None) return;

            Debug.Log("Cooi");

            var rhythmData = ball.gameObject.GetComponent<RhythmData>();

            if (BatInteract.CurrentGrabInputDevice.HasValue)
            {
                var inputDevice = BatInteract.CurrentGrabInputDevice.Value;

                if (inputDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 localAngularVel))
                {
                    Vector3 angularVelWorld = BatInteract.CurrentGrabDevice.transform.TransformDirection(localAngularVel);

                    Vector3 batDirection = batTip.position - batHandle.position;

                    Vector3 swingDir = Vector3.Cross(angularVelWorld, batDirection).normalized;

                    float swingStrength = angularVelWorld.magnitude * batDirection.magnitude;
                    if (swingStrength > 5)
                    {
                        Vector3 impulse = swingDir * swingStrength;
                        ball.SetImpulseValue(impulse);
                        InteractionNotifier.Instance.NotifyInteraction(ball.gameObject);
                    }
                }
            }
        }
    }
}