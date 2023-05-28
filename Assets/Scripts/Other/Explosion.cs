using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Animator m_animator;
    public float TTK = 2;

    private float timer;

    public void Activate(Vector3 position, float scale)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.localScale = Vector3.one * scale;
        transform.parent = null;
        timer = TTK;
        m_animator.SetTrigger("Explode");
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        transform.parent =ExplosionManager.Instance.transform;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Deactivate();
        }
    }
}
