using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IsoCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float CatchUpSpeed = 1f;

    public float ShakeDuration = 1f;
    public float ShakeStrength = 1f;
    public int ShakeVibrato = 1;
    public float ShakeRandomness = 1f;

    // Start is called before the first frame update
    void Start()
    {
        transform.forward = -offset.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destination = target.position + offset;
        Vector3 location = transform.position;
        transform.position = transform.position + (destination - location) * Time.deltaTime * CatchUpSpeed;

        if(Input.GetKeyDown(KeyCode.Q))
        {
            transform.DOShakePosition(ShakeDuration, ShakeStrength, ShakeVibrato, ShakeRandomness, true, true, ShakeRandomnessMode.Harmonic);

        }
    }
}
