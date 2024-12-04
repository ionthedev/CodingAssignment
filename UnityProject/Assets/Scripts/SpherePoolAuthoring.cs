using UnityEngine;
using Unity.Entities;

namespace ECS 
{
    public class SpherePoolAuthoring : MonoBehaviour 
    {
        public GameObject spherePrefab;
        public int initialPoolSize = 70000;
    }

    public class SpherePoolBaker : Baker<SpherePoolAuthoring> 
    {
        public override void Bake(SpherePoolAuthoring authoring) 
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpherePoolComponent 
            {
                PoolSize = authoring.initialPoolSize,
                ActiveCount = 0,
                SpherePrefab = GetEntity(authoring.spherePrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}