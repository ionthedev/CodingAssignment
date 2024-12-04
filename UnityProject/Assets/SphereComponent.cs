using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;

namespace ECS
{
    public struct SphereComponent : IComponentData
    {
        public float3 moveDirection;
        public float moveSpeed;
        public float updateInterval;
    }
}
