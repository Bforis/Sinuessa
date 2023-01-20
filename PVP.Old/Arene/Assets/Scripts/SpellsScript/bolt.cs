using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bolt : MonoBehaviour
{
private Vector2 movement;
    [SerializeField]
    public int playerIndex; // GET ID OF CASTER
    [Header("Stats")]
    public int damage = 25;
    private bool touch = false;

    void Update(){
        StartCoroutine(destroyBolt());
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player" && touch == false){
            PlayerStats enemy = col.gameObject.GetComponent<PlayerStats>();
                if (enemy.playerIndex != playerIndex){
                    enemy.HP -= damage;
                    enemy.GetHit = true;
                    enemy.stun = true;
                    touch = true;
                }
        }   
    }

    IEnumerator destroyBolt(){
        yield return new WaitForSeconds(0.8f);
        Destroy(this.gameObject);
    }
}
