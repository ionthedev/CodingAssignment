using UnityEngine;
using UnityEngine.Rendering;

namespace ECS
{
    public class SceneOptimizer : MonoBehaviour
    {
        void Awake()
        {
            // Frame rate and performance settings
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            
            // Mobile-specific graphics settings
            QualitySettings.antiAliasing = 0;
            QualitySettings.shadows = ShadowQuality.Disable;
            QualitySettings.softParticles = false;
            QualitySettings.realtimeReflectionProbes = false;
            QualitySettings.billboardsFaceCameraPosition = true;
            
            // Camera optimizations
            var mainCamera = Camera.main;
            if (mainCamera != null)
            {
                mainCamera.allowHDR = false;
                mainCamera.allowMSAA = false;
                mainCamera.farClipPlane = 30f;
                mainCamera.layerCullDistances = new float[32];
                for (int i = 0; i < 32; i++)
                {
                    mainCamera.layerCullDistances[i] = 30f;
                }
            }
            
            // Memory management
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            
            // Garbage collection settings
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
            
            // Set low quality level for mobile
            QualitySettings.SetQualityLevel(0, true);
            
            // Reduce GPU overhead
            Graphics.activeTier = GraphicsTier.Tier1;
            Shader.globalMaximumLOD = 100;

            // Additional mobile optimizations
            Application.targetFrameRate = 60; 
            QualitySettings.skinWeights = SkinWeights.TwoBones; 
            QualitySettings.lodBias = 0.3f; 
            QualitySettings.particleRaycastBudget = 4; 
            QualitySettings.asyncUploadTimeSlice = 1; 
            QualitySettings.asyncUploadBufferSize = 4; 
        }
    }
}
