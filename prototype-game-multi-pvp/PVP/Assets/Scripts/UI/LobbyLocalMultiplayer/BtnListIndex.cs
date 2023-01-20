using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnListIndex : MonoBehaviour
{
    public int index;
    public int indexPlayer;
    private string grandparent;

    public void Start(){
        grandparent = transform.parent.parent.parent.parent.name;

        for (var x = 1; x < 4; x++){
            if (grandparent == "LinPanelP" + x){
            indexPlayer = x;
        }
        }
    }  
}