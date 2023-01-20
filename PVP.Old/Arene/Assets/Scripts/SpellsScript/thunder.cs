using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class thunder : MonoBehaviour
{
    private Vector2 movement;
    [SerializeField]
    public int playerIndex; // GET ID OF CASTER
    [Header("Stats")]
    public int damage = 10;

    void Update(){
        StartCoroutine(destroyThunder());
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player"){
            PlayerStats enemy = col.gameObject.GetComponent<PlayerStats>();
                if (enemy.playerIndex != playerIndex){
                    enemy.HP -= damage;
                    enemy.GetHit = true;
                }
        }   
    }

    IEnumerator destroyThunder(){
        yield return new WaitForSeconds(0.4f);
            Destroy(this.gameObject);
    }
}
