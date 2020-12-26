using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.AI.Planner;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Unity.Burst;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.BotPlan;
using Generated.AI.Planner.StateRepresentation.Enums;

namespace Generated.AI.Planner.Plans.BotPlan
{
    [BurstCompile]
    struct NavigateToEnemy : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_AgentIndex = 0;
        const int k_TargetIndex = 1;
        const int k_MaxArguments = 2;

        public static readonly string[] parameterNames = {
            "Agent",
            "Target",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        internal NavigateToEnemy(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Agent", StringComparison.OrdinalIgnoreCase))
                 return k_AgentIndex;
            if (string.Equals(parameterName, "Target", StringComparison.OrdinalIgnoreCase))
                 return k_TargetIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            var AgentFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Generated.AI.Planner.StateRepresentation.Agent>(),[1] = ComponentType.ReadWrite<Unity.AI.Planner.DomainLanguage.TraitBased.Location>(),  };
            var TargetFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Unity.AI.Planner.DomainLanguage.TraitBased.Location>(),[1] = ComponentType.ReadWrite<Generated.AI.Planner.StateRepresentation.Target>(),  };
            var AgentObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(AgentObjectIndices, AgentFilter);
            
            var TargetObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(TargetObjectIndices, TargetFilter);
            
            var AgentBuffer = stateData.AgentBuffer;
            var TargetBuffer = stateData.TargetBuffer;
            
            

            for (int i0 = 0; i0 < AgentObjectIndices.Length; i0++)
            {
                var AgentIndex = AgentObjectIndices[i0];
                var AgentObject = stateData.TraitBasedObjects[AgentIndex];
                
                if (!(AgentBuffer[AgentObject.AgentIndex].Navigating == false))
                    continue;
                
                
                
            
            

            for (int i1 = 0; i1 < TargetObjectIndices.Length; i1++)
            {
                var TargetIndex = TargetObjectIndices[i1];
                var TargetObject = stateData.TraitBasedObjects[TargetIndex];
                
                
                if (!(AgentBuffer[AgentObject.AgentIndex].UniqueId == TargetBuffer[TargetObject.TargetIndex].UniqueId))
                    continue;
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_AgentIndex] = AgentIndex,
                                                       [k_TargetIndex] = TargetIndex,
                                                    };
                argumentPermutations.Add(actionKey);
            
            }
            
            }
            AgentObjectIndices.Dispose();
            TargetObjectIndices.Dispose();
            AgentFilter.Dispose();
            TargetFilter.Dispose();
        }

        StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> ApplyEffects(ActionKey action, StateEntityKey originalStateEntityKey)
        {
            var originalState = m_StateDataContext.GetStateData(originalStateEntityKey);
            var originalStateObjectBuffer = originalState.TraitBasedObjects;
            var originalAgentObject = originalStateObjectBuffer[action[k_AgentIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newAgentBuffer = newState.AgentBuffer;
            {
                    var @Agent = newAgentBuffer[originalAgentObject.AgentIndex];
                    @Agent.@Navigating = true;
                    newAgentBuffer[originalAgentObject.AgentIndex] = @Agent;
            }

            

            var reward = Reward(originalState, action, newState);
            var StateTransitionInfo = new StateTransitionInfo { Probability = 1f, TransitionUtilityValue = reward };
            var resultingStateKey = m_StateDataContext.GetStateDataKey(newState);

            return new StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>(originalStateEntityKey, action, resultingStateKey, StateTransitionInfo);
        }

        float Reward(StateData originalState, ActionKey action, StateData newState)
        {
            var reward = 0f;
            {
                var param0 = originalState.GetTraitOnObjectAtIndex<Unity.AI.Planner.DomainLanguage.TraitBased.Location>(action[0]);
                var param1 = originalState.GetTraitOnObjectAtIndex<Unity.AI.Planner.DomainLanguage.TraitBased.Location>(action[1]);
                reward -= new global::Unity.AI.Planner.Navigation.LocationDistance().RewardModifier( param0, param1);
            }

            return reward;
        }

        public void Execute(int jobIndex)
        {
            m_StateDataContext.JobIndex = jobIndex;

            var stateEntityKey = m_StatesToExpand[jobIndex];
            var stateData = m_StateDataContext.GetStateData(stateEntityKey);

            var argumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            GenerateArgumentPermutations(stateData, argumentPermutations);

            var transitionInfo = new NativeArray<NavigateToEnemyFixupReference>(argumentPermutations.Length, Allocator.Temp);
            for (var i = 0; i < argumentPermutations.Length; i++)
            {
                transitionInfo[i] = new NavigateToEnemyFixupReference { TransitionInfo = ApplyEffects(argumentPermutations[i], stateEntityKey) };
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<NavigateToEnemyFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(transitionInfo);

            transitionInfo.Dispose();
            argumentPermutations.Dispose();
        }

        
        public static T GetAgentTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_AgentIndex]);
        }
        
        public static T GetTargetTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_TargetIndex]);
        }
        
    }

    public struct NavigateToEnemyFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


