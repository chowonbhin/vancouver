using UnityEngine;
using System.Collections.Generic;

namespace TT
{
    public class PingPongTrajectory : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public float lifeTime = 2f;
        public float startWidth = 0.03f;
        public float endWidth = 0.05f;

        private List<Vector3> positions = new List<Vector3>();
        private List<float> timers = new List<float>();
        private float timeSinceLastPoint = 0f;
        private float pointInterval = 0.02f;

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
                UpdateTrajectory();

                timeSinceLastPoint = 0f;
            }

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
        }

        void UpdateTrajectory()
        {
            if (positions.Count < 4) return;

            Vector3 start = positions[0];
            Vector3 mid1 = positions[1];
            Vector3 mid2 = positions[2];
            Vector3 end = positions[3];

            lineRenderer.positionCount = 100; // 더 많은 점으로 매끄러운 포물선

            for (int i = 0; i < 100; i++)
            {
                float t = (float)i / 99f;
                Vector3 pos;

                if (t < 0.33f)
                {
                    pos = CalculateParabola(start, mid1, t / 0.33f);
                }
                else if (t < 0.66f)
                {
                    pos = CalculateParabola(mid1, mid2, (t - 0.33f) / 0.33f);
                }
                else
                {
                    pos = CalculateParabola(mid2, end, (t - 0.66f) / 0.34f);
                }

                lineRenderer.SetPosition(i, pos);
            }
        }

        Vector3 CalculateParabola(Vector3 start, Vector3 end, float t)
        {
            float u = 1 - t;
            Vector3 mid = (start + end) / 2 + Vector3.up * Mathf.Abs(Vector3.Distance(start, end) / 3f);
            return u * u * start + 2 * u * t * mid + t * t * end;
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
