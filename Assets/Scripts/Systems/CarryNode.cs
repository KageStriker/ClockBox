using UnityEngine;
using System.Collections;

public class CarryNode : MonoBehaviour {

    public Transform rightHand;
    public Transform leftHand;

    Rigidbody rb;
    Collider col;

    bool active = true;
    
    void Start () {
        if (!rightHand || !leftHand)
            Debug.LogError("Carry Node: " + gameObject.name + " cannot find hand positions");

        rb = transform.root.GetComponent<Rigidbody>();
        if (!rb)
            Debug.LogError("Carry Node: " + gameObject.name + " Cannot find ridigbody in root");

        col = transform.root.GetComponent<Collider>();
        if (!col)
            Debug.LogError("Carry Node: " + gameObject.name + " Cannot find ridigbody in root");
    }

    public void delayPickup(float delay)
    {
        StartCoroutine(PickupDelayed(delay));
    }

    IEnumerator PickupDelayed(float delay)
    {
        if (!active)
        {
            yield return new WaitForSeconds(delay);
            active = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(rightHand.position, 0.05f);
        Gizmos.DrawSphere(leftHand.position, 0.05f);
    }

    public bool Active
    {
        set { active = value; }
        get { return active; }
    }

    public Rigidbody rigidBody
    {
        get {return rb; }
    }

    public Collider rootCollider
    {
        get { return col; }
    }
}
