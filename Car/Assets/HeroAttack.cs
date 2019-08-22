using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HeroAttack : StateMachineBehaviour
{
    HeroController hc;
    bool onHit = false;
    CinemachineImpulseSource impulseManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hc = animator.GetComponentInParent<HeroController>();
        impulseManager = animator.GetComponentInParent<CinemachineImpulseSource>();
        onHit = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        //if (stateInfo.normalizedTime > 0.5f && hc.hit && hc.hit.transform.gameObject.layer == LayerMask.NameToLayer("Monsters") && !onHit)
        //{
        //    onHit = true;

        //    impulseManager.GenerateImpulse();
        //    hc.hitFX.transform.position = hc.hit.transform.position;
        //    hc.hitFX.Play();

        //    var mobs = hc.hit.transform.GetComponent<Character>().parentPlayer.GetComponent<PlayerController>().Damaged(hc.damage);
        //    if (mobs == null)
        //        return;
        //    foreach (var m in mobs)
        //    {
        //        if (hc.foundMonsters.Contains(m))
        //        {
        //            hc.foundMonsters.Remove(m);
        //        }
        //    }
        //}
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

    //public void Attack()
    //{
    //    if (hc.hit && hc.hit.transform.gameObject.layer == LayerMask.NameToLayer("Monsters") && !onHit)
    //    {
    //        onHit = true;

    //        impulseManager.GenerateImpulse();
    //        hc.hitFX.transform.position = hc.hit.transform.position;
    //        hc.hitFX.Play();

    //        var mobs = hc.hit.transform.GetComponent<Character>().parentPlayer.GetComponent<PlayerController>().Damaged(hc.damage);
    //        if (mobs == null)
    //            return;
    //        foreach (var m in mobs)
    //        {
    //            if (hc.foundMonsters.Contains(m))
    //            {
    //                hc.foundMonsters.Remove(m);
    //            }
    //        }
    //    }
    //}
}
