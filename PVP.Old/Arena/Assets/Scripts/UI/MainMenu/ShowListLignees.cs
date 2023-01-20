using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Script dans le prefab ChoiceMenu.
La fonction CreateButtonLigneeList() est utilisé pour détruire la liste existante de bouton qui représente
les lignées pour les choisir dans la scene MainMenu. Elle utilise le prefab BtnListLignee pour l'instancier.
Toujours en lien avec la Record.Lignee.Count du Record.cs dans le prefab GameManager.
*/

public class ShowListLignees : MonoBehaviour
{
    public GameObject buttonLignee;
    public GameObject canvasParent;
    void Awake()
    {
        CreateButtonLigneeList();
    }

    public void CreateButtonLigneeList() // Create button lignee list menu 1
    {
        float posY = canvasParent.transform.position.y + 3.50f;
        float posX;
        var clones = GameObject.FindGameObjectsWithTag("ButtonListLignee"); // RESET LIST
        foreach (var clone in clones)
        {
            Destroy(clone);
        }

        for (var i = 0; i < RecordLignee.Lignee.Count; i++)
        {
            string name = RecordLignee.Lignee[i].name;
            int id = RecordLignee.Lignee[i].id;
            posY -= 1f;
            posX = canvasParent.transform.position.x - 0.50f;
            Vector2 samplePosition = new Vector2(posX, posY);
            GameObject sample = Instantiate(buttonLignee);
            sample.GetComponentInChildren<Text>().text = name;
            // Button list id 
            Button btn = sample.GetComponentInChildren<Button>();
            BtnListIndex btnId = btn.GetComponentInChildren<BtnListIndex>();
            btnId.indexBtn = id;
            // end 
            sample.transform.SetParent(canvasParent.transform, false);
            sample.transform.position = samplePosition;
        }
        MainMenu.ChoiceMenuActive = true;
    }
}
