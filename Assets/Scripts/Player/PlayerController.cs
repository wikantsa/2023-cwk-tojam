using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;

    private Rigidbody m_RigidBody;
    private Vector3 m_Inputs = Vector3.zero;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        m_Inputs = Vector3.zero;
        m_Inputs.x = Input.GetAxis("Horizontal");
        m_Inputs.z = Input.GetAxis("Vertical");
        if (m_Inputs != Vector3.zero)
            transform.forward = m_Inputs;
    }


    void FixedUpdate()
    {
        m_RigidBody.MovePosition(m_RigidBody.position + m_Inputs * Speed * Time.fixedDeltaTime);
    }
}
