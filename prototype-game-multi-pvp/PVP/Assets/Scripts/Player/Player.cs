using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Player : MonoBehaviour {
	[SerializeField]
	private PVP.DataPlayer.Linea player;
	public PVP.DataPlayer.Linea _player => player;
	public PlayerInput playerInput;
	[SerializeField]
	public PlayerStates PlayerState;
	public bool Ready = false;
	public bool HasLinea = false;
	[SerializeField]
	private string LineaName;
	[SerializeField]
	private int PlayerID;

	private PVP.GameManagerHandler GM;

	void Awake(){
		GM = GameObject.Find("GameManager").GetComponent<PVP.GameManagerHandler>();
		player = new PVP.DataPlayer.Linea();
		playerInput = gameObject.GetComponent<PlayerInput>();
		DontDestroyOnLoad(this);
		GM.gameManager.AddPlayer(gameObject);
		CheckStateAtStart();
	}

	void FixedUpdate(){
		UpdateInformations();
	}

	public void UpdateInformations(){
        LineaName = player.name;
		PlayerID = player.ActualPlayerID;
	}

	public void CheckStateAtStart(){
		switch (GM.gameManager.GameState){
			case PVP.GameStates.FirstMenu:
			{
				PlayerState = PlayerStates.InFirstMenu;
				break;
			}
			case PVP.GameStates.LobbyLocalMultiplayer:
			{
				PlayerState = PlayerStates.InLobbyLocalMultiplayer;
				break;
			}
			case PVP.GameStates.Playing:
			{
				PlayerState = PlayerStates.Playing;
				break;
			}
		}
	}
}