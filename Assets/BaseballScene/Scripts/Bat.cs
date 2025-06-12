using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;
using UnityEngine.XR;


namespace BaseBallScene
{
    public class Bat : MonoBehaviour
    {
        public Transform batTip;
        public Transform batHandle;

        public GameObject hitVfxObj;
        public AudioSource PositiveHit;
        public AudioSource NegativeHit;

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

        private void Instance_OnJudgment(GameObject obj, JudgmentResult arg2, float arg3)
        {
            Ball ball = obj.GetComponent<Ball>();
            if(ball != null)
            {
                if (arg2 == JudgmentResult.Good || arg2 == JudgmentResult.Perfect)
                {
                    ball.state = Ball.RhythmState.Hit;
                    if (ball.IsHomRun)
                    {
                        ball.FireEffect.OnSpecial();
                        if(chromaticEffect!= null)
                        {
                            StopCoroutine(chromaticEffect);
                            chromaticEffect = null;
                        }
                        chromaticEffect = StartCoroutine(ChromaticEffectCoroutine());
                    }
                    ball.Impuse();
                    hitVfxObj.transform.position = ball.gameObject.transform.position;
                    hitVfx.Play();
                    PositiveHit.Play();
                }
                else
                {
                    ball.SetImpulseValue(Vector3.zero);
                    ball.Impuse();
                    NegativeHit.Play();
                    ball.state = Ball.RhythmState.Miss;
                }
            }
        }

        IEnumerator ChromaticEffectCoroutine()
        {
            chromatic.intensity.value = 0.2f;
            float duration = 0.8f;
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