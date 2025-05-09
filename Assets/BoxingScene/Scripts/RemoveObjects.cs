using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PunchBag") || other.CompareTag("Barrel")  || other.CompareTag("Bomba"))
        {
            UnityEngine.VFX.VisualEffect vfx = other.GetComponentInChildren<UnityEngine.VFX.VisualEffect>();
            if (vfx != null)
            {
                vfx.transform.parent = null;
                vfx.Play();
                Destroy(vfx.gameObject, 2f);
            }
            Destroy(other.gameObject);
        }
    }
}
