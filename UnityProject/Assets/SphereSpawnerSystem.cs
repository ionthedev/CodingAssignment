using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS 
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(UISpawnerSystem))]
    public partial struct SphereSpawnerSystem : ISystem 
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) 
        {
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<SphereComponent>();
            state.EntityManager.CreateEntityQuery(builder);

            builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<UISpawnerComponent>();
            state.EntityManager.CreateEntityQuery(builder);
            
            builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<SphereSpawnerComponent>();
            state.EntityManager.CreateEntityQuery(builder);

            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) 
        {
            var sphereQuery = SystemAPI.QueryBuilder().WithAll<SphereComponent>().Build();
            
            if (!SystemAPI.TryGetSingletonEntity<UISpawnerComponent>(out Entity uiEntity))
                return;

            if (!SystemAPI.TryGetSingletonEntity<SphereSpawnerComponent>(out Entity spawnerEntity))
                return;

            var uiSpawner = state.EntityManager.GetComponentData<UISpawnerComponent>(uiEntity);
            var spawner = state.EntityManager.GetComponentData<SphereSpawnerComponent>(spawnerEntity);

            if (!uiSpawner.ShouldSpawnSpheres)
                return;

            int currentCount = sphereQuery.CalculateEntityCount();
            int remainingToSpawn = uiSpawner.DesiredSphereCount - currentCount;

            if (remainingToSpawn <= 0)
            {
                uiSpawner.ShouldSpawnSpheres = false;
                state.EntityManager.SetComponentData(uiEntity, uiSpawner);
                return;
            }

            const int spawnPerFrame = 1000;
            int spawnThisFrame = math.min(spawnPerFrame, remainingToSpawn);

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            var random = Random.CreateFromIndex((uint)SystemAPI.Time.ElapsedTime);

            for (int i = 0; i < spawnThisFrame; i++)
            {
                Entity newEntity = ecb.Instantiate(spawner.Prefab);
                float3 randomPosition = random.NextFloat3(
                    new float3(-10f, 0f, -10f),
                    new float3(10f, 5f, 10f)
                );

                ecb.SetComponent(newEntity, LocalTransform.FromPosition(randomPosition));
                ecb.AddComponent(newEntity, new SphereComponent
                {
                    moveDirection = random.NextFloat3Direction(),
                    moveSpeed = 10f
                });
            }

            ecb.SetComponent(uiEntity, new UISpawnerComponent
            {
                CurrentSphereCount = currentCount + spawnThisFrame,
                DesiredSphereCount = uiSpawner.DesiredSphereCount,
                ShouldSpawnSpheres = (currentCount + spawnThisFrame) < uiSpawner.DesiredSphereCount,
                ShouldRemoveAllSpheres = uiSpawner.ShouldRemoveAllSpheres
            });
        }

        public void OnDestroy(ref SystemState state) { }
    }
}