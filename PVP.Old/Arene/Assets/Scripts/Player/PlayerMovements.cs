using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovements : MonoBehaviour { // PLAYERS MOVEMENTS AND ANIMATIONS + PHYSICS COLLIDER

public float speed;
Rigidbody2D rb2d;
public bool MoveAir = true;
public bool OnWall = false;
public bool facingRight;
public GameObject Player;
public GameObject ResetPosition;
Animator anim;
Collider2D col;
public SpriteRenderer sprite;
public Color colorPlayer;
private Spells spellScript;

// NEW INPUT MANAGER SYSTEM OF SHIIIITTTT
public Vector2 movement;

	public void Start () {
        col = GetComponent<Collider2D>();
		rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = Player.GetComponent<SpriteRenderer>();
        speed = 12f;
        spellScript = GetComponent<Spells>();
	}

    void MortPlayer()
    {
        Destroy(Player);
    }

	void FixedUpdate () {
         //Init(anim); Animation Ovverride pour change les animations avec ruban
		//float Move = Input.GetAxisRaw("Horizontal");
        //float MoveV = Input.GetAxisRaw("Vertical");
        // NEW HORRIBLE SYSTEM OF MOVEMENTS
        transform.Translate(movement / speed);
	}

    void Update(){
        Animation();
    }

	 /*public void Move()
    {
        // OLD BEAUTIFUL SYSTEM OF MOVEMENTS
        // JOUEUR HORIZONTAL : S & F et spécial :  D -- JOUEUR VERTICAL : K & O et spécial : M ; JOUEUR SOLO : FLECHE DIRECTIONNELLE.
        // Ne pas s'accrocher au mur
        if (!MoveAir && OnWall == true)
            return;
        //
        Vector2 moveVel = rb2d.velocity;
        moveVel.x = horizontalInput * speed;
        moveVel.y = verticalInput * speed * speedDouble;
        rb2d.velocity = moveVel;
    }*/

    public void OnMove(InputValue value){
        movement = value.Get<Vector2>();
    }

    public void ChangeColor(){
        sprite.color = new Color(Random.Range(0F,1F), Random.Range(0, 1F), Random.Range(0, 1F));
        colorPlayer = sprite.color;
    }

	public void Animation()
	{
		if (movement.x > 0.2f || movement.x < -0.2f)
        {
           // transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0f, 0f));
            if (movement.x > -0.2f && facingRight && speed > 0)
            {
              Flip();
               facingRight = false;
            }
            else if (movement.x < 0.2f && !facingRight && speed > 0)
            {
               Flip();
                facingRight = true;
            }
	}
    float Move = movement.x;
    float MoveV = movement.y;
    if ((MoveV != 0 || Move != 0) && spellScript.casting == false)
    {
        anim.SetBool("RunDroite", true);
        anim.SetBool("IdleDroite", false);
        anim.SetBool("Casting", false);
    }
    if ((MoveV == 0 && Move == 0) && spellScript.casting == false){
        anim.SetBool("IdleDroite", true);
        anim.SetBool("RunDroite", false);
    }

	} // Fin animation

	 void Flip()
    {
       facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *=-1;
        transform.localScale = theScale;
    }
    
	// COLLISION WITH EVENT !!!
    private void OnTriggerEnter2D(Collider2D collider)
    {

    }

    private void OnTriggerStay2D(Collider2D collider) { // COLLIDER NPC INTERACTIONS
        if (collider.gameObject.tag == "NPC"){

        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // COLLISION
    {
        if (collision.gameObject.tag == "Wall")
        {
            OnWall = true;
        }
       
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            OnWall = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            OnWall = false;
        }
    }

}