using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("----------[ Core ]")]
    public int maxLevel;
    public int score;
    public bool isGameOver;

    [Header("----------[ Object Pooling ]")]
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

    [Header("----------[ Audio ]")]
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;
    public enum sfx {Attach, Button, GameOver, LevelUP, Next};
    int sfxIndex;

    [Header("----------[ UI ]")]
    public GameObject gameoverGroup;
    public GameObject titleGroup;
    public Text textScore;
    public Text textBestScore;
    public Text textSubScore;
    public GameObject line;

    
    private void Awake() 
    {
        Application.targetFrameRate = 60;
        effectPool = new List<ParticleSystem>();
        donglePool = new List<Dongle>();
        for(int index=0; index<poolSize; index++){
            MakeDongle();
        }
        if(!PlayerPrefs.HasKey("BestScore")){
            PlayerPrefs.SetInt("BestScore", 0);
        }
        textBestScore.text = PlayerPrefs.GetInt("BestScore").ToString();

    }
    public void GameStart()
    {
        line.SetActive(true);
        textScore.gameObject.SetActive(true);
        textBestScore.gameObject.SetActive(true);

        titleGroup.SetActive(false);

        SfxPlay(sfx.Button);
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
        gameoverGroup.SetActive(true);
        StartCoroutine(GameOverRoutine());
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
        if (score > PlayerPrefs.GetInt("BestScore")){
            PlayerPrefs.SetInt("BestScore",score);
        }
        textSubScore.text = "점수: " + score.ToString();
        bgmPlayer.Stop();
        SfxPlay(sfx.GameOver);             
    }

    public void Restart()
    {
        SfxPlay(sfx.Button);
        StartCoroutine(RestartRoutine());
    }
    IEnumerator RestartRoutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main");
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

    private void Update() 
    {
        if (Input.GetButtonDown("Cancel")){
            Application.Quit();
        }        
    }

    private void LateUpdate() {
        textScore.text = score.ToString();        
    }
}
