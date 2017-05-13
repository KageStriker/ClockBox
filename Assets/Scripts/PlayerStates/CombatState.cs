using UnityEngine;
using System.Collections;

public class CombatState : GroundedState
{
    public CombatState(GameObject player) : base(player)
    {
        MovementSpeed = 5f;

        anim.SetBool("isGrounded", true);
        anim.SetBool("hasWeapon", true);
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
        //if (Input.GetButtonDown("Fire1"))
        //return new AttackState(Player);

        if (Input.GetButton("Fire2"))
            return new AimState(Player);

        if (Input.GetKeyDown(KeyCode.Q))
            return new GroundedState(Player);

        return base.HandleInput();
    }

    public override PlayerState OnTriggerEnter(Collider other) { return null; }
    public override PlayerState OnTriggerStay(Collider other) { return null; }
}
