using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon 
{
    public string name;
    public Sprite image;
    public string grade;

    public Pokemon(Pokemon p)
    {
        this.name = p.name;
        this.image = p.image;
        this.grade = p.grade;
    }
}
