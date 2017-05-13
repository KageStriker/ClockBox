using UnityEngine;
using System.Collections;

public class AimState : GroundedState
{
    static float focus = 5f;

    public AimState(GameObject player) : base(player)
    {
        IK.LeftHandWeight = 0.8f;

        anim.SetBool("isGrounded", true);
        anim.SetBool("hasWeapon", false);
        anim.SetBool("aiming", true);
    }

    public override PlayerState UpdateState()
    {
        UpdateMovement();
        UpdateIK();
        return HandleInput();
    }

    public override PlayerState HandleInput()
    {
        if (Input.GetButton("Fire1"))
            anim.SetTrigger("shoot");

        if (!Input.GetButton("Fire2"))
        {
            IK.LeftHandWeight = 0;
            return new GroundedState(Player);
        }
        return base.HandleInput();
    }

    public override void UpdateIK()
    {
        IK.LeftHandPosition = Player.transform.position + (Player.transform.up * 1.5f) - (Player.transform.right * 0.2f) + (IK.lookAtPosition.normalized * 0.35f);
        IK.LeftHandRotation = Quaternion.LookRotation(IK.lookAtPosition,-Player.transform.right);
    }

    public override PlayerState OnTriggerEnter(Collider other) { return null; }
    public override PlayerState OnTriggerStay(Collider other) { return null; }
}
