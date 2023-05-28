using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float m_range;
    private int m_damageDone = 3;
    private float m_laserThickness = 1f;
    public Animator m_animator;

    private float m_baseRange;

    public SpriteRenderer m_laserSprite;

    public float Level3LaserThickness = 1.5f;
    public float Level2LaserThickness = 1f;
    public float Level1LaserThickness = 0.5f;

    public int Level3Damage = 15;
    public int Level2Damage = 10;
    public int Level1Damage = 5;

    private void Start()
    {
        m_baseRange = m_laserSprite.size.y;
        m_range = m_baseRange * 3;
        transform.localScale = Vector3.one * 3;
        m_laserThickness = Level3LaserThickness;
        m_damageDone = Level3Damage;
    }

    public void Activate()
    {
        SetLaserSpriteDistance(m_range);
        m_animator.SetTrigger("Shoot");
    }

    public void DoHitScan()
    {
        SFXManager.Instance.PlayPlayerSound(PlayerAction.Lazer);
        // Bit shift the index of the layer (6) to get a bit mask for terrain objects
        var terrainMask = 1 << 6;

        float finalDistance = m_range;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.SphereCast(transform.position, m_laserThickness, transform.TransformDirection(Vector3.forward), out hit, m_range, terrainMask))
        {
            SetLaserSpriteDistance(hit.distance);
            finalDistance = hit.distance;
        }

        var killableMask = 1 << 7;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, m_laserThickness, transform.TransformDirection(Vector3.forward), finalDistance, killableMask);

        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].transform.GetComponent<Killable>().DealDamage(m_damageDone);
        }
    }

    void SetLaserSpriteDistance(float distance)
    {
        var size = m_laserSprite.size;
        size.y = distance;
        m_laserSprite.size = size;

        var pos = m_laserSprite.transform.localPosition;
        pos.z = distance * 0.5f;
        m_laserSprite.transform.localPosition = pos;
    }

    public void SetLaserLevel(int level)
    {
        if (level == 2)
        {
            m_laserThickness = Level2LaserThickness;
            m_damageDone = Level2Damage;
        }
        else if (level == 1)
        {
            m_laserThickness = Level1LaserThickness;
            m_damageDone = Level1Damage;
        }

        transform.localScale = Vector3.one * level;
        m_range = m_baseRange * level;
    }
}
