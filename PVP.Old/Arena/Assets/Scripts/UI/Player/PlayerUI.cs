using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    private int index;

    private void Start()
    {
        index = ManagerScript.totalPlayerIndex;
        if (!MainMenu.ChoiceMenuStayActive)
        {
            gameObject.GetComponent<MultiplayerEventSystem>().firstSelectedGameObject = GameObject.Find("FirstMenu/Start");
        }
        else
        {
            switch (index)
            {
                case 1:
                    gameObject.GetComponent<MultiplayerEventSystem>().playerRoot = GameObject.Find("ChoiceMenu/P1");
                    gameObject.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(GameObject.Find("ChoiceMenu/P1/Button"));
                    break;
                case 2:
                    gameObject.GetComponent<MultiplayerEventSystem>().playerRoot = GameObject.Find("ChoiceMenu/P2");
                    gameObject.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(GameObject.Find("ChoiceMenu/P2/Button"));
                    break;
            }
        }
    }

    private void Update()
    {
        if (MainMenu.ChoiceMenuActive)
        {
            switch (index)
            {
                case 1:
                    gameObject.GetComponent<MultiplayerEventSystem>().playerRoot = GameObject.Find("ChoiceMenu/P1");
                    gameObject.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(GameObject.Find("ChoiceMenu/P1/Button"));
                    break;
                case 2:
                    gameObject.GetComponent<MultiplayerEventSystem>().playerRoot = GameObject.Find("ChoiceMenu/P2");
                    gameObject.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(GameObject.Find("ChoiceMenu/P2/Button"));
                    break;
            }
            MainMenu.ChoiceMenuActive = false;
        }
    }
}
