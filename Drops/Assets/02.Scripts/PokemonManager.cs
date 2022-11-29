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
    public int pokeballCnt;
    
    private void Awake() 
    {
        //Pokeball init
        if(!PlayerPrefs.HasKey("pokeballCnt")){
            PlayerPrefs.SetInt("pokeballCnt", 0);
        }else{
            pokeballCnt = PlayerPrefs.GetInt("pokeballCnt");
        }

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
        
        if (pokeballCnt > 0){
            pokeballCnt--;
            gradePool = setPool();
            Debug.Log(gradePool.gameObject.name);
            Pokemon pokemon = gradePool.GetComponent<PokemonSelector>().PickPokemon();

            imagePokemon.GetComponent<Image>().sprite = pokemon.image;
            pokemon.unlock = true;
            PlayerPrefs.SetInt(pokemon.name+"unlock",1);

            PlayerPrefs.SetInt("pokeballCnt",pokeballCnt);
        }
        
    }
    public GameObject setPool()
    {
        int i;
        float cursor = Random.Range(0.01f,1.00f);

        if(cursor < 0.40f){
            i = 0;
        } else if (cursor < 0.60f){
            i = 1;
        } else if (cursor < 0.75f){
            i = 2;
        } else if (cursor < 0.85f){
            i = 3;
        } else if (cursor < 0.92f){
            i = 4;
        } else if (cursor < 0.97f){
            i = 5;
        } else {
            i = 6;
        }       
        return gradePoolList[i];
    }    
}
