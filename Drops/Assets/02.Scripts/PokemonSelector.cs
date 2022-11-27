using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonSelector : MonoBehaviour
{
    public List<Pokemon> pool = new List<Pokemon>();

    public void PickPokemon()
    {
        Pokemon tmp = pool[Random.Range(0,pool.Count)];
        Debug.Log("Catch " + tmp.name);
    }
}
