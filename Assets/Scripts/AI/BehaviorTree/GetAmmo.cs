#if ENABLE_BEHAVIOR_TREE_CODE

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetAmmo : Action
{
    public SharedInt ammo;
    public SharedGameObject botObject;

    public override TaskStatus OnUpdate()
    {
        ammo.Value = botObject.Value.GetComponent<PlayerAmmo>().ammo;
        return TaskStatus.Success;
    }
}

#endif
