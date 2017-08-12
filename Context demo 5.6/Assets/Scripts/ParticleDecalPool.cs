using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalPool : MonoBehaviour {

    public int maxDecals;
    public float decalSizeMin;
    public float decalSizeMax;

    public float fluidSpeed;
    public float fluidHeight;
    private float fluidNoise;

    private ParticleSystem decalParticleSystem;
    private int particleDecalDataIndex;
    private ParticleDecalData[] particleData;
    private ParticleSystem.Particle[] particles;    

    void Start () {        
        decalParticleSystem = GetComponent<ParticleSystem>();
        particleDecalDataIndex = 0;
        particles = new ParticleSystem.Particle[maxDecals];
        particleData = new ParticleDecalData[maxDecals];
        for(int i = 0; i < maxDecals; i++) {
            particleData[i] = new ParticleDecalData();
        }        
    }

    void Update()
    {
        NoiseParticles();
    }

    public void ParticleHit(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    {
        SetParticleData(particleCollisionEvent, colorGradient);
        DisplayParticles();
    }    

    void SetParticleData(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    {
        if(particleDecalDataIndex >= maxDecals)
            particleDecalDataIndex = 0;

        particleData[particleDecalDataIndex].position = particleCollisionEvent.intersection;
        particleData[particleDecalDataIndex].position.y += Random.Range(0.0f, 0.3f);
        Vector3 particleRotationEuler = Quaternion.LookRotation(particleCollisionEvent.normal).eulerAngles;
        particleRotationEuler.z = Random.Range(0, 360);
        particleData[particleDecalDataIndex].rotation = particleRotationEuler;
        particleData[particleDecalDataIndex].size = Random.Range(decalSizeMin, decalSizeMax);
        particleData[particleDecalDataIndex].color = colorGradient.Evaluate(Random.Range(0f, 1f));

        particleDecalDataIndex++;
    }

    void DisplayParticles()
    {
        for (int i = 0; i < particleData.Length; i++) {
            particles[i].position = particleData[i].position;            
            particles[i].rotation3D = particleData[i].rotation;
            particles[i].startSize = particleData[i].size;
            particles[i].startColor = particleData[i].color;
        }
    }  
    
    void NoiseParticles()
    {
        for (int i = 0; i < particleData.Length; i++) {
            float pX = particles[i].position.x + (Time.timeSinceLevelLoad * fluidSpeed);
            float pZ = particles[i].position.z + (Time.timeSinceLevelLoad * fluidSpeed);
            Vector3 particlesPos = particleData[i].position;
            particles[i].position = new Vector3(particlesPos.x, particlesPos.y + (Mathf.PerlinNoise(pX, pZ)) * fluidHeight, particlesPos.z);
        }
        decalParticleSystem.SetParticles(particles, particles.Length);
    }  
}
