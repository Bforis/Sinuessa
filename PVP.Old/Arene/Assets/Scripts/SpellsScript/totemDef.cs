using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class totemDef : MonoBehaviour
{
    public int playerIndex;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyTotem());
    }

    IEnumerator DestroyTotem(){
        yield return new WaitForSeconds(15f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Spell"){
            boule boules = col.gameObject.GetComponent<boule>();
            thunder thunders = col.gameObject.GetComponent<thunder>();
            tornade tornades = col.gameObject.GetComponent<tornade>();
            if (boules){
                if (boules.playerIndex != playerIndex){
                Destroy(this.gameObject);
            }
            }
            if (thunders){
                if (thunders.playerIndex != playerIndex){
                Destroy(this.gameObject);
            }
            if (tornades){
                if (tornades.playerIndex != playerIndex){
                    Destroy(this.gameObject);
                }
            }
            }
        }
    }
}
