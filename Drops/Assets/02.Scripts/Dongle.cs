using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public GameManager manager;
    public ParticleSystem effect;
    public bool isDrag;
    public bool isMerge;
    public bool isAttach;
    Rigidbody2D rigid;
    CircleCollider2D circle;
    public int level;
    float deadTime;
    Animator anim;
    SpriteRenderer spriteRenderer;

    private void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();   
        circle = GetComponent<CircleCollider2D>();   
        anim = GetComponent<Animator>();   
        spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    private void OnEnable() 
    {
        anim.SetInteger("Level", level);
    }

    private void OnDisable() 
    {
        // init all values and settings
        level = 0;
        isDrag = false;
        isMerge = false;
        isAttach = false;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.zero;

        rigid.simulated = false;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0f;
        circle.enabled = true;        
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
    private void OnCollisionEnter2D(Collision2D other) 
    {
        StartCoroutine(AttachRoutine());        
    }

    IEnumerator AttachRoutine()
    {
        if (isAttach)
        {
            yield break;
        }
        isAttach = true;
        manager.SfxPlay(GameManager.sfx.Attach);
        yield return new WaitForSeconds(0.2f);
        isAttach = false;
    }

    // collision with other dongle
    private void OnCollisionStay2D(Collision2D collider) 
    {
        if(!manager.isGameOver) {   //do not merge after game over
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
    }
    // trigger with the finish line
    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Finish"){
            deadTime += Time.deltaTime;
            if (deadTime > 2){
                spriteRenderer.color = new Color(0.9f, 0.2f, 0.2f);
            }
            if (deadTime > 5){
                manager.GameOver();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Finish"){
            deadTime = 0;
            spriteRenderer.color = Color.white;
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
            if(target == Vector3.up * 100){                
                //manupulated value coming from game manager for the case of game over
                transform.localScale = Vector3.Lerp(transform.position, Vector3.zero, 0.2f);
                manager.SfxPlay(GameManager.sfx.LevelUP);
                PlayEffect();
            } else {
                transform.position = Vector3.Lerp(transform.position, target, 0.1f);                
            }            
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
        manager.score += (int)Mathf.Pow(2, level);
        isMerge = false;
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("Level", level+1);
        manager.SfxPlay(GameManager.sfx.LevelUP);
        yield return new WaitForSeconds(0.25f);        
        level++; // avoid to multiple level up
        manager.maxLevel = Mathf.Max(level, manager.maxLevel);

        if (level > 6){
            yield return new WaitForSeconds(1f);
            Hide(transform.position);
        }
        PlayEffect();
    }

    void PlayEffect()
    {
        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale;
        effect.Play();
    }
}
