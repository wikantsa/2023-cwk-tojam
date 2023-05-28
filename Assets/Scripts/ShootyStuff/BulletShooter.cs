using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : BaseShooter
{
    [Header("Bullet")]
    [SerializeField]
    GameObject m_BulletPrefab;
    [SerializeField]
    Transform m_BulletSpawner;
    [SerializeField]
    int m_PoolSize = 3;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_PoolSize; i++)
        {
            GameObject.Instantiate(m_BulletPrefab, m_BulletSpawner);
        }
    }

    protected override void OnShootBullet()
    {
        SFXManager.Instance.PlayPlayerSound(PlayerAction.Shoot);
        if (m_BulletSpawner.childCount == 0)
        {
            GameObject.Instantiate(m_BulletPrefab, m_BulletSpawner);
        }

        m_BulletSpawner.GetChild(0).GetComponent<BaseBullet>().Activate(m_BulletSpawner);
        //m_Animator.SetTrigger("Fireball");
        //FireArrowSound.Play();
    }
}
