using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    Transform player;

    float rotateSpeed = 5.0f;
    float scale = 2.0f;

    Vector3 offset = Vector3.zero;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player)
            offset = new Vector3(player.position.x, player.position.y + 5.0f, player.position.z - 3.0f);
    }

	void Update ()
    {
        scale += Input.GetAxis("Mouse ScrollWheel");
        scale = Mathf.Clamp(scale, 1f, 3f);

        if (player)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateSpeed, Vector3.up) * offset;
            offset = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * rotateSpeed, transform.right) * offset;

            transform.position = player.position + offset / scale;
            transform.LookAt(new Vector3(player.position.x, player.position.y + 2.0f, player.position.z));
        }
    }
}
