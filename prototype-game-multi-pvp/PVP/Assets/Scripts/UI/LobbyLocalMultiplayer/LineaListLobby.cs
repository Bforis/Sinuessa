using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LineaListLobby : MonoBehaviour
{
    public GameObject BtnLinea;
    public GameObject PanelP1;
    public GameObject PanelP2;
    public GameObject PanelP3;
    public GameObject PanelP4;
    public List<GameObject> Panels;

    // SHOW LINEAS IN MENU MUTLIPLAYER LOCAL
    void Start()
    {
        ShowListLinea();
    }

    public void ShowListLinea(){
        // Reset clones boutons lineas
        var clones = GameObject.FindGameObjectsWithTag("BtnListLinea");
        foreach (var clone in clones)
            {
                Destroy(clone);
            }
        Panels.Clear();
        Panels.Add(PanelP1);
        Panels.Add(PanelP2);
        Panels.Add(PanelP3);
        Panels.Add(PanelP4);

      foreach (var panel in Panels)
            {
                float posY = panel.transform.position.y;
                float posX = panel.transform.position.x;
                float posZ = 0;

                for (var i = 0; i < PVP.GameManager.Lineas.Count; i++)
                {
                    string name = PVP.GameManager.Lineas[i].name;
                    int id = PVP.GameManager.Lineas[i].LineaID;
                    Vector3 samplePosition = new Vector3(posX, posY, posZ);
                    GameObject sample = Instantiate(BtnLinea, samplePosition, Quaternion.identity);
                    sample.GetComponentInChildren<Text>().text = name;
                    Button btn = sample.GetComponentInChildren<Button>();
                    BtnListIndex btnId = btn.GetComponentInChildren<BtnListIndex>();
                    btnId.index = id;
                    sample.transform.SetParent(panel.transform, false);
                }
            }   
    }
}
