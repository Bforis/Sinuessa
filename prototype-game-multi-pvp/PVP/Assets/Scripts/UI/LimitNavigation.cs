using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

// Préviens les joueurs d'aller sur le panel des autres joueurs, dans le lobby ML
public class LimitNavigation : MonoBehaviour
{
    private MultiplayerEventSystem MES;
    [SerializeField]
    private int index;
    public GameObject BackTo;

    private void Start()
    {
        MES = this.gameObject.GetComponent<MultiplayerEventSystem>();
        for (var i = 0; i < 4; i++)
        {
            if (this.gameObject.name == "LobbyEventSystemP" + i)
            {
                index = i;
            }
        }
    }

    private void Update()
    {
        Limit();
    }

    private void Limit()
    {
        if (MES.currentSelectedGameObject != null)
        {
            if (MES.currentSelectedGameObject.name == "BtnChooseLinea(Clone)")
            {
             if (MES.currentSelectedGameObject.transform.parent.parent.parent.parent.name == ("LinPanelP" + index))
             {
                return;
            }
            else {
             MES.SetSelectedGameObject(BackTo);
         }
     }
 }
}
}
