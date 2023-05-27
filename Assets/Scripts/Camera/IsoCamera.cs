using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        transform.forward = -offset.normalized;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
