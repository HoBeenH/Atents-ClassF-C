using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonIdle : StateMachineBehaviour
{
    DragonController dragon;

    Transform target;

    Vector3 directvector;
    Vector3 rotatevector;

    float distance;

    float idletime = 0f;

    int randompattern;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dragon = animator.GetComponent<DragonController>();
        target = dragon.playerobject.transform;
        target.position = dragon.playerobject.transform.position;

        animator.ResetTrigger("Walk");
        animator.ResetTrigger("TailattackIdle");
        animator.ResetTrigger("FallDownReady");
        animator.ResetTrigger("FireRainReady");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distance = Vector3.Distance(target.position, animator.transform.position);

        //target.position = dragon.playerobject.transform.position;
        //directvector = target.position - animator.transform.position;
        //rotatevector = Vector3.RotateTowards(animator.transform.forward, directvector.normalized, Time.deltaTime, 0.0f);
        //animator.transform.rotation = Quaternion.LookRotation(rotatevector*Time.deltaTime*0.2f);

        idletime += Time.deltaTime;  

        if(dragon.hp <= 70f && DragonController.instance.dragonfalldown == true)
        {            
            animator.SetTrigger("FallDownReady");
        }
        else if (dragon.hp <= 30f && DragonController.instance.dragonfirerain == true && DragonController.instance.dragonfalldown == false)
        {
            animator.SetTrigger("FireRainReady");
        }


        if (idletime > 1f && distance > 12f)
        {
            animator.SetTrigger("Walk");

            idletime = 0f;
        }
        else if (distance <= 12f && idletime > 1f)
        {
            randompattern = Random.Range(0, 10);

            if (randompattern < 6)
            {
                animator.SetTrigger("TailattackIdle");
                idletime = 0f;
            }
            else if (6<=randompattern && randompattern < 9)
            {
                animator.SetTrigger("RushIdle");
                idletime = 0f;
            }
            else
            {
                animator.SetTrigger("BreathIdle");
                idletime = 0f;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
