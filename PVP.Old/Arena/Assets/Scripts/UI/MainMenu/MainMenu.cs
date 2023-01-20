using UnityEngine;

/*
Script pr�sent dans MenuManager pour g�rer tout les menus.
La plus part des fonctions sont activ�s dans les OnClick() des buttons sur l'�diteur.
Peut-�tre manque de MVC Pattern / State Pattern ! 
*/
public class MainMenu : MonoBehaviour
{
    public GameObject FirstMenu;
    public GameObject ReturnButton;
    public static bool ChoiceMenuActive = false; // Marque l'ouverture du menu choix reli� � la fonction dans PlayerScript.cs : BtnLigneeApplyFunction()
    public static bool ChoiceMenuStayActive = false; // La m�me chose que ChoiceMenuActive mais non static, seulement pour PlayerUI. Les deux ne pouvant pas �tre partag� car le bool static change dans plusieurs scripts.

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
