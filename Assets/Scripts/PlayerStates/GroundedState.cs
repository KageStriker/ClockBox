using UnityEngine;
using System.Collections;

public class GroundedState : PlayerState
{
    public GroundedState(GameObject player) : base(player)
    {
        MovementSpeed = 5f;

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
            return new FallState(Player);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            return new FallState(Player);
        }

        if (Input.GetKeyDown(KeyCode.F))
            return new DodgeAction(Player, moveDirection);

        if (Input.GetButtonDown("Fire1"))
            return new CombatState(Player);

        if (Input.GetKeyDown(KeyCode.Q))
            return new CombatState(Player);

        if (Input.GetButton("Fire2"))
            return new AimState(Player);

        if (Input.GetKey(KeyCode.LeftShift))
            MovementSpeed = 8;
        else MovementSpeed = 5;
        
        return null;
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

        Player.transform.LookAt(moveDirection + Player.transform.position);

        anim.SetFloat("Z", moveDirection.magnitude);

        anim.SetFloat("turnAngle", calculateSignedAngle(Player.transform.forward, lookDirection));
        if (X == 0 && Z == 0 && Vector3.Angle(Player.transform.forward, lookDirection) > 80.0f)
        {
            anim.SetTrigger("turn");
        }

        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        rb.AddForce(Physics.gravity * rb.mass);
    }
    public override void UpdateIK() { }

    public override void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce * rb.mass, ForceMode.Impulse);
    }

    float calculateSignedAngle(Vector3 from, Vector3 to)
    {
        float angle = Vector3.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);

        if (cross.y < 0)
            angle = -angle;

        return angle;
    }

    public override PlayerState OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pushable"))
            return new PushState(Player, other.gameObject);
        else if (other.gameObject.CompareTag("ClimbingNode"))
            return new ClimbState(Player, other.gameObject.GetComponent<ClimbingNode>());
        return null;
    }

    public override PlayerState OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CarryNode") && Input.GetKey(KeyCode.E))
        {
            CarryNode temp = other.gameObject.GetComponent<CarryNode>();
            if (temp && temp.Active)
                return new CarryState(Player, temp);
        }
        return null;
    }
}