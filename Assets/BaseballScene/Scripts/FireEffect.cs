using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{

    public ParticleSystem Spire;
    public ParticleSystem Fleart;
    public TrailRenderer Trail;
    public TrailRenderer TrailAdd;



    public void On()
    {
        Spire.Play();
        Fleart.Play();
        Trail.Clear();
        TrailAdd.Clear();
    }

    public void Off()
    {
        Spire.Stop();
        Fleart.Stop();
    }
}
