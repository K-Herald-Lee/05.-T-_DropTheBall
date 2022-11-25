using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public bool isDrag;
    Rigidbody2D rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDrag) {
            float rad = transform.localScale.x / 2f;
            float leftBorder = -4.15f + rad;
            float rightBorder = 4.15f - rad;  
            
            //get mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //set x 
            if (mousePos.x < leftBorder){
                mousePos.x = leftBorder;
            } 
            else if (mousePos.x > rightBorder){
                mousePos.x = rightBorder;
            }
            // set y, z
            mousePos.y = 8;
            mousePos.z = 0;

            //set dongle's position
            transform.position = Vector3.Lerp(transform.position, mousePos, 0.2f);
        }        
    }
    
    public void Drag()
    {
        isDrag = true;
        rigid.simulated = false;
    }

    public void Drop()
    {
        isDrag = false;
        rigid.simulated = true;
    }
}
