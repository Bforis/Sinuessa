using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class boule : MonoBehaviour
{
    public Animator anim;
    [SerializeField]
    public int playerIndex; // GET ID OF CASTER
    [Header("Stats")]
    public int damage = 10;
    public int mana = 5;
    private bool touch = false;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player" && touch == false){
            PlayerStats enemy = col.gameObject.GetComponent<PlayerStats>();
            if (enemy.playerIndex != playerIndex){
                enemy.HP -= damage;
                enemy.GetHit = true;
                anim.SetBool("Fire", false);
                anim.SetBool("Hit", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(transform.position.x, transform.position.y) * 0;
                touch = true;
                StartCoroutine(Destroy());
            }
        }
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Spell"){
            anim.SetBool("Fire", false);
            anim.SetBool("Hit", true);
            GetComponent<Rigidbody2D>().velocity = new Vector2(transform.position.x, transform.position.y) * 0;
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy(){
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
