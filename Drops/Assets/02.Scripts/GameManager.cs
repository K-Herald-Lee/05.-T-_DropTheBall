using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dongle lastDongle;    
    public GameObject donglePrefab;
    public Transform dongleGroup;
    public List<Dongle> donglePool;

    public GameObject effectPrefab;
    public Transform effectGroup;
    public List<ParticleSystem> effectPool;

    [Range(1,30)]
    public int poolSize;
    public int poolIndex;

    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;
    public enum sfx {Attach, Button, GameOver, LevelUP, Next};
    int sfxIndex;

    public int maxLevel;
    public int score;
    public bool isGameOver;
    
    private void Awake() 
    {
        Application.targetFrameRate = 60;
        effectPool = new List<ParticleSystem>();
        donglePool = new List<Dongle>();
        for(int index=0; index<poolSize; index++){
            MakeDongle();
        }

    }
    void Start()
    {
        bgmPlayer.Play();
        GenDongle();
    }

    Dongle MakeDongle()
    {
        // init effect
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        instantEffectObj.name = "Effect" + effectPool.Count;
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();
        effectPool.Add(instantEffect);

        // init dongle
        GameObject instant = Instantiate(donglePrefab, dongleGroup);
        instant.name = "Dongle" + donglePool.Count;
        Dongle instantDongle = instant.GetComponent<Dongle>();
        instantDongle.manager = this;
        donglePool.Add(instantDongle);
        
        // pairing
        instantDongle.effect = instantEffect;
        return instantDongle;
    }

    Dongle InitDongle()
    {
        for(int index=0; index<donglePool.Count; index++){
            poolIndex = (poolIndex + 1) % donglePool.Count;
            if(!donglePool[poolIndex].gameObject.activeSelf){
                return donglePool[poolIndex];
            }
        }
        return MakeDongle();
    }  

    void GenDongle()
    {
        if(!isGameOver){
            lastDongle = InitDongle();
            lastDongle.level = Random.Range(0, maxLevel);
            lastDongle.gameObject.SetActive(true);
            
            SfxPlay(sfx.Next);
            StartCoroutine(WaitNext());
        }        
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

    public void GameOver()
    {
        if(isGameOver){
            return;
        }
        isGameOver = true;

        StartCoroutine(GameOverRoutine());
        
        // print score
    }
    IEnumerator GameOverRoutine()
    {
        // destroy every dongles
        Dongle[] dongleList = FindObjectsOfType<Dongle>();
        for(int index=0; index<dongleList.Length; index++){
            dongleList[index].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        bgmPlayer.Stop();
        SfxPlay(sfx.GameOver);             
    }
    public void SfxPlay(sfx type)
    {
        switch(type){
            case sfx.Attach:
                sfxPlayer[sfxIndex].clip = sfxClip[0];
                break;
            case sfx.Button:
                sfxPlayer[sfxIndex].clip = sfxClip[1];
                break;
            case sfx.GameOver:
                sfxPlayer[sfxIndex].clip = sfxClip[2];
                break;
            case sfx.LevelUP:
                sfxPlayer[sfxIndex].clip = sfxClip[3];
                break;
            case sfx.Next:
                sfxPlayer[sfxIndex].clip = sfxClip[4];
                break;
        }
        sfxPlayer[sfxIndex].Play();
        sfxIndex = (sfxIndex + 1) % sfxPlayer.Length;

    }
}
