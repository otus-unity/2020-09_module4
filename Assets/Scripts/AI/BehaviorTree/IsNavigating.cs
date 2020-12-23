#if ENABLE_BEHAVIOR_TREE_CODE

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsNavigating : Conditional
{
    public SharedGameObject botObject;

    public override TaskStatus OnUpdate()
    {
        BotUtility botUtility = botObject.Value.GetComponent<BotUtility>();
        return (botUtility.IsNavigating() ? TaskStatus.Success : TaskStatus.Failure);
    }
}

#endif
