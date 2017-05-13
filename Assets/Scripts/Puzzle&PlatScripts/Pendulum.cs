using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    HingeJoint hingeJ;
    JointMotor jM;
    Rigidbody rb;

    float elapsedTime;
    float speed;

    private void Start()
    {
        hingeJ = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
        speed = 45;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        jM.targetVelocity = Mathf.Cos(elapsedTime) * speed;
        jM.force = 100 * rb.mass;
        hingeJ.motor = jM;
    }
}
