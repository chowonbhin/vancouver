using System;
using UnityEngine;
namespace BaseBallScene
{
    public class Ball : MonoBehaviour
    {
        public enum BallState
        {
            Normal,
            Hit,
            Miss,
        }

        BallState currentState = BallState.Normal;

        public void ResetState()
        {
            currentState = BallState.Normal;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag("Ground"))
            {
                if (currentState == BallState.Normal)
                {
                    currentState = BallState.Miss;
                    OnStateChange();
                }
            }
            else
            {
                var bat = collision.gameObject.GetComponent<Bat>();

                if (bat != null && currentState == BallState.Normal)
                {
                    currentState = BallState.Hit;
                    OnStateChange();
                }
            }
        }

        void OnStateChange()
        {
            switch (currentState)
            {
                case BallState.Normal:
                    break;
                case BallState.Hit:
                    ScoreSystem.instance.Hit();
                    break;
                case BallState.Miss:
                    ScoreSystem.instance.Miss();
                    break;
                default:
                    break;
            }
        }
    }
}