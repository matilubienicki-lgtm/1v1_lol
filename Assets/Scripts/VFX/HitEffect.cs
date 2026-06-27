using UnityEngine;
using System.Collections;

namespace Game.VFX
{
    /// <summary>
    /// Creates visual effects when a player is hit.
    /// </summary>
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hitParticles;
        [SerializeField] private float duration = 0.5f;

        /// <summary>
        /// Plays hit effect at a position.
        /// </summary>
        public static void PlayHitEffect(Vector3 position, Vector3 hitDirection)
        {
            GameObject effectGO = new GameObject("HitEffect");
            effectGO.transform.position = position;
            
            ParticleSystem ps = effectGO.AddComponent<ParticleSystem>();
            
            // Configure particles
            var main = ps.main;
            main.duration = 0.5f;
            main.loop = false;
            
            var emission = ps.emission;
            emission.rateOverTime = 50;
            
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            
            var vel = ps.velocityOverLifetime;
            vel.x = hitDirection.x * 5f;
            vel.y = hitDirection.y * 5f;
            vel.z = hitDirection.z * 5f;
            
            Destroy(effectGO, 1f);
            
            Debug.Log($"[HitEffect] Hit effect at {position}");
        }

        /// <summary>
        /// Plays muzzle flash effect.
        /// </summary>
        public static void PlayMuzzleFlash(Vector3 position)
        {
            GameObject flashGO = new GameObject("MuzzleFlash");
            flashGO.transform.position = position;
            
            Light light = flashGO.AddComponent<Light>();
            light.type = LightType.Point;
            light.range = 10f;
            light.intensity = 2f;
            light.color = Color.yellow;
            
            Destroy(flashGO, 0.1f);
            
            Debug.Log($"[HitEffect] Muzzle flash at {position}");
        }
    }
}
