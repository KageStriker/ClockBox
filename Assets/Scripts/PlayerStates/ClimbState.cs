using UnityEngine;
using System.Collections;

public class ClimbState : PlayerState {

    ClimbingNode currentNode;
    ClimbingNode nextNode;

    ClimbingNode currentRight;
    ClimbingNode currentLeft;

    Vector3 Offset;

    const int NONE = -1;
    const int RIGHT = 1;
    const int LEFT = 2;

    int nextMove = NONE;
    int lastMove = NONE;

    int nodeIndex = NONE;
    int lastIndex = NONE;

    bool moving = false;

    public ClimbState(GameObject player, ClimbingNode node) : base(player)
    {
        MovementSpeed = 2f;

        IK.GlobalWeight = 1;
        IK.SetInitialIKPositions(node.rightHand,node.leftHand,node.rightFoot,node.leftFoot);

        currentNode = node;
        currentRight = node;
        currentLeft = node;

        anim.SetBool("climbing", true);
        UpdateAnimator();

        rb.velocity = Vector3.zero;
    }
	
	public override PlayerState UpdateState()
    {
        UpdateMovement();
        return HandleInput();
    }

    public override PlayerState HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            return new FallState(Player);
        }

        if (currentNode && currentLeft && currentLeft)
        {
            if (currentLeft == currentRight && currentNode.Edge && Z >= Mathf.Epsilon)
                return new ClimbAction(Player);
        }
        return null;
    }

    public override void UpdateMovement()
    {
        X = Input.GetAxis("Horizontal");
        Z = Input.GetAxis("Vertical");

        if (anim.GetBool("braced"))
        {
            IK.RightFootWeight = 1;
            IK.LeftFootWeight = 1;
            Offset = Player.transform.forward * 0.3f - Player.transform.up * 1.3f;
        }
        else
        {
            IK.RightFootWeight = 0;
            IK.LeftFootWeight = 0;
            IK.lookAtPosition += Vector3.up * 10f;
            IK.lookAtPosition = Vector3.ProjectOnPlane(IK.lookAtPosition, Player.transform.right);
            Offset = Player.transform.up;
        }

        moveDirection = new Vector3(X, Z, 0);

        calculateNextMove();

        if (currentNode)
        {
            if (!moving && nextMove != NONE)
            {
                if (moveDirection.magnitude > Mathf.Epsilon)
                {
                    nodeIndex = Mathf.RoundToInt(Mathf.Atan2(moveDirection.x, moveDirection.y) / Mathf.PI * 4);
                    if (nodeIndex < 0)
                        nodeIndex += 8;

                    calculateNextMove();
                    calculateNextNode();
                }
                UpdateAnimator();
            }
            else
            {
                UpdateIK();
            }
            UpdateRoot();
        }
    }

    public override void Jump()
    {
        anim.SetBool("climbing", false);
        anim.SetFloat("X", X);
        anim.SetFloat("Z", Z);
        IK.GlobalWeight = 0;

        if (X > Mathf.Epsilon || X < -Mathf.Epsilon && anim.GetBool("braced"))
            rb.velocity = Player.transform.right * X * jumpForce + (Vector3.up * jumpForce / 2f);
    }

    public override void UpdateIK()
    {
        if (nextNode)
        {
            float duration = (Player.transform.position - nextNode.leftHand.position).magnitude * MovementSpeed * Time.deltaTime;
            IK.MoveRoot(nextNode.transform, duration, Offset);

            if (nextMove == RIGHT)
            {
                float handDuration = (currentRight.rightHand.position - nextNode.rightHand.position).magnitude * MovementSpeed * Time.deltaTime;
                float footDuration = (currentRight.rightFoot.position - nextNode.rightFoot.position).magnitude * MovementSpeed * Time.deltaTime;

                bool done = true;
                if (!(IK.MoveRightHand(nextNode.rightHand.transform, handDuration))) done = false;
                if (!(IK.MoveRightFoot(nextNode.rightFoot.transform, footDuration))) done = false;
                if (done) {
                    currentRight = nextNode;
                    currentNode = nextNode;
                    lastMove = RIGHT;
                    lastIndex = nodeIndex;
                    moving = false;
                }
            }
            else if (nextMove == LEFT)
            {
                float handDuration = (currentLeft.leftHand.position - nextNode.leftHand.position).magnitude * MovementSpeed * Time.deltaTime;
                float footDuration = (currentLeft.leftFoot.position - nextNode.leftFoot.position).magnitude * MovementSpeed * Time.deltaTime;

                bool done = true;
                if (!(IK.MoveLeftHand(nextNode.leftHand.transform, handDuration))) done = false;
                if (!(IK.MoveLeftFoot(nextNode.leftFoot.transform, footDuration))) done = false;
                if (done) {
                    currentLeft = nextNode;
                    currentNode = nextNode;
                    lastMove = LEFT;
                    lastIndex = nodeIndex;
                    moving = false;
                }
            }
        }
    }

    private void calculateNextMove()
    {
        if (nodeIndex == 0 || nodeIndex == 4)
        {
            if ((currentRight.transform.position - currentNode.transform.position).magnitude > (currentLeft.transform.position - currentNode.transform.position).magnitude)
            {
                nextMove = RIGHT;
                currentNode = currentLeft;
            }
            else
            {
                nextMove = LEFT;
                currentNode = currentRight;
            }
            return;
        }
        else
        {
            if (lastMove == NONE || currentRight == currentLeft)
                if (nodeIndex < 4)
                {
                    nextMove = RIGHT;
                }
                else
                {
                    nextMove = LEFT;
                }

            else if (lastIndex == 0 || lastIndex == 4)
            {
                if (nodeIndex < 4)
                {
                    nextMove = RIGHT;
                }
                else
                {
                    nextMove = LEFT;
                }
            }

            else if (lastMove == RIGHT)
                if (nodeIndex < 4)
                {
                    nextMove = LEFT;
                }
                else
                {
                    nextMove = RIGHT;
                }

            else if (lastMove == LEFT)
                if (nodeIndex > 4)
                {
                    nextMove = RIGHT;
                }
                else
                {
                    nextMove = LEFT;
                }
        }
        if (nextMove == RIGHT)
            currentNode = currentRight;
        else if (nextMove == LEFT)
            currentNode = currentLeft;
    }

    private void calculateNextNode()
    {
        if (currentNode.neighbours[nodeIndex])
            nextNode = currentNode.neighbours[nodeIndex];
        else if (currentNode.neighbours[(nodeIndex + 1) % 8])
        {
            nodeIndex = (nodeIndex + 1) % 8;
            if (currentRight == currentLeft)
                nextNode = currentNode.neighbours[nodeIndex];
            else
            {
                if (nextMove == RIGHT)
                    nextNode = currentLeft.neighbours[nodeIndex];
                else if (nextMove == LEFT)
                    nextNode = currentRight.neighbours[nodeIndex];
                nextNode = currentNode;
            }
        }
        else if (currentNode.neighbours[Mathf.Abs((nodeIndex - 1) % 8)])
        {
            nodeIndex = (nodeIndex - 1) % 8;
            if (currentRight == currentLeft)
                nextNode = currentNode.neighbours[nodeIndex];
            else
            {
                if (nextMove == RIGHT)
                    nextNode = currentLeft.neighbours[nodeIndex];
                else if (nextMove == LEFT)
                    nextNode = currentRight.neighbours[nodeIndex];
                nextNode = currentNode;
            }
        }
        else
            nextNode = currentNode;

        moving = true;
    }

    void UpdateRoot()
    {
        Vector3 averagePoint = (IK.RightHandPosition + IK.RightFootPosition + IK.LeftHandPosition + IK.LeftFootPosition) / 4;
        Vector3 averageHandPos = (IK.RightHandPosition + IK.LeftHandPosition) / 2;
        averagePoint.y = Mathf.Clamp(averagePoint.y, averageHandPos.y - 0.5f, averageHandPos.y);

        if (anim.GetBool("braced"))
        {
            Offset = Player.transform.up + Player.transform.forward * 0.3f;
            Player.transform.position = averagePoint - Offset;
        }
        else
        {
            IK.lookAtPosition += Vector3.up * 10f;
            Player.transform.position = averagePoint - Player.transform.up;
        }
    }

    void UpdateAnimator()
    {
        if ((nextNode && nextNode.FreeHang) || currentNode.FreeHang)
            anim.SetBool("braced", false);
        else
            anim.SetBool("braced", true);
    }
}