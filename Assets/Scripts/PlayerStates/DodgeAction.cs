using UnityEngine;
using System.Collections;

public class DodgeAction : PlayerState
{
    float startTime = 0.0f;
    float elapsedtime = 0.0f;
    Vector3 direction;

    public DodgeAction(GameObject player, Vector3 finish) : base(player)
    {
        anim.SetTrigger("roll");

        MovementSpeed = 8f;
        startTime = Time.deltaTime;
        IK.GlobalWeight = 0f;

        direction = finish.normalized;
    }

    public override PlayerState UpdateState()
    {
        UpdateMovement();
        return HandleInput();
    }

    public override PlayerState HandleInput()
    {
        elapsedtime += Time.deltaTime;
        if (elapsedtime - startTime >= 1.9f) {
            return new GroundedState(Player);
        }
        else return null;
    }

    public override void UpdateMovement()
    {
        Vector3 temp = direction * -(elapsedtime * (elapsedtime - 1.9f) * MovementSpeed);
        float Y = rb.velocity.y;
        temp.y = Y;
        rb.velocity = temp;
        rb.AddForce(Physics.gravity * rb.mass, ForceMode.Force);
    }
}