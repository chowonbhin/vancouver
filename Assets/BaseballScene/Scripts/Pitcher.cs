using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System.Collections;
using UnityEngine.InputSystem;
using System;

namespace HP
{
    public class Pitcher : MonoBehaviour
    {
        [Header("투구 설정")]
        // 투구 시작점과 종료점을 에디터에서 할당
        public Transform startPoint;
        public Transform endPoint;
        public GameObject ballPref;
        public Transform balls;
        // 공이 목적지에 도달하는데 걸리는 시간 (초)
        public float duration = 1.5f;
        // 공이 자연ㄴ히 사라지는데 걸리는 시간 (초)
        public float liveTime = 6f;
        private IObjectPool<GameObject> objectPool;


        public InputActionReference throwAction;

        void Start()
        {
            objectPool = new UnityEngine.Pool.ObjectPool<GameObject>(createFunc: () =>
            {
                var ball = Instantiate(ballPref, balls, false);
                ball.gameObject.SetActive(false);
                return ball;
            },
            actionOnGet: ball =>
            {
                ball.gameObject.SetActive(true);
                var tr = ball.GetComponent<Transform>();
                var rb = ball.GetComponent<Rigidbody>();
                var trajectory = ball.GetComponent<BallTrajectory>();
                tr.position = startPoint.position;
                rb.velocity = Vector3.zero;
                trajectory.ClearTrajectory();
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
                ThrowBall(duration);
            }
        }

        public void ThrowBall(float duration)
        {
            var ball = objectPool.Get();
            var tr = ball.GetComponent<Transform>();
            var rb = ball.GetComponent<Rigidbody>();
            Vector3 distance = endPoint.position - startPoint.position;
            Vector3 initialVelocity = distance / duration - 0.5f * Physics.gravity * duration;
            rb.velocity = initialVelocity;
            StartCoroutine(ReturnToPoolAfterTime(ball, liveTime));
        }
        private IEnumerator ReturnToPoolAfterTime(GameObject ball, float time)
        {
            yield return new WaitForSeconds(time);
            objectPool.Release(ball);
        }

        private void OnDestroy()
        {
            throwAction.action.performed -= onThrow;
        }
    }
}