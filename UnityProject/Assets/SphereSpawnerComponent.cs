using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    public struct SphereSpawnerComponent : IComponentData
    {
        public Entity Prefab;
        public float3 Position;
        public float SpawnRate;
        public float NextSpawnTime;
    }

}
   