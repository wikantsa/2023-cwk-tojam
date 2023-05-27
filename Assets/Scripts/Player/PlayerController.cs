using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpForce = 1f;
    public float DashForce = 1f;
    public float DashDuration = 1f;
    public float DashCooldown = 4f;
    public float MaxVelocity = 1f;
    public float SlowDownModifier = 1f;
    public float GravityModifier = 1f;

    private Vector3 m_Inputs = Vector3.zero;
    
    private float m_DashTimer = 0f;
    private Vector3 m_DashDirection;
    private float m_DashCooldown = 0f;
    private Vector3 m_LastPosition;

    private Transform m_body;
    private Rigidbody m_Rigidbody;

    void Start()
    {
        m_body = transform;
        m_Rigidbody = transform.GetComponent<Rigidbody>();
        m_LastPosition = m_body.position;
    }

    void Update()
    {
        
        m_Inputs = Vector3.zero;
        m_Inputs.x = Input.GetAxis("Horizontal");
        m_Inputs.z = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.E) && m_DashCooldown <= 0 && m_DashTimer <= 0)
        {
            m_DashTimer = DashDuration;
            m_DashCooldown = DashCooldown;
            m_DashDirection = m_Inputs.normalized;
        }
            
        if (Input.GetKeyDown(KeyCode.Space) && m_body.position.y <= 1.05f)
        {
            m_Rigidbody.AddForce(Vector3.up * JumpForce);
        }

        if(m_DashCooldown > 0)
        {
            m_DashCooldown -= Time.deltaTime;
        }

        m_LastPosition = m_body.position;
    }

    void FixedUpdate()
    {
        // Add movement Velocity
        m_Rigidbody.AddForce(m_Inputs * Speed * Time.fixedDeltaTime);

        // Slow down in directions you are not gaining velocity in
        Vector3 velocity = m_Rigidbody.velocity;
        float yVelocity = m_Rigidbody.velocity.y;
        velocity.y = 0;
        m_Rigidbody.AddForce(-velocity * SlowDownModifier * Time.fixedDeltaTime);
        velocity.x = Mathf.Clamp(velocity.x, -MaxVelocity, MaxVelocity);
        velocity.z = Mathf.Clamp(velocity.z, -MaxVelocity, MaxVelocity);
        velocity.y = yVelocity - GravityModifier * Time.fixedDeltaTime;
        m_Rigidbody.velocity = velocity;

        // Add dash force
        if(m_DashTimer > 0)
        {
            m_Rigidbody.AddForce(m_DashDirection * DashForce);
            m_DashTimer -= Time.fixedDeltaTime;
        }
    }
}
