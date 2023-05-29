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

    private Dictionary<PlayerAction, AudioSource> m_actionSources = new Dictionary<PlayerAction, AudioSource>();

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
            var clip = Clips.First(c => c.Action == action).Clip;

            AudioSource source = null;
            m_actionSources.TryGetValue(action, out source);

            if (source == null)
            {
                source = m_sources.FirstOrDefault(s => s.isPlaying == false);
                m_actionSources.Add(action, source);
            }

            if (source != null)
            {
                source.volume = GlobalVolume;
                source.clip = clip;
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