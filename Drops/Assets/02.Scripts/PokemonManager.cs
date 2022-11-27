using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonManager : MonoBehaviour
{
    public enum type {Common, Uncommon, Rare};
    public List<GameObject> gradePoolList;
    public GameObject gradePool;
    
    public void GetPokemon()
    {
        int ball = PlayerPrefs.GetInt("pokeballCnt");
        if (ball > 0){
            ball--;
            gradePool = setPool();
            Debug.Log(gradePool.gameObject.name);
            gradePool.GetComponent<PokemonSelector>().PickPokemon();
            PlayerPrefs.SetInt("pokeballCnt",ball);
        }
        
    }
    public GameObject setPool()
    {
        int i;
        float cursor = Random.Range(0.01f,1.00f);

        if(cursor < 0.6f){
            i = 0;
        } else if (cursor < 0.9f){
            i = 1;
        } else {
            i = 2;            
        }
        
        return gradePoolList[i];
    }
    
}
