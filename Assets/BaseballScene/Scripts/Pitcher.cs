using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using System.Diagnostics;

namespace BaseBallScene
{
    public class Pitcher : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;
        public Ball ballPref;
        public Bat bat;
        public Transform balls;
        public float duration = 1.5f;
        public float liveTime = 6f;
        private IObjectPool<Ball> objectPool;
        public PitcherEvent pitcherEvent;
        void Start()
        {
            objectPool = new UnityEngine.Pool.ObjectPool<Ball>(createFunc: () =>
            {
                var ball = Instantiate(ballPref, balls, false);
                ball.gameObject.SetActive(false);
                ball.state = Ball.RhythmState.None;
                ball.tag = "BaseBall";
                return ball;
            },
            actionOnGet: ball =>
            {
                var tr = ball.GetComponent<Transform>();
                tr.position = startPoint.position;
                ball.gameObject.SetActive(true);
                ball.FireEffect.On();
                ball.IsHomRun = false;
                ball.state = Ball.RhythmState.None;

                float randomValue = Random.value;
                if (randomValue < 0.2f)
                {
                    ball.PitcherE =  (Random.value < 0.5f)? Ball.SwingEvent.Right : Ball.SwingEvent.Left;
                }
                else
                {
                    ball.PitcherE = Ball.SwingEvent.None;
                }
            },
            actionOnRelease: ball => ball.gameObject.SetActive(false),
            actionOnDestroy: ball => Destroy(ball.gameObject),
            collectionCheck: false,
            defaultCapacity: 50);
        }


        public GameObject ThrowBall(float duration, float beatTime)
        {
            // currentBeatTime = beatTime;
            var ball = objectPool.Get();
            pitcherEvent.OnEventImg(ball.PitcherE);
            ball.ParabolicCoroutine =StartCoroutine(ParabolicMovement(ball, duration, beatTime));
            ball.ReturnCoroutine = StartCoroutine(ReturnToPoolAfterTime(ball, liveTime));
            if (ball.tag != "BaseBall")
            {
                ball.tag = "BaseBall";
            }
            return ball.gameObject;

        }

        Vector3 GetPositionAtTime(Vector3 start, Vector3 end, float duration, float t)
        {
            Vector3 g = Physics.gravity;
            Vector3 initialVelocity = (end - start - 0.5f * g * duration * duration) / duration;
            return start + initialVelocity * t + 0.5f * g * t * t;
        }

        // t 시점 속도 반환
        Vector3 GetVelocityAtTime(Vector3 start, Vector3 end, float duration, float t)
        {
            Vector3 g = Physics.gravity;
            Vector3 initialVelocity = (end - start - 0.5f * g * duration * duration) / duration;
            return initialVelocity + g * t;
        }

        private IEnumerator ParabolicMovement(Ball ball, float duration,float beatTime)
        {
            Vector3 start = startPoint.position;
            Vector3 end = endPoint.position;
            float timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                if (ball.state == Ball.RhythmState.Hit)
                {
                    break;
                }
                timeElapsed += Time.deltaTime;
                ball.transform.position = GetPositionAtTime(start, end, duration, timeElapsed);
                yield return null;
            }
            if (ball.state == Ball.RhythmState.None)
            {
                ball.transform.position = GetPositionAtTime(start, end, duration, timeElapsed);
                Vector3 finalVelocity = GetVelocityAtTime(start, end, duration, timeElapsed);
                ball.SetRBVelocity(finalVelocity);
            }

            while (timeElapsed < duration + 1.0f)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            if (ball.state == Ball.RhythmState.None)
            {
                if (InteractionNotifier.Instance != null)
                {
                    ball.state = Ball.RhythmState.Miss;
                    InteractionNotifier.Instance.NotifyInteraction(ball.gameObject);
                }
            }
        }



        public void ReturnBall(Ball ball)
        {
            ball.FireEffect.Off();
            if(ball.ReturnCoroutine != null)
            {
                StopCoroutine(ball.ReturnCoroutine);
                ball.ReturnCoroutine = null;
            }
            if(ball.ParabolicCoroutine != null)
            {
                StopCoroutine(ball.ParabolicCoroutine);
                ball.ParabolicCoroutine = null;
            }

            ball.gameObject.SetActive(false);
            objectPool.Release(ball);
        }

        private IEnumerator ReturnToPoolAfterTime(Ball ball, float time)
        {
            yield return new WaitForSeconds(time);
            ReturnBall(ball);
        }

        private void OnDestroy()
        {
            objectPool.Clear();
        }
    }
}