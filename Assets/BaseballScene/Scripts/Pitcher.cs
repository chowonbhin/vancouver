using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System.Collections;

namespace HP
{
    public class Pitcher : MonoBehaviour
    {
        [Header("���� ����")]
        // ���� �������� �������� �����Ϳ��� �Ҵ�
        public Transform startPoint;
        public Transform endPoint;
        public GameObject ballPref;
        public Transform balls;
        // ���� �������� �����ϴµ� �ɸ��� �ð� (��)
        public float duration = 1.5f;
        // ���� �ڿ����� ������µ� �ɸ��� �ð� (��)
        public float liveTime = 6f;
        private IObjectPool<GameObject> objectPool;

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
                ThrowBall(duration);
            }
        }
    }
}