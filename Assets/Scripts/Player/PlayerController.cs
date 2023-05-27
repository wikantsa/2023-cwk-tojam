using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpHeight = 6f;
    public float JumpDuration = 3f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public float DashDuration = 1f;
    public float DashCooldown = 4f;
    public float MaxVelocity = 1f;
    public float SlowDownModifier = 1f;

    private Vector3 m_Inputs = Vector3.zero;
    
    private bool m_Jumping = false;
    private bool m_Dashing = false;
    private float m_DashCooldown = 0f;

    private Sequence m_dashSequence;
    private Sequence m_jumpSequence;

    private Transform m_body;
    private Rigidbody m_Rigidbody;

    void Start()
    {
        m_body = transform;
        m_Rigidbody = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        m_Inputs = Vector3.zero;
        m_Inputs.x = Input.GetAxis("Horizontal");
        m_Inputs.z = Input.GetAxis("Vertical");

        if (m_Inputs != Vector3.zero)
        {
            transform.forward = m_Inputs;
        }

        if (Input.GetKeyDown(KeyCode.E) && m_DashCooldown <= 0)
        {
            m_Dashing = true;
            m_DashCooldown = DashCooldown;
            m_jumpSequence.TogglePause();
            m_dashSequence = DOTween.Sequence();
            m_dashSequence.Append(transform.DOBlendableMoveBy(DashDistance * transform.forward, DashDuration).SetEase(Ease.InOutQuad));
            m_dashSequence.OnComplete(() => { m_Dashing = false; m_jumpSequence.TogglePause();});
        }
            
        if (Input.GetKeyDown(KeyCode.Space) && !m_Jumping)
        {
            m_Jumping = true;
            m_jumpSequence = DOTween.Sequence();
            m_jumpSequence.Append(m_body.DOBlendableLocalMoveBy(JumpHeight * Vector3.up, JumpDuration/2).SetEase(Ease.OutQuad));
            m_jumpSequence.Append(m_body.DOBlendableLocalMoveBy(JumpHeight * -Vector3.up, JumpDuration/2).SetEase(Ease.InQuad));
            m_jumpSequence.OnComplete(() => { m_Jumping = false; });
        }
    }

    void FixedUpdate()
    {
        if(m_DashCooldown > 0)
        {
            m_DashCooldown -= Time.fixedDeltaTime;
        }

        //transform.position = transform.position + m_Inputs * Speed * Time.fixedDeltaTime;
        m_Rigidbody.AddForce(m_Inputs * Speed * Time.fixedDeltaTime);

        Vector3 velocity = m_Rigidbody.velocity;
        m_Rigidbody.AddForce(-velocity * SlowDownModifier * Time.fixedDeltaTime);
        velocity.x = Mathf.Clamp(velocity.x, -MaxVelocity, MaxVelocity);
        velocity.y = Mathf.Clamp(velocity.y, -MaxVelocity, MaxVelocity);
        velocity.z = Mathf.Clamp(velocity.z, -MaxVelocity, MaxVelocity);
        m_Rigidbody.velocity = velocity;
    }
}
