using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;

public class RootMotion : StateMachineBehaviour {
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = false;
    }
}