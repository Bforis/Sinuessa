using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;
using PVP.Events;
using PVP;
using UnityEngine.SceneManagement;

namespace PVP {
  [CreateAssetMenu(menuName = "GameManager")]
  public class GameManager : ScriptableObject
  {
    // ML = Multiplayer Local
    // SP = Solo Player

    // VARIABLES
    public int TimeUntilGameStarts = 5;
    public int playerIndex = 0;
    public int playerReady = 0;

    // BOOL
    public bool GameIsStarted = false;
    public bool GameIsPaused = false;
    public bool GameIsInMenu = false;
    public bool Multiplayer = false;
    public bool AllPlayersReady = false;
    public bool Launch = false;
    // public bool DevMode = false;

    // DATA
    public static List<PVP.DataPlayer.Linea> Lineas = new List<PVP.DataPlayer.Linea>(); // Lignées globales
    public List<GameObject> Players = new List<GameObject>();
    public string path;
    private PVP.DataPlayer.Linea LineaToDelete;

    // GAME OBJECT
    public GameObject Player;

    // EVENTS
    public PlayerEvents OnPlayerJoined;
    public PlayerEvents OnPlayerLeft;
    public UnityEvent OnEnablePlayerJoin;
    public UnityEvent OnDisablePlayerJoin;
    public PlayerEvents OnPlayersReady;
    public PlayerEvents OnPlayersUnready;
    public PlayerEvents OnGamePaused;
    public PlayerEvents OnGameUnpaused;

    // STATES
    public GameStates GameState;
    public PlayerStates PlayerState;
    
    #region Begin
    public void Initialization () {
      GameIsStarted = false;
      GameIsPaused = false;
      GameIsInMenu = false;

      Time.timeScale = 1f;
      playerIndex = 0;
      playerReady = 0;

      path = Application.streamingAssetsPath + "/" + "data.json";
      Lineas = JsonHelper.ReadFromJsonFile<List<PVP.DataPlayer.Linea>>(path);
      Debug.Log("Nombre de lignées actuelles : " + Lineas.Count);
      for (var i = 0; i < Lineas.Count; i++){
        Debug.Log(Lineas[i].name + " - " + Lineas[i].LineaID);
      }
      Players.Clear();
      GameStart();
    }

    public void GameStart(){
      SceneManager.LoadScene("FirstMenu", LoadSceneMode.Single);
      GameState = GameStates.FirstMenu;
      Debug.Log(GameState);
      Debug.Log("Game Start");
      GameIsStarted = true;
    }
    #endregion

    public void StatePlayers(PlayerStates playerState){
      for (var i = 0; i < Players.Count; i++){
        Players[i].GetComponent<Player>().PlayerState = playerState;
      }
      Debug.Log("Changement d'Etats des joueurs : " + playerState);
    }
    
    // Destroy Players Prefab, and reset all links to them
    public void ResetPlayers(){
      foreach (GameObject player in Players){
        GameObject.Destroy(player);
      }
      Players.Clear();
      playerIndex = 0;
      playerReady = 0;
      // reset lineas Actual Player ID 
      for (var i = 0; i < Lineas.Count; i++)
      {
        Lineas[i].ActualPlayerID = 0;
      }
    }

    #region ML

    public void AddPlayer(GameObject player){
      Debug.Log("Player: Joined");
      playerIndex++;
      Player _player = player.GetComponent<Player>();
      player.GetComponent<Player>()._player.ActualPlayerID = playerIndex;
      Players.Add(player);
      OnPlayerJoined.Invoke(_player);
      player.name = "Player" + playerIndex;
      AllPlayersReady = false;
    }

