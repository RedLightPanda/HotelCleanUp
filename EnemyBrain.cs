using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public Transform[] patrolPath;
    public Rigidbody2D enemyRB;
    public WindowsHealth windowHP;

    [SerializeField]private GameObject patrolHold;
    [SerializeField]private int currentPoint, windowPoint;
    private float waitCounter, stunCounter, bubbleCounter;
    public float Dirtycount;
    public float moveSpeed, waitPoint;
    public bool slipped,Bubbled,canDirty;
    public int beforeDirt;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitPoint;
        stunCounter = waitPoint;
        Dirtycount = 5;
        
        foreach (Transform pPoint in patrolPath){
            patrolHold.transform.SetParent(null);
        }
 
    } 

    // Update is called once per frame
    void Update()
    {

        #region Patrol
        if(Mathf.Abs(transform.position.x - patrolPath[currentPoint].position.x) > .2f)
        {
            if (transform.position.x < patrolPath[currentPoint].position.x)
            {
                enemyRB.velocity = new Vector2(moveSpeed, enemyRB.velocity.y);
                transform.localScale = Vector3.one;
            }
            else
            {
                enemyRB.velocity = new Vector2(-moveSpeed, enemyRB.velocity.y);
                transform.localScale = new Vector3(-1f,1f,1f);
            }
        }
        else 
        {
            enemyRB.velocity = new Vector2(0f, enemyRB.velocity.y);

            waitCounter -= Time.deltaTime;
            
            if(waitCounter <= 0)
            {
                waitCounter = waitPoint;

                currentPoint++;

                if(currentPoint >= 2){
                    currentPoint = 0;
                    beforeDirt--;
                }
            }
        }
        #endregion

        #region Dirty window
        if(beforeDirt <= 0){
            Debug.Log("beforedirt hit 0");
            canDirty = true;
        }

        if(canDirty == true){
            Dirtycount -= Time.deltaTime;
        }

        if(Dirtycount <= 0 ){
            windowPoint = Random.Range(2, 5);
            if(windowPoint == 5){
                windowPoint = Random.Range(2,5);
            }
            currentPoint = windowPoint;
            Dirtycount = 5;
            canDirty = false;
            beforeDirt = 5;
        }
        #endregion

        #region Hit by player
        if(slipped == true && stunCounter >= 0){
            enemyRB.velocity = new Vector2(moveSpeed * 0 , enemyRB.velocity.y);
            stunCounter -= Time.deltaTime;
            
        }
        else if(stunCounter <= 0)
        {
            slipped = false;
            
            if(currentPoint == 1){
                enemyRB.velocity = new Vector2(-moveSpeed, enemyRB.velocity.y); 
            }else if (currentPoint == 0){
                enemyRB.velocity = new Vector2(moveSpeed, enemyRB.velocity.y); 
            }
            
        }

        if(Bubbled == true && bubbleCounter >= 0){
            enemyRB.velocity = new Vector2(moveSpeed * 0, enemyRB.velocity.y);
            bubbleCounter -= Time.deltaTime;
        } else if (stunCounter <= 0){
            Bubbled = false;
            
            if(currentPoint == 1){
                enemyRB.velocity = new Vector2(-moveSpeed, enemyRB.velocity.y); 
            }else if (currentPoint == 0){
                enemyRB.velocity = new Vector2(moveSpeed, enemyRB.velocity.y); 
            }
        }

        if(slipped == false && stunCounter <= 0){
            stunCounter = waitCounter;
        }

        if(Bubbled == false && bubbleCounter <= 0){
            bubbleCounter = 2f;
        }
        #endregion

        Physics2D.IgnoreLayerCollision(6,6);
        Physics2D.IgnoreLayerCollision(6,7);
        
    }

    private void OnTriggerEnter2D (Collider2D other){
        if(other.tag == "Soap" || other.tag == "Bubble"){
           slipped = true;
        }

        if(other.tag == "Bubble"){
            Bubbled = true;
        }

        if(other.tag == "SuperSoap"){
            Destroy(gameObject);
            Destroy(patrolHold);
        }

        if(other.tag == "Player" && Bubbled == true){
            Destroy(gameObject);
            Destroy(patrolHold);
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if(other.tag == "Window" && canDirty == true){
            if(windowHP. windowHealth == 1){
                windowHP. windowHealth--;
            }
            else if(windowHP.windowHealth == 0)
            {
                windowPoint = Random.Range(2,5);
            }
        }
    }


}
