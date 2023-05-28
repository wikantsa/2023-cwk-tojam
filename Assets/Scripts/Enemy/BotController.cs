using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BotController : MonoBehaviour, ITargetSettable
{

    private NavMeshAgent agent;
    private Rigidbody rb;
    private Vector3 kbVec;


    public GameObject target;
    public float minDistance;
    public float speed;
    public float turnSpeed;
    public float knockback;
    public float brakeSpeed;

    public void SetTarget(GameObject obj) {
        target=obj;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null){
            agent.SetDestination(target.transform.position);
        }
    }

    void FixedUpdate() {
        Vector3 directionVector = (agent.steeringTarget-rb.position).normalized;
        //rotate
        Quaternion newRotation = Quaternion.LookRotation(directionVector, Vector3.up);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, newRotation, turnSpeed));


        //move
        if (kbVec == Vector3.zero) {
            directionVector = (agent.steeringTarget-rb.position).normalized;
        } else {
            directionVector = (kbVec*knockback);
            kbVec = Vector3.zero;
        }

        if (agent.remainingDistance >= minDistance) {
            rb.MovePosition(rb.position + (directionVector * speed * Time.fixedDeltaTime));
        }

      rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, brakeSpeed);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet") {
            kbVec = (rb.position- agent.steeringTarget).normalized;
        }
    }
}

interface ITargetSettable
{
  void SetTarget(GameObject obj);
}