using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HItEffectScript : MonoBehaviour { 

    public ParticleSystem particleSystemEffect;
    public VisualEffect vfxGraphEffect;

    // Start is called before the first frame update
    void Start()
    {
        particleSystemEffect.Play();
        vfxGraphEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            particleSystemEffect.Play();
            vfxGraphEffect.Play();
        }
    }
}
