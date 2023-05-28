using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float m_range = 40f;
    public int m_damageDone = 3;
    public float m_laserThickness = 0.1f;
    public Animator m_animator;

    public SpriteRenderer m_laserSprite;

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
}
