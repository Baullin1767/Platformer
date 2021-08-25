using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MineMenu : MonoBehaviour
{
    public void NewGame() 
    {
        SceneManager.LoadScene("Level");
    }
    public void Exit() 
    {
        Application.Quit();
    }
}
