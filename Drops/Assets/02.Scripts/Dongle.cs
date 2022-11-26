using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public GameManager manager;
    public ParticleSystem effect;
    public bool isDrag;
    public bool isMerge;
    Rigidbody2D rigid;
    CircleCollider2D circle;
    public int level;
    Animator anim;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();   
        circle = GetComponent<CircleCollider2D>();   
        anim = GetComponent<Animator>();   
    }

    private void OnEnable() {
        anim.SetInteger("Level", level);
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

    private void OnCollisionStay2D(Collision2D collider) 
    {
        if(collider.gameObject.tag == "Dongle"){
            Dongle other = collider.gameObject.GetComponent<Dongle>();

            //dongle merge logic
            if(level == other.level && 
            !isMerge && 
            !other.isMerge && 
            level < 7 && 
            other.level < 7){
                //Get x,y position
                float myPositionX = transform.position.x;
                float myPositionY = transform.position.y;
                float otherPositionX = other.transform.position.x;
                float otherPositionY = other.transform.position.y;

                if (myPositionY >= otherPositionY ||
                (myPositionY >= otherPositionY && myPositionX > otherPositionX)){
                    //hide opponent
                    other.Hide(transform.position);
                    
                    //level up myself
                    LevelUp();
                }

            }
        } 
    }
    public void Hide(Vector3 target)
    {
        isMerge = true;
        rigid.simulated = false;
        circle.enabled = false;

        StartCoroutine(HideRoutine(target));
        isMerge = false;
    }

    IEnumerator HideRoutine(Vector3 target)
    {
        int frameCnt = 0;
        while(frameCnt < 20){
            frameCnt++;
            transform.position = Vector3.Lerp(transform.position, target, 0.1f);
            yield return null;
        }        
        gameObject.SetActive(false);
    }

    void LevelUp()
    {
        isMerge = true;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0f;

        StartCoroutine(LevelUpRoutine());
        isMerge = false;
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("Level", level+1);
        PlayEffect();
        yield return new WaitForSeconds(0.25f);        
        level++; // avoid to multiple level up
        manager.maxLevel = Mathf.Max(level, manager.maxLevel);
    }

    void PlayEffect()
    {
        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale;
        effect.Play();
    }
}
