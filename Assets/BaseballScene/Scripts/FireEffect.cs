using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{

    public ParticleSystem Spire;
    public ParticleSystem Fleart;
    public TrailRenderer Trail;
    public TrailRenderer TrailAdd;

    private void Awake()
    {
        Off();
    }

    public void OnSpecial()
    {
        Spire.Play();
    }

    public void On()
    {
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
