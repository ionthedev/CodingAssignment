using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS
{
    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    public partial struct SphereSystem : ISystem
    {
        [BurstCompile]
        [WithAll(typeof(SphereComponent), typeof(LocalTransform))]
        partial struct MoveSphereJob : IJobEntity
        {
            public float DeltaTime;

            void Execute(ref LocalTransform transform, ref SphereComponent sphere)
            {
                if (sphere.moveSpeed <= 0) return;
                
                sphere.moveSpeed = math.lerp(sphere.moveSpeed, 0, DeltaTime);
                if (sphere.moveSpeed > 0.01f)
                {
                    transform.Position += sphere.moveDirection * sphere.moveSpeed * DeltaTime;
                }
            }
        }

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new MoveSphereJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            job.Schedule();
        }

        public void OnDestroy(ref SystemState state)
        {
        }
    }
}