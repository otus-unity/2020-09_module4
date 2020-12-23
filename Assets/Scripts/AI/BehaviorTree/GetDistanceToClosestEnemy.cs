#if ENABLE_BEHAVIOR_TREE_CODE

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetDistanceToClosestEnemy : Action
{
    public SharedFloat distanceToEnemy;
    public SharedGameObject botObject;

    public override TaskStatus OnUpdate()
    {
        distanceToEnemy.Value = botObject.Value.GetComponent<BotUtility>().GetDistanceToClosestEnemy();
        return TaskStatus.Success;
    }
}

#endif
