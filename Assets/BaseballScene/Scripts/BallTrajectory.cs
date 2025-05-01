using UnityEngine;
using System.Collections.Generic;

namespace HP
{
    public class BallTrajectory : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public float lifeTime = 3f;
        public float startWidth = 0.1f;
        public float endWidth = 0.05f;

        private List<Vector3> positions = new List<Vector3>();
        private List<float> timers = new List<float>();
        private float timeSinceLastPoint = 0f;
        private float pointInterval = 0.05f;

        void Start()
        {
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 0;
                lineRenderer.startWidth = startWidth;
                lineRenderer.endWidth = endWidth;
            }
        }

        void Update()
        {
            timeSinceLastPoint += Time.deltaTime;

            if (timeSinceLastPoint >= pointInterval)
            {
                Vector3 currentPosition = transform.position;

                positions.Add(currentPosition);
                timers.Add(0f);

                lineRenderer.positionCount = positions.Count;
                lineRenderer.SetPosition(positions.Count - 1, currentPosition);

                timeSinceLastPoint = 0f;
            }

            // 타이머 업데이트 및 오래된 점 삭제
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i] += Time.deltaTime;

                if (timers[i] > lifeTime)
                {
                    positions.RemoveAt(i);
                    timers.RemoveAt(i);
                    i--;
                }
            }

            // 모든 위치 업데이트
            for (int i = 0; i < positions.Count; i++)
            {
                lineRenderer.SetPosition(i, positions[i]);
            }
        }
        public void ClearTrajectory()
        {
            positions.Clear();
            timers.Clear();
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 0;
            }
        }
    }
}