using UnityEngine;

public class Bullet : BaseBullet
{
    Transform m_spawner;
    Rigidbody m_rigidBody;
    public Animator m_animator;

    public float travelSpeed = 1;
    public float TTK = 3;

    private float timer;
    private Collider trigger;

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        trigger = GetComponent<Collider>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && trigger.enabled)
        {
            Deactivate();
        }

        /*
        if (LevelManager.Instance.Paused && m_rigidBody.velocity.z != 0)
        {
            m_rigidBody.velocity = Vector3.zero;
        }
        */
    }

    public override void Activate(Transform spawner)
    {
        gameObject.SetActive(true);
        trigger.enabled = true;
        m_spawner = spawner;
        transform.parent = null;
        timer = TTK;
        transform.rotation = spawner.rotation;
        m_rigidBody.AddForce(transform.forward * travelSpeed, ForceMode.Impulse);
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
        m_rigidBody.velocity = Vector3.zero;
        transform.parent = m_spawner;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Bullet")
        {
            Deactivate();

            //transform.localPosition -= transform.forward * 0.05f * LevelManager.Instance.LevelSpeed;
            //m_rigidBody.velocity = Vector3.zero;
            //trigger.enabled = false;
            //m_animator.SetTrigger("Explode");
        }
    }
}