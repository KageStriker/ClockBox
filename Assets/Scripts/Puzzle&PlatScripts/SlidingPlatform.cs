using UnityEngine;
using System.Collections;

public class SlidingPlatform : MonoBehaviour
{
    float counter = 0;
    float speed;

    public float distance;
    public bool move;

    Vector3 platformStartPos;

    private void Start()
    {
        platformStartPos = transform.position;
        speed = 0.4f;
        move = true;
        StartCoroutine(StopMoving());
    }

    void Update()
    {
        if (move)
        {
            counter += Time.deltaTime * speed;

            float movementDir = Mathf.Sin(counter) * distance;

            transform.position = ((transform.forward * movementDir) + platformStartPos);
        }
        else
        {
            return;
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.transform.parent = GetComponentInChildren<Transform>();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.transform.parent = null;
        }
    }

    IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.5f);
        move = false;
    }
}
    