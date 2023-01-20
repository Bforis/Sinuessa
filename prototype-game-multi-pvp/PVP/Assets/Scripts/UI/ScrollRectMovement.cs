using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;


public class ScrollRectMovement : MonoBehaviour
{
    public RectTransform scrollRect;
    public RectTransform content;
    GameObject previouslySelected;
    private MultiplayerEventSystem MES;
    private string grandparent;
    PVP.GameManager GM;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<PVP.GameManagerHandler>().gameManager;

    }

    private void Update()
    {
        ScrollWithSelection(scrollRect, content);
    }

    private void ScrollWithSelection(RectTransform _scrollRect, RectTransform _content)
    {
        grandparent = transform.parent.name;
        for (var i = 0; i < (GM.Players.Count + 1); i++)
        {
            if (grandparent == "LinPanelP" + i){
                MES = GameObject.Find("MenuMultiplayer/LobbyEventSystemP" + i).GetComponent<MultiplayerEventSystem>();
            }
        }
        GameObject selected = MES.currentSelectedGameObject;
        if (selected == null || selected == previouslySelected) return;
        if (selected.transform.parent != _content.transform) return;
        RectTransform selectedRectTransform = selected.GetComponent<RectTransform>();


        float scrollViewMinY = _content.anchoredPosition.y;
        float scrollViewMaxY = _content.anchoredPosition.y + _scrollRect.rect.height;


        float selectedPositionY = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + (selectedRectTransform.rect.height / 2);

        // If selection below scroll view
        if (selectedPositionY > scrollViewMaxY)
        {
            float newY = selectedPositionY - _scrollRect.rect.height;
            _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, newY);
        }


        // If selection above scroll view
        else if (Mathf.Abs(selectedRectTransform.anchoredPosition.y) < scrollViewMinY)
        {
            _content.anchoredPosition =
            new Vector2(_content.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y)
                - (selectedRectTransform.rect.height / 2));
        }
        previouslySelected = selected;
        
    }
}
