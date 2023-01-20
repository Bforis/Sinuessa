using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    public static int totalPlayerIndex;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void AddPlayerIndex()
    {
        totalPlayerIndex++;
    }

    public void SousPlayerIndex()
    {
        totalPlayerIndex--;
    }
}