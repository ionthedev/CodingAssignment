using Unity.Burst;
using Unity.Entities;

namespace ECS
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(UISpawnerSystem))]
    public partial struct SphereRemovalSystem : ISystem
    {
        private EntityQuery sphereQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            sphereQuery = state.GetEntityQuery(ComponentType.ReadOnly<SphereComponent>());
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingletonEntity<UISpawnerComponent>(out Entity uiEntity))
                return;

            var uiSpawner = SystemAPI.GetComponentRW<UISpawnerComponent>(uiEntity);

            if (!uiSpawner.ValueRO.ShouldRemoveAllSpheres)
                return;

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            ecb.DestroyEntity(sphereQuery);

            uiSpawner.ValueRW.ShouldRemoveAllSpheres = false;
            uiSpawner.ValueRW.CurrentSphereCount = 0;
        }
    }
}