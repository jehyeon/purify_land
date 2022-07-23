using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header ("Menu Tabs")]
    [SerializeField]
    private GameObject goMainMenu;
    [SerializeField]
    private GameObject goLobby;

    public void OpenMainMenu()
    {
        Close();
        goMainMenu.SetActive(true);
    }

    public void CloseMainMenu()
    {
        goMainMenu.SetActive(false);
    }

    public void OpenLobby()
    {
        Close();
        goLobby.SetActive(true);
    }

    public void CloseLobby()
    {
        goLobby.SetActive(false);
    }

    public void Close()
    {
        CloseMainMenu();
        CloseLobby();
    }
}
