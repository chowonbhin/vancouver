using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using UnityEngine.InputSystem;
using BaseBallScene;

namespace BaseBallScene
{
    public class Pitcher : MonoBehaviour
    {
        [Header("���� ����")]
        // ���� �������� �������� �����Ϳ��� �Ҵ�
        public Transform startPoint;
        public Transform endPoint;
        public Ball ballPref;
        public Transform balls;
        // ���� �������� �����ϴµ� �ɸ��� �ð� (��)
        public float duration = 1.5f;
        // ���� �ڿ����� ������µ� �ɸ��� �ð� (��)
        public float liveTime = 6f;
        private IObjectPool<Ball> objectPool;


        public InputActionReference throwAction;

        void Start()
        {
            objectPool = new UnityEngine.Pool.ObjectPool<Ball>(createFunc: () =>
            {
                var ball = Instantiate(ballPref, balls, false);
                ball.gameObject.SetActive(false);
                return ball;
            },
            actionOnGet: ball =>
            {
                ball.ResetState();
                ball.gameObject.SetActive(true);
                var tr = ball.GetComponent<Transform>();
                var rb = ball.GetComponent<Rigidbody>();
                //var trajectory = ball.GetComponent<BallTrajectory>();
                //trajectory.ClearTrajectory();
                tr.position = startPoint.position;
                rb.velocity = Vector3.zero;
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
        private IEnumerator ReturnToPoolAfterTime(Ball ball, float time)
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