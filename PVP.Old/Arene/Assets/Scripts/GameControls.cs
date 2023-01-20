using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControls : MonoBehaviour
{
    public static int playerIndex;
    public static Scene scene;
    [SerializeField]
    private int ready;
    [SerializeField]
    private bool launch;
    public static int alive;
    public static bool RESET = false;

    public GameObject Player;
    public Dictionary<int, GameObject> PlayerID;
    
    // UI READY - JOIN
    public GameObject JoinText;
    public GameObject readyOne;
    public GameObject readyTwo;
    public GameObject readyThree;
    public GameObject readyFour;
    //

    void Awake(){
        PlayerID = new Dictionary<int, GameObject>();
        DontDestroyOnLoad(this.gameObject); 
    }

    void Start(){
        playerIndex = 0;
    }

    public void GetPlayerIndex(){
        playerIndex += 1;
        PlayerID.Add(playerIndex, Player);
        alive += 1;
    }

    public void SousPlayerIndex(){
        playerIndex -= 1;
    }

    void Update(){
        launch = PlayerActions.launch;
        PlayerActions.launch = launch;
        scene = SceneManager.GetActiveScene();
        ready = PlayerActions.ready;
        PlayerActions.ready = ready;

        if (launch == true){ // RESET pour pouvoir relancer après
            ready = 0; 
            PlayerActions.ready = ready;
        }
        
        RESTART();

        // LAUNCH
        if (playerIndex > 1 && ready == playerIndex && scene.buildIndex == 0 && launch == false){ // READY LAUNCH GAME
        launch = true;
        SceneManager.LoadScene(1, LoadSceneMode.Single); // Lancement de la scene
        }
        // FIN LAUNCH
    
        if (alive <= 1 && scene.buildIndex == 1){
            Debug.Log(alive);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            RESET = true;
        }

        if (scene.buildIndex == 0){
            RESET = false;
        if (playerIndex < 4){ // TEXT REJOINDRE
            JoinText.SetActive(true);
        }
        else{
            JoinText.SetActive(false);
        }
        //

        if (ready == 1){ // Bouton bleue indiquant le nombre de personne prête
            readyOne.SetActive(true);
        }
        if (ready == 2){
            readyOne.SetActive(true);
            readyTwo.SetActive(true);
        }
        if (ready == 3){
            readyOne.SetActive(true);
            readyTwo.SetActive(true);
            readyThree.SetActive(true);
        }
        if (ready == 4){
            readyOne.SetActive(true);
            readyTwo.SetActive(true);
            readyThree.SetActive(true);
            readyFour.SetActive(true);
        }
        //
        }
        else{
            readyOne.SetActive(false);
            readyTwo.SetActive(false);
            readyThree.SetActive(false);
            readyFour.SetActive(false);
        }
    }

    void RESTART(){
        if (RESET == true){
            ready = 0;
            alive = playerIndex;
            launch = false;
        }
    }

    //
} // FIN

