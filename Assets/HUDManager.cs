using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public GameObject _indicator;
    public PlayerController _player;

    public Material _green;
    public Material _red;

    public void ToggleDashIndicator()
    {
        StartCoroutine(DashOffline());
    }

    private IEnumerator DashOffline()
    {
        _indicator.GetComponent<Renderer>().material = _red;
        yield return new WaitForSeconds(1f);
        _indicator.GetComponent<Renderer>().material = _green;
    }
}
