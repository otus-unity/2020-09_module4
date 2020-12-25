#if ENABLE_BEHAVIOR_TREE_CODE

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class NavigateToAmmo : Action
{
    public SharedGameObject botObject;

    public override TaskStatus OnUpdate()
    {
        BotUtility botUtility = botObject.Value.GetComponent<BotUtility>();

        var target = botUtility.FindClosestAmmo();
        if (!botUtility.NavigateTo(target))
            return TaskStatus.Failure;

        return TaskStatus.Success;
    }
}

#endif
