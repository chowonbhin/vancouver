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

            if (collision.gameObject.CompareTag("Pingpong"))
            {
                checker = true;
            }

        }
        public bool checking()
        {
            return checker;
        }
        void update()
        {
            checker = false;
        }
    }
}
