using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PVP {
    public class UIManager : MonoBehaviour
    {
        public GameManagerHandler GM;

        private void Update(){
            ActiveAndAttributeEventSystem();
        }

        #region UIPlayerInML
        // ATTRIBUE LES DIFFERENTS INPUTSYSTEMUIMODULE À CHAQUES JOUEURS
        public void ActiveAndAttributeEventSystem(){
            #region UILocalMultiplayer
            if (GM.gameManager.GameState == PVP.GameStates.LobbyLocalMultiplayer && GM.gameManager.Multiplayer){
                if (GameObject.Find("Player1")){
                    if (GameObject.Find("LobbyEventSystemP1")){
                        GameObject.Find("Player1").GetComponent<Player>().playerInput.uiInputModule = GameObject.Find("LobbyEventSystemP1").GetComponent<InputSystemUIInputModule>();
                    }
                }
                if (GameObject.Find("Player2") && !GameObject.Find("Canvas/MenuMultiplayer/LobbyEventSystemP2").GetComponent<MultiplayerEventSystem>().enabled){
                    GameObject.Find("Canvas/MenuMultiplayer/LobbyEventSystemP2").GetComponent<MultiplayerEventSystem>().enabled = true;
                    GameObject.Find("Player2").GetComponent<Player>().playerInput.uiInputModule = GameObject.Find("LobbyEventSystemP2").GetComponent<InputSystemUIInputModule>();
                    GameObject.Find("LobbyEventSystemP2").GetComponent<LimitNavigation>().enabled = true;
                    GameObject.Find("ScrollView2").GetComponent<ScrollRectMovement>().enabled = true;
                    // Active ScrellRectMovement.cs Script for active scroll rect, and desactive for prevent bug not find MES.current object. 
                    if (GM.gameManager.GameState == GameStates.LobbyLocalMultiplayer)
                    {
                        GameObject.Find("ScrollView2").GetComponent<ScrollRectMovement>().enabled = true;
                    }
                }
                if (GameObject.Find("Player3") && !GameObject.Find("Canvas/MenuMultiplayer/LobbyEventSystemP3").GetComponent<MultiplayerEventSystem>().enabled){
                    GameObject.Find("Canvas/MenuMultiplayer/LobbyEventSystemP3").GetComponent<MultiplayerEventSystem>().enabled = true;
                    GameObject.Find("Player3").GetComponent<Player>().playerInput.uiInputModule = GameObject.Find("LobbyEventSystemP3").GetComponent<InputSystemUIInputModule>();
                    GameObject.Find("LobbyEventSystemP3").GetComponent<LimitNavigation>().enabled = true;
                    GameObject.Find("ScrollView3").GetComponent<ScrollRectMovement>().enabled = true;
                    // Active ScrellRectMovement.cs Script for active scroll rect, and desactive for prevent bug not find MES.current object. 
                    if (GM.gameManager.GameState == GameStates.LobbyLocalMultiplayer)
                    {
                        GameObject.Find("ScrollView3").GetComponent<ScrollRectMovement>().enabled = true;
                    }
                }
                if (GameObject.Find("Player4") && !GameObject.Find("Canvas/MenuMultiplayer/LobbyEventSystemP4").GetComponent<MultiplayerEventSystem>().enabled){
                    GameObject.Find("Canvas/MenuMultiplayer/LobbyEventSystemP4").GetComponent<MultiplayerEventSystem>().enabled = true;
                    GameObject.Find("Player4").GetComponent<Player>().playerInput.uiInputModule = GameObject.Find("LobbyEventSystemP4").GetComponent<InputSystemUIInputModule>();
                    GameObject.Find("LobbyEventSystemP4").GetComponent<LimitNavigation>().enabled = true;
                    GameObject.Find("ScrollView4").GetComponent<ScrollRectMovement>().enabled = true;
                    // Active ScrellRectMovement.cs Script for active scroll rect, and desactive for prevent bug not find MES.current object. 
                    if (GM.gameManager.GameState == GameStates.LobbyLocalMultiplayer)
                    {
                        GameObject.Find("ScrollView4").GetComponent<ScrollRectMovement>().enabled = true;
                    }
                }
            }
        }

        public void ReturnSelectPanel() // bug go.isactive()
        {
            if (GameObject.Find("Player1"))
            {
                GameObject.Find("Canvas/MenuMultiplayer/LobbyEventSystemP1").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(GameObject.Find("LinPanelP1").transform.GetChild(0).GetChild(0).gameObject);
            }
            if (GameObject.Find("Player2"))
            {
                GameObject.Find("Canvas/MenuMultiplayer/LobbyEventSystemP2").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(GameObject.Find("LinPanelP2").transform.GetChild(0).GetChild(0).gameObject);
            }
            if (GameObject.Find("Player3")) 
            {
                GameObject.Find("Canvas/MenuMultiplayer/LobbyEventSystemP3").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(GameObject.Find("LinPanelP3").transform.GetChild(0).GetChild(0).gameObject);
            }
            if (GameObject.Find("Player4"))
            {
                GameObject.Find("Canvas/MenuMultiplayer/LobbyEventSystemP4").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(GameObject.Find("LinPanelP4").transform.GetChild(0).GetChild(0).gameObject);
            }
        }
        #endregion
    #endregion

} // class UIManager
} // namespace PVP