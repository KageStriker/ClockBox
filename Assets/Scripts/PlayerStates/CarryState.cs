using UnityEngine;
using System.Collections;

public class CarryState : GroundedState
{
    Transform carryObject;
    CarryNode carryNode;

    Vector3 OffsetPosition;

    public CarryState(GameObject player, CarryNode node) : base(player)
    {
        IK.RightHandWeight = 0.8f;
        IK.LeftHandWeight = 0.8f;
        
        carryNode = node;
        carryNode.rigidBody.isKinematic = true;
        carryNode.rootCollider.enabled = false;
        carryNode.Active = false;

        carryObject = node.gameObject.transform.parent;
        carryObject.parent = Player.transform;

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
        if (!Physics.Raycast(Player.transform.position + (Vector3.up * 0.5f) - (Player.transform.forward * 0.2f), Vector3.down, 0.6f) &&
            !Physics.Raycast(Player.transform.position + (Vector3.up * 0.5f) + (Player.transform.forward * 0.2f), Vector3.down, 0.6f))
        {
            dropObject();
            return new FallState(Player);
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q) || Input.GetButton("Fire1"))
        {
            dropObject();
            return new GroundedState(Player);
        }

        if (Input.GetButtonDown("Jump"))
        {
            dropObject();
            Jump();
            return new FallState(Player);
        }

        return base.HandleInput();
    }

    public override void UpdateIK()
    {
        OffsetPosition = Player.transform.position + (Player.transform.up * 1.1f) + (Player.transform.forward * 0.3f);

        IK.RightHandPosition = carryNode.rightHand.position;
        IK.RightHandRotation = carryNode.rightHand.rotation;

        IK.LeftHandPosition = carryNode.leftHand.position;
        IK.LeftHandRotation = carryNode.leftHand.rotation;

        carryObject.rotation = Player.transform.rotation;
        carryObject.position =  OffsetPosition;
        
    }

    void dropObject()
    {
        IK.RightHandWeight = 0;
        IK.LeftHandWeight = 0;
        carryObject.parent = null;
        carryNode.rigidBody.isKinematic = false;
        carryNode.rootCollider.enabled = true;
        carryNode.delayPickup(0.5f);
    }

    public override PlayerState OnTriggerEnter(Collider other) { return null; }
    public override PlayerState OnTriggerStay(Collider other) { return null; }
}
