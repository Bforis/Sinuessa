using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PVP{ 
// Gestion des changements de scenes et des changements dûes à cela. 
    public class LevelManager : MonoBehaviour
    {
        public GameManagerHandler GM;
        private UIManager uiManager;

        private void FixedUpdate()
        {
            LaunchML(); // Countdown in ML
        }

    #region Utilities
        private void Start(){
            GM = gameObject.GetComponent<PVP.GameManagerHandler>();
            uiManager = gameObject.GetComponent<PVP.UIManager>();
        }

        public void LoadScene(string sceneName){
            SceneManager.LoadScene(sceneName);
        }

        public void Quit(){
            Debug.Log("QUIT");
        }
    #endregion

    #region BeforePlay
        public void MenuMain () { // Load First Menu 
            Debug.Log("Start Scene : FirstMenu | GameState = FirstMenu");
            SceneManager.LoadScene("FirstMenu", LoadSceneMode.Single);
            Debug.Log("GameManager: Changing join behavior menu main = JoinPlayersManually");
            GM.playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            GM.gameManager.GameState = GameStates.FirstMenu;
            GM.gameManager.StatePlayers(PlayerStates.InFirstMenu);
            GM.gameManager.Multiplayer = false;
            GM.gameManager.ResetPlayers();
        }

        public void LobbySolo () { // Load Lobby Solo Scene Menu
            Debug.Log("Start Scene : LobbySolo | GameState = LobbySingleplayer");
            GM.playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            SceneManager.LoadScene("LobbySolo", LoadSceneMode.Single);
            GM.gameManager.GameState = GameStates.LobbySingleplayer;
            GM.gameManager.StatePlayers(PlayerStates.InLobbySingleplayer);
        }

        public void LobbyLocalMultiplayer () { // Load Lobby Multiplayer Scene Menu
            Debug.Log("Start Scene : LobbyMultiplayer | GameState = LobbyLocalMultiplayer");
            SceneManager.LoadScene("LobbyMultiplayer", LoadSceneMode.Single);
            Debug.Log("GameManager: Changing join behavior menu local multiplayer");
            GM.playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            GM.gameManager.GameState = GameStates.LobbyLocalMultiplayer;
            GM.gameManager.StatePlayers(PlayerStates.InLobbyLocalMultiplayer);
            GM.gameManager.Multiplayer = true;
        }
    #endregion

        public void Launch()
        {
            Debug.Log("LAUNCH GAME");
            SceneManager.LoadScene("LobbyPlaying", LoadSceneMode.Single);
            GM.gameManager.GameState = GameStates.Playing;
            GM.gameManager.StatePlayers(PlayerStates.Playing);
            GM.playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        }

        #region CountdownInML
        public void LaunchML()
        {
            if (GM.gameManager.GameState == GameStates.LobbyLocalMultiplayer)
            {
                if (GM.gameManager.AllPlayersReady && !GM.gameManager.Launch)
                {
                   StartCoroutine("LobbyCountdown");
               }
               if (!GM.gameManager.AllPlayersReady && GM.gameManager.Launch)
               {
                StopCoroutine("LobbyCountdown");
                GM.gameManager.Launch = false;
                GameObject.Find("CountdownTimer").GetComponent<Text>().text = "";
               }
            }
        }

        public IEnumerator LobbyCountdown()
        {
            if (GM.gameManager.AllPlayersReady && !GM.gameManager.Launch && GM.gameManager.GameState == GameStates.LobbyLocalMultiplayer)
            {
                GM.gameManager.Launch = true;
                int countdownDuration = 5;
                int remainingTime = countdownDuration;                
                for(int i = 0; i < countdownDuration; i++)
                {
                    GameObject.Find("CountdownTimer").GetComponent<Text>().text = "Start in " + remainingTime.ToString();
                    yield return new WaitForSeconds(1);
                    remainingTime--;
                }
                Launch();
            }
        }
        #endregion
    }
}