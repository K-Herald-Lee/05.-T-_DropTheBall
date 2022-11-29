using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.Audio;

public class PokemonDraw : MonoBehaviour
{
    public float rotateSpeed;
    public float alphaSpeed;
    public GameObject screencover;
    public PokemonManager pm;
    public GameManager gm;
    public Text text;
    public AudioClip[] sfxClip;
    public enum sfx {PokeballDraw, Fail};
    private Image image;
    private Color color;
    private float maxBounceSize = 1.6f;
    private float minBounceSize = 1.5f;
    private float bounceSize = 1.5f;
    private bool downSizing = false;
    private bool draw = false;

    private void Awake() {
        image = screencover.GetComponent<Image>();
        color = image.color;
    }
    
    public void TouchDown()
    {        
        rotateSpeed -= 5;
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        if (rotateSpeed < -900){
            GetComponent<AudioSource>().clip = sfxClip[0];
            GetComponent<AudioSource>().Play();
            
            draw = true;            
            Transparency(true);
            Bounce();
        }
    }
    private void Bounce()
    {
        if (bounceSize < minBounceSize){
            downSizing = false;
        } else if (bounceSize > maxBounceSize){
            downSizing = true;
        }

        if (downSizing){
            bounceSize -= Time.deltaTime;
        } else {
            bounceSize += Time.deltaTime;
        }
        transform.localScale = Vector3.one * (bounceSize);
    }
    private void Transparency(bool isSet)
    {
        if(isSet){
            alphaSpeed += 0.02f;            
            color.a = alphaSpeed;
            image.color = color;
        } else {
            alphaSpeed = 0;
            color.a = 0;
            image.color = color;
        }
    }

    private void Update() 
    {
        text.text = "수량: " + pm.pokeballCnt.ToString();
        if(Input.anyKey || draw){
            if (pm.pokeballCnt > 0){
                TouchDown();           
            } else {
                //beep
                GetComponent<AudioSource>().clip = sfxClip[1];
                GetComponent<AudioSource>().Play();                
            }            
        } 
        if(draw && color.a > 1.5){
            pm.GetPokemon();
            Reset();
        }
        
    }    
    public void Reset()
    {
        //reset
        rotateSpeed = -10;
        transform.rotation = Quaternion.identity;
        Transparency(false);
        draw = false;
    }
}
