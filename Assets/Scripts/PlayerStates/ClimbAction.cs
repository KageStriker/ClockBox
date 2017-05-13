using UnityEngine;
using System.Collections;

public class ClimbAction : PlayerState
{
    float startTime = 0.0f;
    float elapsedtime = 0.0f;
    float waitTime;

    Vector3 direction;
    Collider col;

    public ClimbAction(GameObject player) : base(player)
    {
        col = Player.GetComponent<Collider>();
        col.enabled = false;

        anim.SetTrigger("climbUp");
        anim.SetBool("climbing", false);

        MovementSpeed = 2f;
        startTime = Time.deltaTime;
        IK.GlobalWeight = 0f;

        if (anim.GetBool("braced"))
            waitTime = 1.0f;
        else waitTime = 3.8f;

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
        if (elapsedtime - startTime >= waitTime)
        {
            col.enabled = true;
            return new GroundedState(Player);
        }
        else return null;
    }

    public override void UpdateMovement()
    {
        float Y = 0f;
        if (anim.GetBool("braced"))
            Y = -elapsedtime * elapsedtime * 20 * (elapsedtime - 1);
        else Y = (-3/4 * elapsedtime * elapsedtime) + (2 * elapsedtime);

        rb.velocity = (Player.transform.forward * elapsedtime/waitTime) + (Vector3.up *  Y);
    }
}
