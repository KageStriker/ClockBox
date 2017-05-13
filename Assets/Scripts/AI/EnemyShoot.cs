using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour {
    // Angle for launching shot
    public float launchAngle = 20;
    public Vector3 projSpeed = Vector3.zero;
    //launching shot prefab
    public Rigidbody bulletPrefab;
    //launching transform
    public Transform spawnPoint;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void rayShot()
    {
        //raycast player, -player script health here*************
        Debug.Log("pewpew");

    }


    //launch shot stuff
    public void launchShot(Transform target)
    {
        Vector3 displacement = target.position - transform.position;
        Rigidbody rb = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation)as Rigidbody;
        rb.velocity = CalculateVelocityArc(launchAngle, displacement);

    }

    // this function needs adjustment************
    public Vector3 CalculateVelocityArc(float angle, Vector3 displacement)
    {
        float height = displacement.y;
        displacement.y = 0;
        float horizontalDistance = displacement.magnitude;
        float angleRad = angle * Mathf.Deg2Rad;
        displacement.y = horizontalDistance * Mathf.Tan(angleRad);
        horizontalDistance += height / Mathf.Tan(angleRad);
        float velocityStart = Mathf.Sqrt(horizontalDistance * Physics.gravity.magnitude / Mathf.Sin(2 * angleRad));
        return velocityStart * displacement.normalized;

    }
}
