using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonManager : MonoBehaviour
{
    public enum type {Common, Uncommon, Rare};
    public List<GameObject> gradePoolList;
    public GameObject gradePool;
    public GameObject imagePokemon;
    
    private void Awake() 
    {
        for(int i=0; i<gradePoolList.Count; i++){ 
            for (int j=0; j<gradePoolList[i].GetComponent<PokemonSelector>().pool.Count; j++){
                Pokemon pokemon = gradePoolList[i].GetComponent<PokemonSelector>().pool[j];
                if (PlayerPrefs.HasKey(pokemon.name+"unlock")){
                    if (PlayerPrefs.GetInt(pokemon.name+"unlock")==1){
                        pokemon.unlock = true;
                    } else {
                        pokemon.unlock = false;
                    }
                } else {
                    pokemon.unlock = false;
                    PlayerPrefs.SetInt(pokemon.name+"unlock", 0);
                }
            }
        }        
    }
    public void GetPokemon()
    {
        //int ball = PlayerPrefs.GetInt("pokeballCnt");
        //if (ball > 0){
        //    ball--;
            gradePool = setPool();
            Debug.Log(gradePool.gameObject.name);
            Pokemon pokemon = gradePool.GetComponent<PokemonSelector>().PickPokemon();

            imagePokemon.GetComponent<Image>().sprite = pokemon.image;
            pokemon.unlock = true;
            PlayerPrefs.SetInt(pokemon.name+"unlock",1);

        //    PlayerPrefs.SetInt("pokeballCnt",ball);
        //}
        
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
