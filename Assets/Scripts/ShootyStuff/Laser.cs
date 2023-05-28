using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float m_range = 40f;
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
        int layerMask = 1 << 6;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, m_range, layerMask))
        {
            SetLaserSpriteDistance(hit.distance);
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
