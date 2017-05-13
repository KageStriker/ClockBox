using UnityEngine;
using System.Collections;

public class ClimbingNode : MonoBehaviour {

    public Transform rightHand;
    public Transform leftHand;
    public Transform rightFoot;
    public Transform leftFoot;

    public ClimbingNode[] neighbours = new ClimbingNode[8];

    public bool Edge;
    public bool FreeHang;

    void Start () {
        if (!rightHand || !leftHand || !rightFoot || !leftFoot)
            Debug.LogError("Climbing Node: "+ gameObject.name +" is not set up properly");

        if (neighbours.Length == 0)
            Debug.LogWarning("Climbing Node: " + gameObject.name + " has no Neighbors");
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(transform.position, rightHand.transform.position);
        Gizmos.DrawLine(transform.position, leftHand.transform.position);
        Gizmos.DrawLine(transform.position, rightFoot.transform.position);
        Gizmos.DrawLine(transform.position, leftFoot.transform.position);
    }
}
