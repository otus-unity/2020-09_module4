using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BotUtility botUtility = animator.GetComponentInParent<BotUtility>();
        var target = botUtility.FindClosestPlayer();
        botUtility.Attack(target);
    }
}
