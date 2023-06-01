using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FlyController : MonoBehaviour, ITargetSettable
{

    private Rigidbody rb;
    private Vector3 kbVec;
    private float attackDelay;
    public FlyState state;
    private Vector3 startPosition;
    private bool JustHit;

    public GameObject target;
    public float diveDistance;
    public float attackDelayMax;



    ///movement stuff
    public float speed;
    public float diveSpeed;
    public float dropSpeed;
    public float turnSpeed;
    public float brakeSpeed;


    public enum FlyState {
        Chase,
        Ready,
        Dive,
        Retreat,

    }


    public void SetTarget(GameObject obj) {
        target=obj;
        state = FlyState.Chase;
        startPosition =  transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        JustHit = false;
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate() {

        if (!target.activeSelf)
        {
            return;
        }

        Vector3 distanceVec = (target.transform.position-rb.position);
        distanceVec.y=0;
        float distance = distanceVec.magnitude;
        if (state == FlyState.Chase) {
            if (distance >= diveDistance) {
                Vector3 directionVector = (target.transform.position-rb.position).normalized;
                directionVector.y = 0;

                this.MoveTowards(directionVector);
            } else {
                state = FlyState.Ready;
                attackDelay = attackDelayMax;
                startPosition = rb.position;
                Debug.Log("Ready State");
            }
        } else if (state == FlyState.Ready) {
            // look at player for a few frames
            Vector3 directionVector = (target.transform.position-rb.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(directionVector, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, newRotation, turnSpeed));
            attackDelay -= Time.fixedDeltaTime;
            if (attackDelay <=0) {
                state = FlyState.Dive;
                Debug.Log("Diving State");
            }
        } else if (state == FlyState.Dive) {
            this.DiveMove();
        } else if (state == FlyState.Retreat) {
            Vector3 directionVector = (startPosition-rb.position).normalized;
            this.MoveTowards(directionVector);

            Vector3 returnVec = startPosition - rb.position;
            if (returnVec.magnitude <=0.1) {
                Debug.Log("Chase state");
                state = FlyState.Chase;
            }
        }

        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, brakeSpeed);
    }


    private void DiveMove() {
        Vector3 directionVector = (target.transform.position-rb.position).normalized;
        //rotate
        Quaternion newRotation = Quaternion.LookRotation(directionVector, Vector3.up);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, newRotation, turnSpeed));

        directionVector.y = 0;
        directionVector.Normalize();
        //move
        if (JustHit) {
            directionVector = - directionVector;
            JustHit = false;
        }
        rb.MovePosition(rb.position + (directionVector * diveSpeed * Time.fixedDeltaTime));
        //move down unless floor
        RaycastHit[] hits = Physics.RaycastAll(rb.position, Vector3.down);
        if (hits.Length > 0) {
            foreach (var hit in hits) {
                Debug.Log(hit.transform.name + " " + hit.transform.tag + " " + hit.distance);
                if (hit.transform.tag == "Untagged" && hit.distance > 1.0f) { //only stop for floors/obstacles
                    Debug.Log("Hit:" + hit.transform.name);
                    rb.MovePosition(rb.position + (Vector3.down * dropSpeed * Time.fixedDeltaTime));
                    return;
                }
            }
        } else {
            rb.MovePosition(rb.position + (Vector3.down * dropSpeed * Time.fixedDeltaTime));
        }
    }

    private void MoveTowards(Vector3 directionVector) {
        //rotate
        Quaternion newRotation = Quaternion.LookRotation(directionVector, Vector3.up);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, newRotation, turnSpeed));

        //move
        if (JustHit) {
            directionVector = - directionVector;
            JustHit = false;
        }
        rb.MovePosition(rb.position + (directionVector * speed * Time.fixedDeltaTime));
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet") {
            JustHit = true;
        }
        if (other.tag == "Player" && state == FlyState.Dive) {
            // CHANGE TO Reset and fly away
            Debug.Log("Retreat State");
            state = FlyState.Retreat;
        }

    }
}