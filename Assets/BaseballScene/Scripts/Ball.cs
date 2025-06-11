using System;
using System.Net;
using UnityEngine;
namespace BaseBallScene
{
    public class Ball : MonoBehaviour
    {
        public enum RhythmState
        {
            None,
            Hit,
            Miss
        }

        public FireEffect FireEffect;
        public RhythmState state;
        private Vector3 Dir;
        public float force;
        Rigidbody rb;
        Collider col;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();

        }
        public void SetImpulseValue(Vector3 dir)
        {
            Dir = dir.normalized;
        }
        public void SetIsTrigger(bool b)
        {
            col.isTrigger = b;
        }
        public void SetIsKinematic(bool b)
        {
            rb.isKinematic = b;
        }
        public void SetRBVelocity(Vector3 vel)
        {
            rb.velocity = vel;
        }
        public void Impuse()
        {
            rb.velocity = Dir * force;
        }
    }
}