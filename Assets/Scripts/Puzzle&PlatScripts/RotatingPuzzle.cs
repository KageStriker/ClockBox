using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPuzzle : MonoBehaviour
{
    private float rotateSpeed;
    public bool allowRotate;
    private Vector3 right;


    void Start ()
    {
        rotateSpeed = 20f;
    }

    void Update()
    {
        if (allowRotate)
        {
            Quaternion rotateNinety = Quaternion.LookRotation(right);
            Vector3 rotation = Quaternion.RotateTowards(transform.rotation, rotateNinety, Time.deltaTime * rotateSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            Debug.Log("HERERERERERERERE");
        }
    }

    public void Rotate()
    {
        allowRotate = true;

        if (transform.rotation.y > 0)
        {
            right = transform.TransformDirection(Vector3.right);
        }
        else
            right = Vector3.right;
    }


    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.transform.parent = null;
        }
    }
}