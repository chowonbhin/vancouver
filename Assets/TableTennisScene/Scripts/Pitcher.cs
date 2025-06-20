using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System.Collections;

namespace TT
{
    public class Pitcher : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;
        public Transform midpoint1;
        public Transform midpoint2;

        public GameObject ballPref;
        public Transform balls;
    
        public float duration = 1f;
        public float liveTime = 6f;
        private IObjectPool<GameObject> objectPool;

        private bool isThrowing = false; // 던지기 중복 방지 플래그

        private float currentBeatTime = 0f;

        void Start()
        {
            objectPool = new UnityEngine.Pool.ObjectPool<GameObject>(createFunc: () =>
            {
                var ball = Instantiate(ballPref, balls, false);
                ball.gameObject.SetActive(false);
                ball.tag = "PingpongBall";
                return ball;
            },
            actionOnGet: ball =>
            {
                ball.gameObject.SetActive(true);
                var tr = ball.GetComponent<Transform>();
                var rb = ball.GetComponent<Rigidbody>();
                var trajectory = ball.GetComponent<PingPongTrajectory>();
                tr.position = startPoint.position;
                rb.velocity = Vector3.zero;
                trajectory.ClearTrajectory();

                // 새 RhythmData 추가 및 초기화
                if(ball.GetComponent<RhythmData>() == null){
                    RhythmData rhythmData = ball.AddComponent<RhythmData>();
                    rhythmData.Initialize(currentBeatTime);
                    Debug.Log($"리듬 데이터 추가: 오브젝트 '{ball.name}', 타겟시간 {currentBeatTime}");
                }
                else{
                    RhythmData rhythmData = ball.GetComponent<RhythmData>();
                    rhythmData.Edit(Time.time, currentBeatTime);
                    Debug.Log($"리듬 데이터 수정: 오브젝트 '{ball.name}', 타겟시간 {currentBeatTime}");
                }
                
            },
            actionOnRelease: ball => ball.gameObject.SetActive(false),
            actionOnDestroy: ball => Destroy(ball.gameObject),
            collectionCheck: false,
            defaultCapacity: 50);
        }

        public GameObject ThrowBall(float duration, float beatTime)
        {
            if (isThrowing) return null; // 이미 던지고 있다면 종료

            // 현재 비트 시간 저장 (actionOnGet에서 사용)
            currentBeatTime = beatTime;

            isThrowing = true;
            var ball = objectPool.Get();
            var tr = ball.GetComponent<Transform>();
            StartCoroutine(ParabolicMovement(ball, duration));
            StartCoroutine(ReturnToPoolAfterTime(ball, liveTime));

            if(ball.tag != "PingpongBall"){
                ball.tag = "PingpongBall";
            }

            return ball; // 생성된 공 반환
        }

        private IEnumerator ParabolicMovement(GameObject ball, float duration)
        {
            Vector3[] points = { startPoint.position, midpoint2.position, midpoint1.position, endPoint.position };

            for (int i = 0; i < points.Length - 1; i++)
            {
                Vector3 start = points[i];
                Vector3 end = points[i + 1];
                float segmentDuration = duration / (points.Length - 1);

                Vector3 distance = end - start;
                Vector3 velocity = (distance / segmentDuration) - (0.5f * Physics.gravity * segmentDuration);

                float timeElapsed = 0f;

                while (timeElapsed < segmentDuration)
                {
                    timeElapsed += Time.deltaTime;
                    Vector3 gravityEffect = Physics.gravity * timeElapsed * timeElapsed * 0.5f;
                    ball.transform.position = start + (velocity * timeElapsed) + gravityEffect;
                    yield return null;
                }
            }

            ball.transform.position = endPoint.position;
            isThrowing = false; // 던지기 종료
        }

        private IEnumerator ReturnToPoolAfterTime(GameObject ball, float time)
        {
            yield return new WaitForSeconds(time);
            objectPool.Release(ball);
        }

        void Update()
        {
            UnityEngine.XR.InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            InputHelpers.Button button = InputHelpers.Button.PrimaryButton;
            bool isPressed = false;

            if (device.isValid)
            {
                InputHelpers.IsPressed(device, button, out isPressed);
            }

            if (isPressed)
            {
                ThrowBall(duration, Time.time);
            }
        }
    }
}
