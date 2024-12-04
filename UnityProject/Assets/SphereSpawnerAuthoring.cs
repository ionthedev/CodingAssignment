using UnityEngine;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine.Serialization;


namespace ECS
{
    public class SphereSpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public float spawnRate;
    
    }

    class SphereSpawnerBaker : Baker<SphereSpawnerAuthoring>
    {
        public override void Bake(SphereSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SphereSpawnerComponent
            {
                Prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic ),
                Position = authoring.transform.position,
                NextSpawnTime = 0.0f,
                SpawnRate = authoring.spawnRate,
            });
        }
    }
}