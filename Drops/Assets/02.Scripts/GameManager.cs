using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dongle lastDongle;
    
    public GameObject donglePrefab;
    public Transform dongleGroup;

    public GameObject effectPrefab;
    public Transform effectGroup;

    public int maxLevel;
    
    private void Awake() 
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        GenDongle();
    }

    Dongle InitDongle()
    {
        // init effect
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();

        // init dongle
        GameObject instant = Instantiate(donglePrefab, dongleGroup);
        Dongle instantDongle = instant.GetComponent<Dongle>();
        
        // pairing
        instantDongle.effect = instantEffect;

        return instantDongle;
    }  

    void GenDongle()
    {
        Dongle newDongle = InitDongle();
        lastDongle = newDongle;
        lastDongle.manager = this;
        lastDongle.level = Random.Range(0, maxLevel);
        lastDongle.gameObject.SetActive(true);

        StartCoroutine(WaitNext());
    }

    IEnumerator WaitNext()
    {
        while(lastDongle!=null){
            yield return null;           
        }
        yield return new WaitForSeconds(0.5f);

        GenDongle();
    }

    public void TouchDown()
    {
        if (lastDongle != null){
            lastDongle.Drag();
        }        
    }

    public void TouchUp()
    {
        if (lastDongle != null){
            lastDongle.Drop();
            lastDongle = null;
        }        
    }
}
