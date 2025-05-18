using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR.Interaction.Toolkit;


namespace BaseBallScene
{
    public class Bat : MonoBehaviour
    {
        public GameObject hitVfxObj;
        AudioSource audioSource;
        VisualEffect hitVfx;


        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            hitVfx = hitVfxObj.GetComponentInChildren<VisualEffect>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            var ball = collision.gameObject.GetComponent<Ball>();
            if (ball != null)
            {
                float collisionForce = collision.relativeVelocity.magnitude;
                if (collisionForce != 0)
                {
                    var batInteractEvent = GetComponent<BatInteractEvent>();
                    float normalizedSpeed = Mathf.Clamp01(collisionForce / batInteractEvent.maxSpeed);
                    batInteractEvent.TriggerHapticsForSelector(normalizedSpeed, batInteractEvent.duration);

                    hitVfxObj.transform.position = ball.gameObject.transform.position;
                    hitVfx.Play();
                    audioSource.Play();
                }
            }
        }
    }
}