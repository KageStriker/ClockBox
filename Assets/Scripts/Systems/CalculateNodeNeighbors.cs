using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CalculateNodeNeighbors : MonoBehaviour
{

    public bool Calculate = false;
    public float DetectionRadius = 1.5f;

    ClimbingNode currentNode;
    Collider[] NodeTriggers;
    ClimbingNode[] Nodes;

    Vector3[] CompareDirection = new Vector3[8];

    void Start()
    {
        currentNode = GetComponent<ClimbingNode>();

        LayerMask mechanics = 8;
        mechanics = ~mechanics;

        NodeTriggers = Physics.OverlapSphere(transform.position, DetectionRadius, mechanics);
        Nodes = new ClimbingNode[NodeTriggers.Length];
        for (int i = 0; i < NodeTriggers.Length; i++)
        {
            ClimbingNode checkNode = NodeTriggers[i].GetComponent<ClimbingNode>();
            if (checkNode)
                if (checkNode != Nodes[i])
                    Nodes[i] = checkNode;
        }

        CompareDirection[0] = transform.up;
        CompareDirection[1] = (transform.up + transform.right).normalized;
        CompareDirection[2] = transform.right;
        CompareDirection[3] = (-transform.up + transform.right).normalized;
        CompareDirection[4] = -transform.up;
        CompareDirection[5] = (-transform.up - transform.right).normalized;
        CompareDirection[6] = -transform.right;
        CompareDirection[7] = (transform.up - transform.right).normalized;
    }

    private void OnDrawGizmos()
    {
        if (currentNode)
        {
            Gizmos.color = Color.red;
            foreach (ClimbingNode neighbor in currentNode.neighbours)
                if (neighbor)
                    Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }

    void Update()
    {
        if (Calculate)
        {
            Calculate = false;
            ResetNodeS();

            foreach (ClimbingNode checkNode in Nodes)
            {
                if (checkNode && (checkNode != currentNode))
                {
                    for (int i = 0; i < CompareDirection.Length; i++)
                    {
                        Vector3 angle = Quaternion.AngleAxis(checkNode.transform.eulerAngles.y - transform.eulerAngles.y, Vector3.up) * CompareDirection[i];
                        float compareAngle = 22.5f + 45f * Mathf.Clamp(1f - (checkNode.transform.position - transform.position).magnitude, 0f, 1f);
                        if (Vector3.Angle(angle, checkNode.transform.position - transform.position) < compareAngle)
                        {
                            if (currentNode.neighbours[i] != null)
                            {
                                if ((checkNode.transform.position - transform.position).magnitude < (currentNode.neighbours[i].transform.position - transform.position).magnitude)
                                    currentNode.neighbours[i] = checkNode;
                            }
                            else
                                currentNode.neighbours[i] = checkNode;
                        }
                    }
                }
            }
        }
    }

    void ResetNodeS()
    {
        for (int i = 0; i < currentNode.neighbours.Length; i++)
        {
            if (currentNode.neighbours[i])
                currentNode.neighbours[i] = null;
        }
    }
}