using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
Script dans prefab Player.
Sert à sauvegarder toutes les données du joueur et de sa lignée correspondante.
On ne détruit jamais l'instance du prefab, sauf si un joueur arrête de jouer. 
La catégorie Choix Lignee Button est en lien avec le ChoiceMenu. 
*/
[System.Serializable]
public class PlayerData : MonoBehaviour
{
    [SerializeField]
    private DataPlayer.Lignee player;
    public DataPlayer.Lignee Player => player; // GETTER
    public static List<DataPlayer.Lignee> AllPlayers = new List<DataPlayer.Lignee>(); // List des joueurs // A voir pour utiliser un Observer Pattern !
    void Awake()
    {
        player = new DataPlayer.Lignee();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        Player.ActualPlayerID = ManagerScript.totalPlayerIndex;
    }

    void Update()
    {
        /*if (MainMenu.ChoiceMenuActive)
        {
            BtnTrigger();
        }*/
    }

    ///////////// CHOIX LIGNEE BOUTON //////////
    public void BtnTrigger()
    {
        // ReadyButton, do whatever, like RegisterPlayer()
        GameObject ReadyButton = GameObject.FindGameObjectWithTag("ReadyButton");
        ReadyButton.GetComponent<Button>().onClick.RemoveListener(RegisterPlayer);
        ReadyButton.GetComponent<Button>().onClick.AddListener(RegisterPlayer);
        // end

        // BtnLignee en lien avec ButtonChoice(), seul manière de fonctionner => addListener, dans l'éditeur non.
        GameObject[] btnLignee = GameObject.FindGameObjectsWithTag("ButtonListLignee");
        foreach (GameObject btn in btnLignee)
        {
            btn.GetComponent<Button>().onClick.RemoveListener(() => ButtonChoice(btn.GetComponent<Button>(), Player));
            btn.GetComponent<Button>().onClick.AddListener(() => ButtonChoice(btn.GetComponent<Button>(), Player));
        }// end
        MainMenu.ChoiceMenuActive = false;
    }

    public void ButtonChoice(Button btn, DataPlayer.Lignee p) // in onClick on button lignee created with ShowListLignee.cs CreateButtonLigneeList();
    {
        foreach (var l in RecordLignee.Lignee)
        {
            int actualPlayerID = l.ActualPlayerID;
            string name = l.name;
            var desc = l.desc;

            if (l.id != btn.GetComponent<BtnListIndex>().indexBtn && actualPlayerID == p.ActualPlayerID) // RESET POUR NE PAS AVOIR DEUX LIGNEES
            {
                actualPlayerID = 0;
                l.ActualPlayerID = actualPlayerID;
            }
            else if (l.id == btn.GetComponent<BtnListIndex>().indexBtn && actualPlayerID == 0)
            {
                Debug.Log("Joueur : " + p.ActualPlayerID + " a choisi la lignée : " + name);
                actualPlayerID = p.ActualPlayerID;
                l.ActualPlayerID = actualPlayerID;
                p.name = name;
                p.desc = desc;
            }
            else if (actualPlayerID != 0 && actualPlayerID != p.ActualPlayerID)
            {
                Debug.Log("Un joueur a déjà choisis cette lignée");
            }
        }
    }

    public void RegisterPlayer()
    {
        if (!AllPlayers.Contains(Player))
        {
            AllPlayers.Add(Player);
        }
        Debug.Log(Player.name + " is ready");
    }

    //////// END CHOIX LIGNEE BOUTON /////////
}
