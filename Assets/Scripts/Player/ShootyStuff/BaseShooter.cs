using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseShooter : MonoBehaviour
{
    [SerializeField]
    float m_Cooldown = 0.5f;
    float m_CooldownTracker = 0;

    public bool IsOnCooldown => m_CooldownTracker > 0;

    // Update is called once per frame
    void Update()
    {
        if (m_CooldownTracker > 0)
            m_CooldownTracker -= Time.deltaTime;
    }

    public void ShootBullet()
    {
        OnShootBullet();

        m_CooldownTracker = m_Cooldown;
    }

    protected abstract void OnShootBullet();
}
