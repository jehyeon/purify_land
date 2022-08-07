using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header ("Menu Tabs")]
    [SerializeField]
    private GameObject goMainMenu;
    [SerializeField]
    private GameObject goEnter;

    [Header ("Enter")]
    [SerializeField]
    private InputField inputFieldIP;

    // -------------------------------------------------------------------------
    // 게임 접속
    // -------------------------------------------------------------------------
    private void EnterGame()
    {
        GameManager.Instance.GameScene();
    }

    public void CheckValidServer()
    {
        // !!! valid ip 주소인지
        // !!! 서버가 열려있는지 확인

        EnterGame();
    }

    // -------------------------------------------------------------------------
    // UI 조작
    // -------------------------------------------------------------------------
    public void OpenMainMenu()
    {
        Close();
        goMainMenu.SetActive(true);
    }

    public void CloseMainMenu()
    {
        goMainMenu.SetActive(false);
    }

    public void OpenEnter()
    {
        Close();
        goEnter.SetActive(true);
    }

    public void CloseEnter()
    {
        goEnter.SetActive(false);
    }

    public void Close()
    {
        CloseMainMenu();
        CloseEnter();
    }
}
