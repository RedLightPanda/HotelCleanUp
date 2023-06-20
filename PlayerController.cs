using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2D;

    public float moveSpeed;
    public float jumpForce;
    public bool clean = true;
    public bool wash = false;

    #region Attack    
    private float soapRate = .5f;
    private float canShoot = -1f;
    #endregion

    public Animator animate;
    public Transform firePoint,groundEye;
    public LayerMask isthereGround;
    private bool isGrounded;

    #region GameObject
    [SerializeField] private SoapBullet soapShot;
    [SerializeField] private SuperSpray superShot;
    [SerializeField] private BubbleShot bubbleShot;
    #endregion

    #region Power-Ups
    [SerializeField] private bool canSoapShot = false;
    [SerializeField] private bool canBubbleShot = false;
    #endregion

    #region Knock back numbers
    public bool enemyHit;
    public float kickBack;
    #endregion

    void Start()
    {
        //canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        animate.SetFloat("Speed",Mathf.Abs(rb2D.velocity.x));

        //Movement sideways
        rb2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb2D.velocity.y);

         //Flipping the Sprite
        if(rb2D.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb2D.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }

        //Soap Sprayer 
        if (Input.GetButtonDown("Fire1") && Time.time > canShoot && groundEye){
            Atks();
            if(canSoapShot == true){
                canSoapShot = false;
            }
            else if(canBubbleShot == true){
                canBubbleShot = false;
            }
        }
        isGrounded = Physics2D.OverlapCircle(groundEye.position, 2f, isthereGround);

        //Jump
        if(Input.GetButtonDown("Jump") && (groundEye)){
            if(isGrounded){
                rb2D.velocity = new Vector2(rb2D.velocity.x,jumpForce);
            }
        }

        if(enemyHit == true)
        {
            if(rb2D.velocity.x < 0)
            {
                rb2D.AddForce(transform.right * kickBack, ForceMode2D.Force);
                enemyHit = false;
            }
            else if (rb2D.velocity.x > 0)
            {
                 rb2D.AddForce(-transform.right * kickBack, ForceMode2D.Force);
                 enemyHit = false;
            }
        }
               
    }
    void Atks(){
            canShoot = Time.time + soapRate;
            animate.SetTrigger("isAtking");
            
            //Super Shot
            if(canSoapShot == true){
                Instantiate(superShot, firePoint.position, firePoint.rotation).movedir = new Vector2(transform.localScale.x, 0f);
            }
            else if (canBubbleShot == true)
            {
                Instantiate(bubbleShot, firePoint.position, firePoint.rotation).movedir = new Vector2(transform.localScale.x, 0f);
            }
            else
            {
                Instantiate(soapShot, firePoint.position, firePoint.rotation).movedir = new Vector2(transform.localScale.x, 0f);
            }
        }

    public void SuperSoap(){
        Debug.Log("Soap bottle picked up.");
        canSoapShot = true;
    }

    public void SuperBubble(){
        Debug.Log("Bar of Soap picked up");
        canBubbleShot = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy")
        {       
          enemyHit = true;  
        }
    }

}
