using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoorScript : MonoBehaviour
{
    Animator anim;
       
	void Start () {
        anim = GetComponent<Animator>();
	}
    
    void OpenDoor()
    {
        anim.SetTrigger("OpenDoor");
    }
}
