using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

    public ParticleSystem particleLauncher;    
    public Gradient particleColorGradient;

    private ParticleDecalPool splatDecalPool;
    List<ParticleCollisionEvent> collisionEvents;

    void Start() {
        splatDecalPool = GameObject.Find("_DecalBloodParticles").GetComponent<ParticleDecalPool>();
        if(splatDecalPool == null) {
            Debug.LogError("decal pool not found");
        }
        collisionEvents = new List<ParticleCollisionEvent>();        
    }    

    void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("Cow") && !other.CompareTag("Mais") && !other.CompareTag("Meat") && !other.name.Contains("slice") && !other.name.Contains("Sphere") && !other.CompareTag("Player")) {
            ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);
            for (int i = 0; i < collisionEvents.Count; i++) {
                splatDecalPool.ParticleHit(collisionEvents[i], particleColorGradient);
            }
        }
    }    
}
