using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
    public UnityEvent myEvent;

    SlidingPlatform sP;
    RotatingPuzzle[] rP;
    
    public void RotatePlatfrom(GameObject platformsParent)
    {
        rP = platformsParent.GetComponentsInChildren<RotatingPuzzle>();
        for (int i = 0; i < rP.Length; i++)
        {
            rP[i].Rotate();
        }
    }

    public void MovePlatform(GameObject platform)
    {
        sP = platform.GetComponent<SlidingPlatform>();
        sP.move = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                myEvent.Invoke();
            }
        }
    }
}