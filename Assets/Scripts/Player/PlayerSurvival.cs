using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSurvival : MonoBehaviour
{
    public int MaxHealth = 100;
    public float InvulnerabilityWindow = 1f;
    private int m_health;
    private float m_invulnerabilityCountdown;

    // Start is called before the first frame update
    void Start()
    {
        m_health = MaxHealth;
    }

    void Update()
    {
        if(m_invulnerabilityCountdown >= 0)
        {
            m_invulnerabilityCountdown -= Time.deltaTime;
        }
        if(m_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(m_invulnerabilityCountdown <= 0)
        {
            int damage = collision.gameObject.GetComponent<DamageSource>().Damage;
            m_health -= damage;
            m_invulnerabilityCountdown = InvulnerabilityWindow;
        }
    }
}
