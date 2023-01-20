using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornade : MonoBehaviour
{
    [SerializeField]
    public int playerIndex; // GET ID OF CASTER
    [Header("Stats")]
    public int damage = 20;
    public int mana = 5;
    private bool touch = false;

    void Start(){
        StartCoroutine(Destroy());
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player" && touch == false){
            PlayerStats enemy = col.gameObject.GetComponent<PlayerStats>();
            if (enemy.playerIndex != playerIndex){
                enemy.HP -= damage;
                enemy.GetHit = true;
                touch = true;
            }
        }
    }

    IEnumerator Destroy(){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
