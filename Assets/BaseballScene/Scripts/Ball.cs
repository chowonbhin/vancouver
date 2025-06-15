using System;
using System.Net;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
namespace BaseBallScene
{
    public class Ball : MonoBehaviour
    {
        public enum RhythmState
        {
            None,
            Hit,
            TimeMiss,
            Miss,
        }
        public enum SwingEvent
        {
            None,
            Left,
            Right
        }
        public Coroutine BadBallCoroutine;
        public Coroutine ParabolicCoroutine;
        public Coroutine ReturnCoroutine;
        public FireEffect FireEffect;
        public RhythmState state;
        private Vector3 Impulse;
        public bool IsHomRun;
        public SwingEvent PitcherE;
        public Vector3 swingDir;
        public float swingStrength;

        Rigidbody rb;
        Collider col;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();

        }

        public void SetImpulseForce(float f)
        {
            Impulse = Impulse.normalized * f;
        }

        public void SetImpulseValue(Vector3 impluse)
        {
            float magnitude = impluse.magnitude;
            if (magnitude > 15f)
            {
                Impulse = impluse.normalized * 15f;
            }
            else if (magnitude < 5f) 
            {
                Impulse = impluse.normalized * 5;
            }
            else
            {
                Impulse = impluse;
            }
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
            rb.velocity = Impulse;
        }
    }
}