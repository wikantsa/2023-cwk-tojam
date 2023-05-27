using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public List<SFXLookup> SFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum SFX
{
    None = 0,
    BasicShot = 1

}

 [Serializable]
 public struct SFXLookup {
     public SFX SFX;
     public AudioClip Clip;
 }