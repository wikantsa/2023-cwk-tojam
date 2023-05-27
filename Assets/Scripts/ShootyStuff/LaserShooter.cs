using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : BaseShooter
{
    public Laser m_Laser;

    protected override void OnShootBullet()
    {
        m_Laser.Activate();
    }
}
