using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootController : MonoBehaviour
{
    public float RotateSpeed = 10f;
    public List<BaseShooter> m_ShooterList;
    public PlayerEvent shootEvent;

    Plane m_Plane;
    Vector3 m_AimDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        m_Plane = new Plane(Vector3.up, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Create a ray from the Mouse click position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Initialise the enter variable
        float enter = 0.0f;
        if (m_Plane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);

            var characterPosition = transform.position;
            characterPosition.y = 0;
            m_AimDirection = hitPoint - characterPosition;
        }

        float singleStep = RotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, m_AimDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        if (Input.GetButton("Fire1"))
        {
            foreach (var shooter in m_ShooterList)
            {
                if (!shooter.IsOnCooldown)
                {
                    shootEvent.Invoke("shoot");
                    shooter.ShootBullet();
                }
            }
        }
    }
}
