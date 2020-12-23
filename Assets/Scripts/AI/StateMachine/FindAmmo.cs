using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAmmo : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BotUtility botUtility = animator.GetComponentInParent<BotUtility>();
        AmmoBox target = botUtility.FindClosestAmmo();
        if (!botUtility.NavigateTo(target))
            animator.SetTrigger("failed");
    }
}
