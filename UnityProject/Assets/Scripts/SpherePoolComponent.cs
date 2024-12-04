using Unity.Entities;

namespace ECS 
{
    // Main pool management component
    public struct SpherePoolComponent : IComponentData 
    {
        public int PoolSize;
        public int ActiveCount;
        public Entity SpherePrefab;
    }

    public struct PooledSphereTag : IComponentData 
    {
        // Empty tag component
    }

    public struct SphereActiveState : ISharedComponentData
    {
        public bool IsActive;
    }
}