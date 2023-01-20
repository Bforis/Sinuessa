using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
Script dans le prefab GameManager. Initialise le jeu avec le JSON.
Sauvegarde plusieurs donn�es des joueurs et des lign�es.
Awake() cr�er la lign�e depuis le dossier Json gr�ce au package JsonHelper ajout� dans unity (script). Elle reset aussi la liste des lign�es.
CreateNewLignee() est utilis� dans l'inputField du prefab ChoiceMenu pour cr�er une nouvelle lign�e.
*/

[System.Serializable]
public class RecordLignee : MonoBehaviour
{
    public static List<DataPlayer.Lignee> Lignee = new List<DataPlayer.Lignee>(); // create Lign�e
    private string path; // STREAMING = IN EDITOR (TEST)

    void Awake()
    {
        path = Application.streamingAssetsPath + "/" + "data.json";
        Lignee = JsonHelper.ReadFromJsonFile<List<DataPlayer.Lignee>>(path); // Get all lignees in json to object in unity
        for (var i = 0; i < Lignee.Count; i++) // RESET PLAYER => LIGNEE
        {
            Lignee[i].ActualPlayerID = 0;
        }
        Debug.Log(Lignee.Count);
    }

    public void CreateNewLignee(InputField _input) // Create new lignee (in onClick() button create lignee)
    {
        Debug.Log(Lignee.Count);
        path = Application.streamingAssetsPath + "/" + "data.json";
        string LigneeName = _input.text;
        DataPlayer.Lignee lignee = new DataPlayer.Lignee
        {
            name = LigneeName
        };
        Lignee.Add(lignee);
        for (var i = 0; i < Lignee.Count; i++)
        {
            lignee.id = i + 1;
            Debug.Log("Lign�e : "+ Lignee[i].name + " ID : " + Lignee[i].id);
        }
        // JsonHelper.WriteToJsonFile(path, Lignee);
        // Lignee = JsonHelper.ReadFromJsonFile<List<DataPlayer.Lignee>>(path);
        SaveLignee();
    }

    public void SaveLignee()
    {
        JsonHelper.WriteToJsonFile(path, Lignee);
        Lignee = JsonHelper.ReadFromJsonFile<List<DataPlayer.Lignee>>(path);
    }
}
