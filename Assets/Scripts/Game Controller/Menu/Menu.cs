using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    void Start()
    {
        menu.SetActive(false);

        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void No()
    { 
        Time.timeScale = 1;
        menu.SetActive(false);
    }

    public void Yes() { SceneManager.LoadScene("MineMenu"); }
}
