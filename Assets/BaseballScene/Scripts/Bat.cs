using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;
using UnityEngine.XR;
using static BaseBallScene.Ball;


namespace BaseBallScene
{
    public class Bat : MonoBehaviour
    {
        public Transform batTip;
        public Transform batHandle;

        public TMPro.TextMeshProUGUI PitcherInfo;
        public GameObject hitVfxObj;
        public AudioSource PositiveHit;
        public AudioSource NegativeHit;

        public PitcherEvent pitcherEvent;
        public Volume volume; 
        private ChromaticAberration chromatic;

        Coroutine chromaticEffect;

        VisualEffect hitVfx;
        BatInteractEvent BatInteract;
        private void Start()
        {
            BatInteract = GetComponent<BatInteractEvent>();
            hitVfx = hitVfxObj.GetComponentInChildren<VisualEffect>();

            if (volume.profile.TryGet(out chromatic))
            {
                chromatic.intensity.value = 0f;
            }

            if (JudgmentSystem.Instance != null)
            {
                JudgmentSystem.Instance.OnJudgment += Instance_OnJudgment;
            }
        }


        bool CheckEvent(Ball ball)
        {
            bool BadEventSwing = false;
            bool isLeftSwing = ball.swingDir.x < 0f;
            bool isRightSwing = ball.swingDir.x > 0f;

            if (ball.PitcherE == Ball.SwingEvent.Left)
            {
                if (isRightSwing)
                {
                    BadEventSwing = true;

                    Debug.Log("PitcherEvent : Left Swing EVENT, But BadSwing...");

                }
            }
            else if (isLeftSwing)
            {
                if (Vector3.Dot(ball.swingDir, Vector3.right) < 0f)
                {
                    BadEventSwing = true;
                    Debug.Log("PitcherEvent : Right Swing EVENT, But BadSwing...");
                }
            }

            if(!BadEventSwing)
            {
                Debug.Log("PitcherEvent : Performed Swing Event Very Well!");
            }

            return BadEventSwing;
        }

        public void Miss(Ball ball)
        {
            if(ball.state != Ball.RhythmState.Miss)
            {
                ball.state = Ball.RhythmState.TimeMiss;
                NegativeHit.Play();
                ball.SetImpulseValue(Vector3.zero);
                ball.Impuse();
            }
            BatInteract.TriggerHapticsForSelector(0.2f, 0.1f);
        }

        void Hit(Ball ball)
        {
            ball.state = Ball.RhythmState.Hit;
            bool BadEventSwing = CheckEvent(ball);
            if (ball.IsHomRun)
            {
                HomeRun(ball);
            }
            else
            {
                BatInteract.TriggerHapticsForSelector(0.5f, 0.2f);
            }

            if (BadEventSwing)
            {
                PitcherInfo.text = "BadSwing";
                JudgmentSystem.Instance.UpdateScore(-5, "PitcherEvent : BadSwing");
                pitcherEvent.StartBadBallCoroutine(ball);
            }
            else
            {
                ball.Impuse();
            }
            hitVfxObj.transform.position = ball.gameObject.transform.position;
            hitVfx.Play();
            PositiveHit.Play();
        }

        void HomeRun(Ball ball)
        {
            PitcherInfo.text = "HomeRun!";
            JudgmentSystem.Instance.UpdateScore(2, "HomeRun Swing");
            ball.SetImpulseForce(20);
            ball.FireEffect.OnSpecial();
            if (chromaticEffect != null)
            {
                StopCoroutine(chromaticEffect);
                chromaticEffect = null;
            }
            chromaticEffect = StartCoroutine(ChromaticEffectCoroutine());
            BatInteract.TriggerHapticsForSelector(0.9f, 0.4f);
        }

        private void Instance_OnJudgment(GameObject obj, JudgmentResult arg2, float arg3)
        {
            Ball ball = obj.GetComponent<Ball>();
            if(ball != null)
            {
                PitcherInfo.text = "";
                if (arg2 == JudgmentResult.Good || arg2 == JudgmentResult.Perfect)
                {
                    Hit(ball);
                }
                else
                {
                    Miss(ball);
                }
            }
        }

        IEnumerator ChromaticEffectCoroutine()
        {
            chromatic.intensity.value = 0.25f;
            float duration = 0.5f;
            float elapsed = 0f;
            float startValue = chromatic.intensity.value;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                chromatic.intensity.value = Mathf.Lerp(startValue, 0f, t);
                yield return null;
            }
            chromatic.intensity.value = 0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            var ball = other.gameObject.GetComponent<Ball>();
            if (ball == null || ball.state != Ball.RhythmState.None) return;

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
                    ball.swingDir = swingDir;
                    ball.swingStrength = swingStrength;
                    if (swingStrength > 5)
                    {
                        Vector3 impulse = swingDir * swingStrength;
                        ball.SetImpulseValue(impulse);
                        float upwardAngle = Vector3.Angle(swingDir, Vector3.up);
                        bool isUpperSwing = upwardAngle >= 20f && upwardAngle <= 45f;
                        bool isStrongEnough = swingStrength > 10f;
                        ball.IsHomRun = isUpperSwing && isStrongEnough;
                        if (InteractionNotifier.Instance !=null)
                        {
                            InteractionNotifier.Instance.NotifyInteraction(ball.gameObject);
                        }
                    }
                }
            }
        }
    }
}