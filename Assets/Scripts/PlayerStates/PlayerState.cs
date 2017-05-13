using UnityEngine;
using System.Collections;

public class PlayerState
{
    protected static GameObject Player;
    protected static Rigidbody rb;
    protected static Animator anim;
    protected static IKController IK;

    protected static Vector3 moveDirection = Vector3.zero;

    protected static float MovementSpeed = 5.0f;
    protected static float jumpForce = 5.0f;

    protected static float X = 0.0f;
    protected static float Z = 0.0f;

    public PlayerState(GameObject player)
    {
        if (!Player) Player = player;
        if (!rb) rb = Player.GetComponent<Rigidbody>();
        if (!anim) anim = Player.GetComponent<Animator>();
        if (!IK) IK = Player.GetComponent<IKController>();
    }

    public virtual PlayerState UpdateState()
    {
        UpdateIK();
        return null;
    }

    public virtual PlayerState HandleInput() { return null; }
    public virtual PlayerState OnTriggerEnter(Collider other) { return null; }
    public virtual PlayerState OnTriggerStay(Collider other) { return null; }
    public virtual PlayerState OnTriggerExit(Collider other) { return null; }

    public virtual void UpdateMovement(){}
    public virtual void UpdateIK() { IK.GlobalWeight = 0; Debug.Log("baseIK"); }
    public virtual void Jump() {}
}
