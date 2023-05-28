using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    public Animator m_Animator;
    public bool GridLockedMovement;
    public float RotateSpeed = 10f;
    public List<BaseShooter> m_ShooterList;

    Plane m_Plane;
    Vector3 m_AimDirection = Vector3.zero;
    bool controllerMode;
    Vector3 prevMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        m_Plane = new Plane(Vector3.up, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerMode)
        {
            if ((prevMousePosition - Input.mousePosition).sqrMagnitude > 2)
            {
                controllerMode = false;
            }

            prevMousePosition = Input.mousePosition;
        }


        var inputs = Vector3.zero;
        inputs.x = Input.GetAxis("Horizontal2");
        inputs.z = Input.GetAxis("Vertical2");

        if (inputs.magnitude > 0.1)
        {
            if (!GridLockedMovement)
            {
                inputs = Quaternion.AngleAxis(45f, Vector3.up) * inputs;
            }

            m_AimDirection = inputs;
            
            if (!controllerMode)
            {
                controllerMode = true;
                prevMousePosition = Input.mousePosition;
            }
        }
        else if (!controllerMode)
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
                characterPosition.y = 0f;
                hitPoint.y = 0f;
                m_AimDirection = hitPoint - characterPosition;
            }
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
