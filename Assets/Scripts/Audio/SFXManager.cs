using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SFXManager : MonoBehaviour
{
    public List<SFXLookup> Sounds;
    public AudioSource Source;

    public void PlayPlayerSound(string action)
    {
        Source.clip = Sounds.First(s => s.Action == action).Clip;
        Source.Play();
    }
}

 [Serializable]
 public struct SFXLookup {
     public string Action;
     public AudioClip Clip;
 }