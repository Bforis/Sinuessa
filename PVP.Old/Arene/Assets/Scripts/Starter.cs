using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    public GameObject GameManager;
    public bool ManagerIsHere;

    void Update(){
        var gamemanager = GameObject.Find("GameManager(Clone)");
        if (gamemanager == null){
            ManagerIsHere = false;
        }
        else{
            ManagerIsHere = true;
        }

        if (ManagerIsHere == false){
        var position = new Vector2(-9.55f, 1.30f);
        Instantiate(GameManager, position, Quaternion.identity);
        ManagerIsHere = true;
        }
    }
}
