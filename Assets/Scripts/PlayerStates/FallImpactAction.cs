using UnityEngine;
using System.Collections;

public class FallImpactAction : PlayerState
{
    float startTime = 0.0f;
    float elapsedtime = 0.0f;

    public FallImpactAction(GameObject player) : base(player)
    {
        anim.SetBool("isGrounded", true);
        anim.ResetTrigger("falling");
        startTime = Time.deltaTime;
        IK.GlobalWeight = 0f;

        rb.velocity = Vector3.zero;
    }

    public override PlayerState UpdateState()
    {
        UpdateMovement();
        return HandleInput();
    }

    public override PlayerState HandleInput()
    {
        elapsedtime += Time.deltaTime;
        if (elapsedtime - startTime >= 5.0f)
        {
            return new GroundedState(Player);
        }
        else return null;
    }

    public override void UpdateMovement() { }
}
