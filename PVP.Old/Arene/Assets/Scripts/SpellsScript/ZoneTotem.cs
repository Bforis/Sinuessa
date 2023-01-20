using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTotem : MonoBehaviour
{

    public int playerIndex;
    public GameObject totem;
    private bool contactMaster = false;
    PlayerStats playerInZone;
    private float CD;
    // Start is called before the first frame update
    void Start()
    {
        playerIndex = totem.GetComponent<totemDef>().playerIndex;
    }

    void Update(){
        if (playerInZone){
            Healing(playerInZone.HP, playerInZone, 2f, 5);
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.tag == "Player"){
            playerInZone = col.gameObject.GetComponent<PlayerStats>();
            if (playerInZone.playerIndex == playerIndex && !contactMaster){
                contactMaster = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
         if (col.gameObject.tag == "Player"){
            playerInZone = col.gameObject.GetComponent<PlayerStats>();
            if (playerInZone.playerIndex == playerIndex && contactMaster){
                contactMaster = false;
            }
        }
    }

    void Healing(int HP, PlayerStats player, float delay, int healer){
        if (contactMaster == true && Time.time > CD){
        CD = Time.time + delay;
        player.GetHeal = true;
        player.HP += healer;
        }
    }
}
