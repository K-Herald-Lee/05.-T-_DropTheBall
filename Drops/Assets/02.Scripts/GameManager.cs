using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dongle lastDongle;
    public GameObject donglePrefab;
    public Transform dongleGroup;
    
    private void Awake() 
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        GenDongle();
    }

    Dongle GetNextDongle()
    {
        GameObject instant = Instantiate(donglePrefab, dongleGroup);
        Dongle instantDongle = instant.GetComponent<Dongle>();
        return instantDongle;
    }  

    void GenDongle()
    {
        Dongle newDongle = GetNextDongle();
        lastDongle = newDongle;
        lastDongle.level = Random.Range(0,8);
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
