using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(SphereSpawnerSystem))]
    public partial struct UISpawnerSystem : ISystem
    {
        private EntityQuery sphereQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            sphereQuery = state.GetEntityQuery(ComponentType.ReadOnly<SphereComponent>());
            state.RequireForUpdate<UISpawnerComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.CompleteDependency();
        
            if (!SystemAPI.TryGetSingletonEntity<UISpawnerComponent>(out var uiEntity))
                return;
            
            var uiSpawner = SystemAPI.GetComponentRW<UISpawnerComponent>(uiEntity);
            var currentCount = sphereQuery.CalculateEntityCount();

            uiSpawner.ValueRW.CurrentSphereCount = currentCount;
        }
    }
}