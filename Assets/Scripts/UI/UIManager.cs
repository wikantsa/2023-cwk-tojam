using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;

    void Awake()
    {
        // the scots object!!!
        if (UIManager.instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }
        UIManager.DontDestroyOnLoad(this);
    }
}
