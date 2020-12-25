using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigateToEnemy : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BotUtility botUtility = animator.GetComponentInParent<BotUtility>();
        var target = botUtility.FindClosestPlayer();
        if (!botUtility.NavigateTo(target))
            animator.SetTrigger("failed");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BotUtility botUtility = animator.GetComponentInParent<BotUtility>();
        if (!botUtility.IsNavigating())
            animator.SetTrigger("failed");
        animator.Play("Idle");
    }
}
