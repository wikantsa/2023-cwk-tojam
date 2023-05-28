using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    public Animator m_Animator;
    public float RotateSpeed = 10f;
    public List<BaseShooter> m_ShooterList;

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

        if (Input.GetButtonDown("Fire1"))
        {
            m_Animator.SetBool("FireMode", true);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            m_Animator.SetBool("FireMode", false);
        }

        if (Input.GetButton("Fire1"))
        {
            foreach (var shooter in m_ShooterList)
            {
                if (!shooter.IsOnCooldown)
                {
                    shooter.ShootBullet();
                }
            }
        }
    }
}
