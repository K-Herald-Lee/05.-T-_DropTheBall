using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonDisplay : MonoBehaviour
{
    private List<Pokemon> unlockedPokemons = new List<Pokemon>();
    public List<GameObject> gradePoolList;
    public GameObject image;
    public Sprite spriteNull;
    public int listCursor;
    
    private void OnEnable() 
    {
        for(int i=0; i<gradePoolList.Count; i++){ 
            for (int j=0; j<gradePoolList[i].GetComponent<PokemonSelector>().pool.Count; j++){
                Pokemon pokemon = gradePoolList[i].GetComponent<PokemonSelector>().pool[j];
                if (pokemon.unlock){
                    unlockedPokemons.Add(pokemon);
                }
            }
        }                
    }

    void LateUpdate()
    {
        if(unlockedPokemons.Count == 0){
            //image.GetComponent<Image>().sprite = spriteNull;
        } else {
            image.GetComponent<Image>().sprite = unlockedPokemons[listCursor].image;
        }
        
    }
}
