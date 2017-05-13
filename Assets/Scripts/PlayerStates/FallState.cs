using UnityEngine;
using System.Collections;

public class FallState : PlayerState
{
    float elapsedTime = 0.0f;

    bool freeFall = false;

    public FallState(GameObject player) : base(player)
    {
        anim.SetBool("isGrounded", false);
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
        if (Physics.Raycast(Player.transform.position + (Vector3.up * 0.5f) - (Player.transform.forward * 0.2f), Vector3.down, 0.6f) &&
            Physics.Raycast(Player.transform.position + (Vector3.up * 0.5f) + (Player.transform.forward * 0.2f), Vector3.down, 0.6f))
        {
            if (freeFall)
                return new FallImpactAction(Player);
            return new GroundedState(Player);
        }
        return null;
    }

    public override void UpdateMovement()
    {
        elapsedTime += Time.deltaTime;
        if (Timer(elapsedTime, 3.0f))
        {
            freeFall = true;
            anim.SetTrigger("falling");
        }
        rb.AddForce(Physics.gravity * rb.mass);
    }

    bool Timer(float start, float waitTime)
    {
        if (elapsedTime >= waitTime)
            return true;
        else return false;
    }

    public override void UpdateIK() { }

    public override PlayerState OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClimbingNode"))
            return new ClimbState(Player, other.gameObject.GetComponent<ClimbingNode>());
        return null;
    }
}
