#if ENABLE_BEHAVIOR_TREE_CODE

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class NavigateToHealth : Action
{
    public SharedGameObject botObject;

    public override TaskStatus OnUpdate()
    {
        BotUtility botUtility = botObject.Value.GetComponent<BotUtility>();

        var target = botUtility.FindClosestHealth();
        if (!botUtility.NavigateTo(target))
            return TaskStatus.Failure;

        return TaskStatus.Success;
    }
}

#endif
