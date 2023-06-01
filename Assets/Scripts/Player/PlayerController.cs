using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public bool IsPaused;

    public float Speed = 5f;
    public bool JumpEnabled = false;
    public float JumpForce = 1f;
    public float DashForce = 1f;
    public float DashDuration = 1f;
    public float DashCooldown = 4f;
    public float MaxVelocity = 1f;
    public float GravityModifier = 1f;
    public bool GridLockedMovement = true;

    public Animator m_Animator;
    public TrailRenderer m_TrailRenderer;

    private Vector3 m_Inputs = Vector3.zero;
    
    private float m_DashTimer = 0f;
    private Vector3 m_DashDirection;
    private float m_DashCooldown = 0f;
    private Vector3 m_LastPosition;

    private Transform m_body;
    private Rigidbody m_Rigidbody;

    [Header("Invoke upon dashing")]
    public UnityEvent dashEvent;

    void Start()
    {
        m_body = transform;
        m_Rigidbody = transform.GetComponent<Rigidbody>();
        m_LastPosition = m_body.position;
    }

    void Update()
    {
        if (IsPaused)
            return;

        m_Inputs = Vector3.zero;
        m_Inputs.x = Input.GetAxis("Horizontal");
        m_Inputs.z = Input.GetAxis("Vertical");

        if(!GridLockedMovement)
        {
            m_Inputs = Quaternion.AngleAxis(45f, Vector3.up) * m_Inputs;
        }

        if (Input.GetButtonDown("Fire2") && m_DashCooldown <= 0 && m_DashTimer <= 0)
        {
            SFXManager.Instance.PlayPlayerSound(PlayerAction.Dash);
            m_DashTimer = DashDuration;
            m_DashCooldown = DashCooldown;
            m_DashDirection = m_Inputs.magnitude > 0 ? m_Inputs.normalized : transform.forward;
            m_TrailRenderer.emitting = true;
            dashEvent.Invoke();
        }

        if(m_DashCooldown > 0)
        {
            m_DashCooldown -= Time.deltaTime;
        }

        m_LastPosition = m_body.position;


        var locVel = transform.InverseTransformDirection(m_Rigidbody.velocity).normalized;
        m_Animator.SetFloat("XVelocity", locVel.x);
        m_Animator.SetFloat("ZVelocity", locVel.z);
    }

    void FixedUpdate()
    {
        // Add movement Velocity
        m_Rigidbody.AddForce(m_Inputs.normalized * Speed * Time.fixedDeltaTime);

        Vector3 velocity = m_Rigidbody.velocity;
        float yVelocity = m_Rigidbody.velocity.y;
        velocity.y = 0;

        velocity.x = Mathf.Clamp(velocity.x, -MaxVelocity, MaxVelocity);
        velocity.z = Mathf.Clamp(velocity.z, -MaxVelocity, MaxVelocity);
        velocity.y = yVelocity - GravityModifier * Time.fixedDeltaTime;
        m_Rigidbody.velocity = velocity;

        // Add dash force
        if(m_DashTimer > 0)
        {
            m_Rigidbody.AddForce(m_DashDirection * DashForce);
            m_DashTimer -= Time.fixedDeltaTime;

            if (m_DashTimer <= 0) {
                m_TrailRenderer.emitting = false;
            }
        }
    }
}
