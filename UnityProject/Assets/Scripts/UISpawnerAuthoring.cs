using UnityEngine;
using Unity.Entities;
using TMPro;
using UnityEngine.UI;

namespace ECS
{
    public class UISpawnerAuthoring : MonoBehaviour
    {
        [Header("UI Elements")]
        public Button spawnButton;
        public Button removeButton;
        public Button increaseButton;
        public Button decreaseButton;
        public TextMeshProUGUI desiredCountText;
        public TextMeshProUGUI currentCountText;
        public Slider spawnRateSlider;
        public TextMeshProUGUI spawnRateText;

        [Header("Audio")]
        public AudioSource buttonAudioSource;
        public AudioClip buttonClickSound;
        
        private UISpawnerComponent spawnerComponent;
        private World world;
        private EntityManager entityManager;

        void Start()
        {
            // Initialize audio
            if (buttonAudioSource == null)
            {
                buttonAudioSource = gameObject.AddComponent<AudioSource>();
                buttonAudioSource.playOnAwake = false;
            }

            // Setup spawn rate slider
            spawnRateSlider.minValue = 100;
            spawnRateSlider.maxValue = 5000;
            spawnRateSlider.value = 1000;
            UpdateSpawnRateText();

            // Get reference to the ECS world and entity manager
            world = World.DefaultGameObjectInjectionWorld;
            entityManager = world.EntityManager;

            // Create entity with UISpawnerComponent
            Entity uiEntity = entityManager.CreateEntity();
            spawnerComponent = new UISpawnerComponent
            {
                DesiredSphereCount = 50000,
                CurrentSphereCount = 0,
                ShouldSpawnSpheres = false,
                ShouldRemoveAllSpheres = false,
                SpawnRatePerFrame = (int)spawnRateSlider.value
            };
            entityManager.AddComponentData(uiEntity, spawnerComponent);

            // Setup button listeners
            spawnButton.onClick.AddListener(() => {
                PlayButtonSound();
                OnSpawnButtonClick();
            });
            
            removeButton.onClick.AddListener(() => {
                PlayButtonSound();
                OnRemoveButtonClick();
            });
            
            increaseButton.onClick.AddListener(() => {
                PlayButtonSound();
                OnIncreaseButtonClick();
            });
            
            decreaseButton.onClick.AddListener(() => {
                PlayButtonSound();
                OnDecreaseButtonClick();
            });

            spawnRateSlider.onValueChanged.AddListener(OnSpawnRateChanged);

            // Initial UI update
            UpdateUIText();
        }

        void PlayButtonSound()
        {
            if (buttonAudioSource != null && buttonClickSound != null)
            {
                buttonAudioSource.PlayOneShot(buttonClickSound);
            }
        }
        
        void OnSpawnRateChanged(float value)
        {
            var entity = GetUIEntity();
            var data = entityManager.GetComponentData<UISpawnerComponent>(entity);
            data.SpawnRatePerFrame = (int)value;
            entityManager.SetComponentData(entity, data);
            UpdateSpawnRateText();
        }

        void UpdateSpawnRateText()
        {
            spawnRateText.text = $"Spawn Rate: {spawnRateSlider.value}/frame";
        }

        void OnSpawnButtonClick()
        {
            var entity = GetUIEntity();
            var data = entityManager.GetComponentData<UISpawnerComponent>(entity);
            data.ShouldSpawnSpheres = true;
            entityManager.SetComponentData(entity, data);
        }

        void OnRemoveButtonClick()
        {
            var entity = GetUIEntity();
            var data = entityManager.GetComponentData<UISpawnerComponent>(entity);
            data.ShouldRemoveAllSpheres = true;
            entityManager.SetComponentData(entity, data);
        }


        void OnIncreaseButtonClick()
        {
            var entity = GetUIEntity();
            var data = entityManager.GetComponentData<UISpawnerComponent>(entity);
            data.DesiredSphereCount += 10000;
            entityManager.SetComponentData(entity, data);
            UpdateUIText();
        }

        void OnDecreaseButtonClick()
        {
            var entity = GetUIEntity();
            var data = entityManager.GetComponentData<UISpawnerComponent>(entity);
            data.DesiredSphereCount = Mathf.Max(0, data.DesiredSphereCount - 10000);
            entityManager.SetComponentData(entity, data);
            UpdateUIText();
        }

        Entity GetUIEntity()
        {
            var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<UISpawnerComponent>());
            var entity = query.GetSingletonEntity();
            query.Dispose(); 
            return entity;
        }

        void UpdateUIText()
        {
            var entity = GetUIEntity();
            var data = entityManager.GetComponentData<UISpawnerComponent>(entity);
            desiredCountText.text = $"Desired Count: {data.DesiredSphereCount}";
            currentCountText.text = $"Current Count: {data.CurrentSphereCount}";
        }

        void Update()
        {
            if (world != null && world.IsCreated)
            {
                UpdateUIText();
            }
        }
    }
}
