using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class CollisionManager : MonoBehaviour
    {
        bool checker = false;

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag != "PingpongBall"){
                return;
            }

            if (collision.gameObject.GetComponent<RhythmData>() != null)
            {
                if (InteractionNotifier.Instance != null)
                {
                    InteractionNotifier.Instance.NotifyInteraction(collision.gameObject);
                }
            }

            collision.gameObject.tag = "processed";  

        }
    }
}
