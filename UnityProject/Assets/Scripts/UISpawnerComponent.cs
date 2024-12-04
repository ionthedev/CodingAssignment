using Unity.Entities;

namespace ECS
{
    public struct UISpawnerComponent : IComponentData
    {
        public int DesiredSphereCount;
        public int CurrentSphereCount;
        public bool ShouldSpawnSpheres;
        public bool ShouldRemoveAllSpheres;
        public int SpawnRatePerFrame;
    }
}