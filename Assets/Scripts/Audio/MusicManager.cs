using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MusicManager : MonoBehaviour
{
    public List<TrackLookup> Clips;
    private AudioSource m_source;

    public static MusicManager Instance { get; private set; }
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
        m_source = GetComponent<AudioSource>();
    }

    public void PlayTrack(MusicTrack track)
    {
        m_source.clip = Clips.First(c => c.Track == track).Clip;
        m_source.Play();
    }
}

 [Serializable]
 public struct TrackLookup 
 {
     public MusicTrack Track;
     public AudioClip Clip;
 }