    public void AttributeLineaToPlayer(Button btn){ // Execute in prefab BtnChooseLinea
      int id = btn.GetComponent<BtnListIndex>().index;
      int idPlayer = btn.GetComponent<BtnListIndex>().indexPlayer; 
        // Debug.Log("index = " + id + " & indexPlayer = " + idPlayer);

        // Prend les informations de la linea du joueur actuel ciblé
      Player actualPlayer = Players[idPlayer - 1].GetComponent<Player>();
      PVP.DataPlayer.Linea PlayerLinea = Players[idPlayer - 1].GetComponent<Player>()._player;

        // Attribute everything of linea => Player linea
      for (var i = 0; i < Lineas.Count; i++){
        if (id == Lineas[i].LineaID){
          // SELECT
          if (Lineas[i].ActualPlayerID == 0)
          {
            ResetActualPlayerID(idPlayer);
            PlayerLinea.LineaID = Lineas[i].LineaID;
            PlayerLinea.name = Lineas[i].name;
            PlayerLinea.desc = Lineas[i].desc;
            Lineas[i].ActualPlayerID = PlayerLinea.ActualPlayerID;
            actualPlayer.HasLinea = true;

            Debug.Log("Player " + idPlayer + " choose Linea : " + PlayerLinea.name + " & Linea ID : "+PlayerLinea.LineaID);
            // show choosed linea
            GameObject.Find("P"+idPlayer+"Text").GetComponent<Text>().text = "P"+idPlayer+": " + PlayerLinea.name;
            TurnGreenBtn(btn);
          }
          // DESELECT
          else if (PlayerLinea.LineaID == Lineas[i].LineaID)
          {
            Debug.Log("Deselect : " + Lineas[i].name);
            PlayerLinea.LineaID = 0;
            PlayerLinea.name = null;
            PlayerLinea.desc = null;
            Lineas[i].ActualPlayerID = 0;
            GameObject.Find("P"+idPlayer+"Text").GetComponent<Text>().text = "P"+idPlayer+": ";
            actualPlayer.Ready = false;
            AllPlayersReady = false;
            TurnGreyBtn(btn);
            actualPlayer.HasLinea = false;
          }
          else
          {
            Debug.Log("Linea already chosen");
          }
        }
      } 
    } // fin AttributeLineaToPlayer()

    public void ResetActualPlayerID(int id) // and button color
    {
      GameObject[] btnObj = GameObject.FindGameObjectsWithTag("BtnListLinea");
      for (var i = 0; i < Lineas.Count; i++)
      {
        if (Lineas[i].ActualPlayerID == id) // reset linea chosen
        {
          for (var x = 0; x < btnObj.Length; x++)
          {
            Button btn = btnObj[x].GetComponent<Button>();
            if (btn.GetComponent<BtnListIndex>().index == Lineas[i].LineaID && btn.GetComponent<BtnListIndex>().indexPlayer == id)
            {
              TurnGreyBtn(btn);
          } // if
          } // for
          Lineas[i].ActualPlayerID = 0;
        } // if
      } // for
    }

    // In BtnStart OnClick()
    public void ReadyPlayersInML(Button btn)
    {
      for (var i = 0; i < Players.Count; i++)
      {
        if (btn.transform.parent.parent.name == "LinPanelP" + (i + 1) && Players[i].GetComponent<Player>().HasLinea)
        {
          if (!Players[i].GetComponent<Player>().Ready)
          {
            playerReady++;
            Players[i].GetComponent<Player>().Ready = true;
            Debug.Log("Player : " + i + " ready !");
            TurnGreenBtn(btn);
            if (playerReady >= (Players.Count + 1)) // Prevent bug players ready doesnt exist
            {
              playerReady--;
            }
            Debug.Log("Players ready = " + playerReady);
          }
          else
          {
            playerReady--;
            Players[i].GetComponent<Player>().Ready = false;
            Debug.Log("Player : " + i + " unready !");
            Debug.Log("Players ready = " + playerReady);
            AllPlayersReady = false;
            TurnGreyBtn(btn);
          }
        }
        else if (btn.transform.parent.parent.name == "LinPanelP" + (i + 1) && !Players[i].GetComponent<Player>().HasLinea)
        {
          Debug.Log("You have to choose a Linea");
          return;
        }
        // CHECK IF ALL READY AND LAUNCH, ADD Players.Count > 1 FOR ML (?)
        if (playerReady == Players.Count)
        {
          int haveLinea = 0;
          for (var x = 0; x < Players.Count; x++)
          {
            if (Players[x].GetComponent<Player>().HasLinea)
            {
              haveLinea++;
            }
            else {
              haveLinea--;
            }
          }
          if (haveLinea == Players.Count)
          {
            AllPlayersReady = true;
            Debug.Log("All Players are ready : " + playerReady);
          }
          else {
           Debug.Log("Some Players don't have Linea");    
          }   
        }
        //
      }
    }

