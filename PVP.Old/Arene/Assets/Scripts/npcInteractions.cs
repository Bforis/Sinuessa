using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcInteractions : MonoBehaviour
{

    public GameObject canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collider){
        if (collider.gameObject.tag == "Player"){
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        canvas.SetActive(false);
    }
}
