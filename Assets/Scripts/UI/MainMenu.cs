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
    private GameObject goGameStart;
    [SerializeField]
    private GameObject goEnter;

    [Header("Game Start")]
    [SerializeField]
    private InputField inputFieldNickname;

    [Header ("Enter")]
    [SerializeField]
    private InputField inputFieldIP;

    // -------------------------------------------------------------------------
    // 게임 접속
    // -------------------------------------------------------------------------
    public void EnterGame()
    {
        // !!! TEMP
        GameManager.Instance.GameScene(inputFieldNickname.text);
    }

    public void CheckValidServer()
    {
        // !!! valid ip 주소인지
        // !!! 서버가 열려있는지 확인

        //EnterGame();
    }

    // -------------------------------------------------------------------------
    // UI 조작
    // -------------------------------------------------------------------------
    public void OpenGameStart()
    {
        Close();
        goGameStart.SetActive(true);
    }

    public void CloseGameStart()
    {
        goGameStart.SetActive(false);
    }

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
        CloseGameStart();
        CloseMainMenu();
        CloseEnter();
    }
}
