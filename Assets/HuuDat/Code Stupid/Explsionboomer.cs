using System;
using JetBrains.Annotations;
using UnityEngine;

public class Explsionboomer : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip[] Explsion;
    void Awake()
    {
        AudioSource.PlayOneShot(Explsion[UnityEngine.Random.Range(0,Explsion.Length)]);
        
    }
}