    // In BtnDelete OnClick()
    public void InputDeleteLinea(Button btn)
    {
      for (var i = 0; i < Players.Count; i++)
      {
        if (btn.transform.parent.parent.name == "LinPanelP" + (i + 1) && Players[i].GetComponent<Player>().HasLinea)
        {
          for (var x = 0; x < Lineas.Count; x++)
          {
            if (Lineas[x].name == Players[i].GetComponent<Player>()._player.name)
            {
              LineaToDelete.name = Lineas[x].name;
            }
          }
        }
      }
    }

    // In InputDelete OnClick()
    public void DeleteLinea(InputField _input)
    {
      if (_input.text == LineaToDelete.name)
      {
        for (var x = 0; x < Lineas.Count; x++)
        {
          if (LineaToDelete.name == Lineas[x].name)
          {
            Debug.Log("Delete Linea : " + Lineas[x].name);
            ResetPlayer(LineaToDelete.name);
            Lineas.RemoveAt(x);
            SaveLineas();
          }
          else {
            Debug.Log("Wrong Linea Name");
          }
        }
      }
    }
    // END ML
    #endregion

    // With DeleteLinea
    private void ResetPlayer(string str)
    {
      for (var i = 0; i < Players.Count; i++)
      {
        if (Players[i].GetComponent<Player>()._player.name == str)
        {
          Players[i].GetComponent<Player>()._player.name = null;
          Players[i].GetComponent<Player>()._player.LineaID = 0;
          Players[i].GetComponent<Player>()._player.desc = null;
          Players[i].GetComponent<Player>().HasLinea = false;
          GameObject.Find("P"+(i+1)+"Text").GetComponent<Text>().text = "P"+(i+1)+":";
        }
      }
    }
    
    #region UIHelper
    public void TurnGreenBtn(Button btn)
    {
      ColorBlock cb = btn.colors;
      cb.normalColor = Color.green;
      cb.selectedColor = Color.green;
      btn.colors = cb;
    }

    public void TurnGreyBtn(Button btn)
    {
      ColorBlock cb = btn.colors;
      cb.normalColor = Color.grey;
      cb.selectedColor = Color.white;
      btn.colors = cb;
    }
    #endregion

    // On submit on input field
    public void CreateNewLignee(InputField _input) // New LINEA
    {
      Debug.Log(Lineas.Count);
      string LineaName = _input.text;
      if (LineaName.Length > 2)
      {
        PVP.DataPlayer.Linea linea = new PVP.DataPlayer.Linea
        {
          name = LineaName
        };
        Lineas.Add(linea);
        for (var i = 0; i < Lineas.Count; i++)
        {
          int randomNum = Random.Range(1,1000);
          if (Lineas[i].LineaID == randomNum)
          {
            return;
          }
          else {
            linea.LineaID = randomNum;
            Debug.Log("Création Lignée : "+ Lineas[i].name + " ID : " + Lineas[i].LineaID);
          }
        }
        SaveLineas();
      }
      else 
      {
        Debug.Log("Name too small !");
      }
    }

    public void SaveLineas()
    {
      JsonHelper.WriteToJsonFile(path, Lineas);
      Lineas = JsonHelper.ReadFromJsonFile<List<PVP.DataPlayer.Linea>>(path);
      Debug.Log("Save Lineas");
    }

}// class GameManager
}// namespace PVP