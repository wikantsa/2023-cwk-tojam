using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShooter : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField]
    GameObject m_BulletPrefab;
    [SerializeField]
    Transform m_BulletSpawner;
    [SerializeField]
    int m_PoolSize = 3;
    [SerializeField]
    float m_BulletCooldown = 0.5f;
    float m_BulletCooldownTracker = 0;

    public bool IsOnCooldown => m_BulletCooldownTracker > 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_PoolSize; i++)
        {
            GameObject.Instantiate(m_BulletPrefab, m_BulletSpawner);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_BulletCooldownTracker > 0)
            m_BulletCooldownTracker -= Time.deltaTime;
    }

    public void ShootBullet()
    {
        if (m_BulletSpawner.childCount == 0)
        {
            GameObject.Instantiate(m_BulletPrefab, m_BulletSpawner);
        }

        m_BulletSpawner.GetChild(0).GetComponent<BaseBullet>().Activate(m_BulletSpawner);
        m_BulletCooldownTracker = m_BulletCooldown;
        //m_Animator.SetTrigger("Fireball");
        //FireArrowSound.Play();
    }
}
