#if ENABLE_BEHAVIOR_TREE_CODE

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetHealth : Action
{
    public SharedInt health;
    public SharedGameObject botObject;

    public override TaskStatus OnUpdate()
    {
        health.Value = botObject.Value.GetComponent<PlayerHealth>().health;
        return TaskStatus.Success;
    }
}

#endif
