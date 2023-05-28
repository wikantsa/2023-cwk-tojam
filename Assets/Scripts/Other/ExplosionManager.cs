using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_ExplosionPrefab;
    [SerializeField]
    int m_PoolSize = 20;

    public static ExplosionManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_PoolSize; i++)
        {
            GameObject.Instantiate(m_ExplosionPrefab, transform);
        }
    }

    public void SpawnExplosion(Vector3 position, float scale)
    {
        //SFXManager.Instance.PlayPlayerSound(PlayerAction.Shoot);
        if (transform.childCount == 0)
        {
            GameObject.Instantiate(m_ExplosionPrefab, transform);
        }

        transform.GetChild(0).GetComponent<Explosion>().Activate(position, scale);
    }
}