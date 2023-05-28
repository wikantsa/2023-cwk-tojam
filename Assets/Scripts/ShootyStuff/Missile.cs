using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : BaseBullet
{
    Transform m_spawner;
    Rigidbody m_rigidBody;
    public Animator m_animator;
    public TrailRenderer m_trailRenderer;
    public List<MeshRenderer> m_renderers;

    public float launchSpeed = 1;
    public float travelSpeed = 1;
    public float TTK = 3;
    public float despawnTime = 2;
    public float rotateSpeed = 10f;

    private bool isActive;
    private float timer;
    private bool isExploded;
    private Collider trigger;
    private Transform target;

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        trigger = GetComponent<Collider>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && (trigger.enabled || isExploded))
        {
            Deactivate();
        }
    }

    private void FixedUpdate()
    {
        if (!isActive)
            return;

        if (target != null)
        {
            float singleStep = rotateSpeed * Time.fixedDeltaTime;

            var missilePos = transform.position;
            var targetPos = target.transform.position;
            targetPos.y = missilePos.y;
            var aimDirection = targetPos - missilePos;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, aimDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            m_rigidBody.AddForce(transform.forward * travelSpeed);
        }
        else
        {
            FindTarget();
        }
    }

    public override void Activate(Transform spawner)
    {
        gameObject.SetActive(true);
        trigger.enabled = true;
        m_spawner = spawner;
        transform.parent = null;
        timer = TTK;
        transform.rotation = spawner.rotation;
        isActive = true;

        foreach (var renderer in m_renderers)
            renderer.enabled = true;

        FindTarget();
        m_rigidBody.AddForce(transform.forward * launchSpeed);
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
        m_rigidBody.velocity = Vector3.zero;
        transform.parent = m_spawner;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        m_trailRenderer.Clear();
        isExploded = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Bullet")
        {
            //transform.localPosition -= transform.forward * 0.05f * LevelManager.Instance.LevelSpeed;
            isActive = false;
            m_rigidBody.velocity = Vector3.zero;
            trigger.enabled = false;
            m_animator.SetTrigger("Explode");
            timer = despawnTime;
            isExploded = true;

            foreach (var renderer in m_renderers)
                renderer.enabled = false;
        }
    }

    void FindTarget()
    {
        //TODO: Add actual target location code
        target = GameObject.Find("Box (2)").transform;
    }
}
