using UnityEngine;

/*
Script présent dans MenuManager pour gérer tout les menus.
La plus part des fonctions sont activés dans les OnClick() des buttons sur l'éditeur.
Peut-être manque de MVC Pattern / State Pattern ! 
*/
public class MainMenu : MonoBehaviour
{
    public GameObject FirstMenu;
    public GameObject ReturnButton;
    public static bool ChoiceMenuActive = false; // Marque l'ouverture du menu choix relié à la fonction dans PlayerScript.cs : BtnLigneeApplyFunction()
    public static bool ChoiceMenuStayActive = false; // La même chose que ChoiceMenuActive mais non static, seulement pour PlayerUI. Les deux ne pouvant pas être partagé car le bool static change dans plusieurs scripts.

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void ActiveChoiceMenu()
    {
        FirstMenu.SetActive(false);
        ReturnButton.SetActive(true);
        ChoiceMenuActive = true;
        ChoiceMenuStayActive = true;
    }

    public void Return()
    {
        ReturnButton.SetActive(false);
        FirstMenu.SetActive(true);
        ChoiceMenuActive = false;
        ChoiceMenuStayActive = false;
    }
}
