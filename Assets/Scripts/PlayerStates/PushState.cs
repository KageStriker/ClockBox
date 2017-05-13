using UnityEngine;
using System.Collections;

public class PushState : PlayerState
{
    Rigidbody PushObject;
    public PushState(GameObject player, GameObject pushObject) : base(player)
    {
        MovementSpeed = 2f;
        PushObject = pushObject.GetComponent<Rigidbody>();
        PushObject.isKinematic = false;

        anim.SetBool("isGrounded", true);
        anim.SetBool("hasWeapon", false);
        anim.SetBool("aiming", false);
    }
	
    public override PlayerState UpdateState()
    {
        UpdateMovement();
        UpdateIK();
        return HandleInput();
    }

    public override PlayerState HandleInput()
    {
        if (Z <= Mathf.Epsilon)
        {
            PushObject.velocity = Vector3.zero;
            PushObject.isKinematic = true;
            IK.GlobalWeight = 0f;
            return new GroundedState(Player);
        }

        if (IK.RightHandWeight == 0.0f && IK.LeftHandWeight == 0)
            return new GroundedState(Player);

        return base.HandleInput();
    }

    public override void UpdateMovement()
    {
        X = Input.GetAxis("Horizontal") * MovementSpeed;
        Z = Input.GetAxis("Vertical") * MovementSpeed;
        
        Vector3 lookDirection = Camera.main.transform.forward.normalized;
        lookDirection.y = 0.0f;

        Vector3 desiredDirection = new Vector3(X, 0, Z);
        desiredDirection = Quaternion.FromToRotation(Vector3.forward, lookDirection) * desiredDirection;
        moveDirection = Vector3.MoveTowards(moveDirection, desiredDirection, 15 * Time.deltaTime);

        if (moveDirection.magnitude > 0)
            moveDirection = Vector3.RotateTowards(moveDirection, desiredDirection + lookDirection * 0.01f, 1.0f * Time.deltaTime, 0.5f * Time.deltaTime);

        Player.transform.LookAt(PushObject.transform.position - new Vector3(0, PushObject.transform.position.y - Player.transform.position.y, 0));
        
        anim.SetFloat("Z", rb.velocity.magnitude);

        rb.velocity = new Vector3(moveDirection.x, 0f, moveDirection.z);
    }

    public override void UpdateIK()
    {
        if (PushObject)
        {
            //RayCast- Right Hand
            Vector3 rightRayStart = Player.transform.up + Player.transform.right * 0.5f;
            Vector3 rightRayDirection = Player.transform.forward - Player.transform.right * 0.5f;
            RaycastHit rightHit;

            if (Physics.Raycast(Player.transform.position + rightRayStart, rightRayDirection, out rightHit, 1f))
            {
                IK.RightHandPosition = rightHit.point - Player.transform.forward * 0.01f;
                IK.RightHandRotation = Quaternion.FromToRotation(Player.transform.up, rightHit.normal) * Player.transform.rotation;
                IK.RightHandWeight = 1;
            }
            else IK.RightHandWeight = 0.0f;

            //RayCast- left Hand
            Vector3 leftRayStart = Player.transform.up - Player.transform.right * 0.5f;
            Vector3 leftRayDirection = Player.transform.forward + Player.transform.right * 0.5f;
            RaycastHit leftHit;

            if (Physics.Raycast(Player.transform.position + leftRayStart, leftRayDirection, out leftHit, 1f))
            {
                IK.LeftHandPosition = leftHit.point - Player.transform.forward * 0.01f;
                IK.LeftHandRotation = Quaternion.FromToRotation(Player.transform.up, leftHit.normal) * Player.transform.rotation;

                IK.LeftHandWeight = 1;
            }
            else IK.LeftHandWeight = 0;
        }
        else
        {
            IK.RightHandWeight = 0.0f;
            IK.LeftHandWeight = 0;
        }
    }

    public override PlayerState OnTriggerEnter(Collider other) { return null; }
    public override PlayerState OnTriggerStay(Collider other) { return null; }
}