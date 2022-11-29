using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon 
{
    public string name;
    public Sprite image;
    public string grade;
    public bool unlock;

    public Pokemon(Pokemon p)
    {
        this.name = p.name;
        this.image = p.image;
        this.grade = p.grade;
        if(!PlayerPrefs.HasKey(this.name+"unlock")){
            this.unlock = true;
            PlayerPrefs.SetInt(this.name+"unlock",1);
        } else {
            if(PlayerPrefs.GetInt(this.name+"unlock") == 1){
                this.unlock = true;
            } else {
                this.unlock = false;
            }
        }
    }
}
