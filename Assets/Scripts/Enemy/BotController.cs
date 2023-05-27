using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BotController : MonoBehaviour
{

    private NavMeshAgent agent;
    private Rigidbody rb;
    public GameObject target;

    public float minDistance;
    public float speed;
    public float brakeSpeed;
    public float acceleration;
    public float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
        //debug input
        //if (Input.GetMouseButtonDown(0)) {
        //    Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    Debug.Log(movePosition);
        //    if (Physics.Raycast(movePosition, out var hitInfo)){
        //        agent.SetDestination(hitInfo.point);
        //    }
        //}
    }

    void FixedUpdate(){
        // move logic
        if (agent.remainingDistance >= minDistance) {
            Vector3 directionVector = (agent.steeringTarget-rb.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(directionVector, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, newRotation, turnSpeed));
            rb.velocity = Vector3.Lerp(rb.velocity, (directionVector * speed), acceleration);
        } else { //stop moving
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, brakeSpeed);
        }
    }
}
