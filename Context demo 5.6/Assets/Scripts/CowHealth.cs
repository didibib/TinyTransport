using UnityEngine;
using System.Collections;

public class CowHealth : MonoBehaviour
{
    public int startingHealth;
    public int fatHealth;

    private Vector3 hitPos;

    private ExplodedCows ExplCows;
    private MeatCollecter meatCollecter;
    private ParticleLauncherPool particleLauncherPool;
    private int currentHealth;
    
    void Start()
    {
        ExplCows = GameObject.Find("_ExplodedCows").GetComponent<ExplodedCows>();
        currentHealth = startingHealth;
        meatCollecter = GameObject.Find("_MeatCollector").GetComponent<MeatCollecter>();
        particleLauncherPool = GameObject.Find("_DecalBloodParticles").GetComponent<ParticleLauncherPool>();
    }

    void Update()
    {
        //Debug.Log("current health " + currentHealth);
    }

    public void EatMais(int damage, Vector3 contactPoint)
    {
        hitPos = contactPoint;
        transform.GetComponent<CowMovement>().AddFood();
        currentHealth -= damage;
        if (currentHealth <= 0) {            
            transform.GetComponent<CowMovement>().defeated = true;
            StartCoroutine(Defeated());
        } else if (currentHealth <= fatHealth) {
            transform.GetComponent<CowMovement>().beingfat = true;
        }
    }

    IEnumerator Defeated()
    {
        GameManager.instance.AddScore(1);        
        yield return new WaitForSeconds(.5f);
        if (transform.name.Contains("koe white")) {
            ExplCows.InstantiateExplCowWhite(transform);
        } else {
            ExplCows.InstantiateExplCowBlack(transform);
        }
        particleLauncherPool.BloodStream(hitPos);
        meatCollecter.InstantiateMeat(transform.position);
        gameObject.SetActive(false);   
    }
}