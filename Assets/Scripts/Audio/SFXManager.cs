using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SFXManager : MonoBehaviour
{
    public bool Muted = false;
    public float GlobalVolume = 1;
    public List<ClipLookup> Clips;
    private List<AudioSource> m_sources;

    public static SFXManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    void Start()
    {
        m_sources = GetComponentsInChildren<AudioSource>().ToList();
    }

    public void PlayPlayerSound(PlayerAction action)
    {
        if(!Muted)
        {
            AudioSource source = m_sources.FirstOrDefault(s => s.isPlaying == false);

            if (source != null)
            {
                source.volume = GlobalVolume;
                source.clip = Clips.First(c => c.Action == action).Clip;
                source.Play();
            }
        }
    }
}

 [Serializable]
 public struct ClipLookup 
 {
     public PlayerAction Action;
     public AudioClip Clip;
 }