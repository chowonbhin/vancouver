using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using UnityEngine.InputSystem;
using System.Diagnostics;

namespace BaseBallScene
{
    public class Pitcher : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;
        public Ball ballPref;
        public Transform balls;
        public float duration = 1.5f;
        public float liveTime = 6f;
        private IObjectPool<Ball> objectPool;

        private float currentBeatTime = 0f;

        public InputActionReference throwAction;

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
                ball.state = Ball.RhythmState.None;
                if (ball.gameObject.GetComponent<RhythmData>() == null){
                    RhythmData rhythmData = ball.gameObject.AddComponent<RhythmData>();
                    rhythmData.Initialize(currentBeatTime);
                    UnityEngine.Debug.Log($"리듬 데이터 추가: 오브젝트 '{ball.name}', 타겟시간 {currentBeatTime}");
                }
                else{
                    RhythmData rhythmData = ball.gameObject.GetComponent<RhythmData>();
                    rhythmData.Edit(Time.time, currentBeatTime);
                    UnityEngine.Debug.Log($"리듬 데이터 수정: 오브젝트 '{ball.name}', 타겟시간 {currentBeatTime}");
                }
            },
            actionOnRelease: ball => ball.gameObject.SetActive(false),
            actionOnDestroy: ball => Destroy(ball.gameObject),
            collectionCheck: false,
            defaultCapacity: 50);
            throwAction.action.performed += onThrow;

        }

        private void onThrow(InputAction.CallbackContext ctx)
        {
            if (ctx.ReadValueAsButton())
            {
                ThrowBall(duration, currentBeatTime);
            }
        }

        public GameObject ThrowBall(float duration, float beatTime)
        {
            currentBeatTime = beatTime;
            var ball = objectPool.Get();
            StartCoroutine(ParabolicMovement(ball, duration));
            StartCoroutine(ReturnToPoolAfterTime(ball, liveTime));
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

        private IEnumerator ParabolicMovement(Ball ball, float duration)
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
            if(ball.state != Ball.RhythmState.Hit)
            {
                ball.transform.position = GetPositionAtTime(start, end, duration, timeElapsed);
                Vector3 finalVelocity = GetVelocityAtTime(start, end, duration, timeElapsed);
                ball.SetRBVelocity(finalVelocity);
            }
        }
        private IEnumerator ReturnToPoolAfterTime(Ball ball, float time)
        {
            yield return new WaitForSeconds(time);
            ball.FireEffect.Off();
            ball.gameObject.SetActive(false);
            objectPool.Release(ball);
        }

        private void OnDestroy()
        {
            throwAction.action.performed -= onThrow;
            objectPool.Clear();
        }
    }
}