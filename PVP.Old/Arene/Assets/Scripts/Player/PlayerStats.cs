using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    // STATS
    public int playerIndex = 0;
    public int MaxHP = 100;
    public int HP;
    public int MANA;
    public bool dead = false;
    public bool stun = false;
    public bool GetHit = false;
    public bool GetHeal = false;
    private float speed;
    private bool RESET;
    private Color colorPlayer;

    // UI
    public Slider slider;
    public HealthBar healthBar;
    public GameObject HB2;
    public GameObject HB3;
    public GameObject HB4;

    Animator anim;
    private PlayerMovements movements;
    public BoxCollider2D collider1;
    public BoxCollider2D collider2;

    void Start(){
        playerIndex = GameControls.playerIndex;     
        HP = MaxHP;
        healthBar.SetMaxHealth(MaxHP);
        MANA = 100;
        PlayerMovements movements = GetComponent<PlayerMovements>();
        speed = movements.speed;
        colorPlayer = movements.colorPlayer;
        anim = GetComponent<Animator>();
    }

    void Update(){
        // BOOL STATES OF PLAYER
        if (GetHit){
            StartCoroutine(GetHitCo());
        }
        if (GetHeal){
            StartCoroutine(GetHealCo());
        }
        if (stun){
            StartCoroutine(GetStunCo());
        }
        //

        RESET = GameControls.RESET;
        RESTART();

        if (GameControls.scene.buildIndex == 0){
            HP = MaxHP;
        }
        else{
            healthBar.SetHealth(HP);
            if (HP > MaxHP){
                HP = MaxHP;
            }
        }

        if (HP <= 0 && dead == false){ // MORT
            StartCoroutine(Death());
        }
        
        if (playerIndex == 2){ // PLACEMENT DES HEALTHBARS POUR CHAQUE JOUEUR
            healthBar.transform.position = HB2.transform.position;
            slider.direction = Slider.Direction.RightToLeft;
        }
        if (playerIndex == 3){
            healthBar.transform.position = HB3.transform.position;
            slider.direction = Slider.Direction.RightToLeft;
        }
        if (playerIndex == 4){
            healthBar.transform.position = HB4.transform.position;
        }
    }

    IEnumerator GetHitCo(){ // HIT
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        GetHit = false;
        GetComponent<SpriteRenderer>().color = colorPlayer;
    }
    IEnumerator GetHealCo(){ // HEAL
        GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(0.2f);
        GetHeal = false;
        GetComponent<SpriteRenderer>().color = colorPlayer;
    }

    IEnumerator GetStunCo(){ // STUN
        speed = 1000000f;
        GetComponent<PlayerActions>().enabled = false;
        GetComponent<SpriteRenderer>().color = Color.blue;
        yield return new WaitForSeconds(1f);
        stun = false;
        speed = 12f;
        GetComponent<PlayerActions>().enabled = true;
        GetComponent<SpriteRenderer>().color = colorPlayer;
    }

    // IEnumerator getstun

    IEnumerator Death(){ // MORT
        collider1.size = new Vector3(0,0,0);
        collider2.size = new Vector3(0,0,0);
        GetComponent<SpriteRenderer>().color = Color.red;
        GetComponent<PlayerActions>().enabled = false;
        GetComponent<PlayerMovements>().enabled = false;
        speed = 1000000f;
        dead = true;
        yield return new WaitForSeconds(1);
        GameControls.alive -= 1;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void RESTART(){ // remet à zéro le joueur
        if (RESET == true){
            collider1.size = new Vector3(0.55f,1,0);
            collider2.size = new Vector3(0.58f,0.48f,0);
            dead = false;
            HP = MaxHP;
            speed = 12f;
            GetComponent<PlayerActions>().enabled = true;
            GetComponent<PlayerMovements>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<SpriteRenderer>().color = colorPlayer;
        }
    }
}
