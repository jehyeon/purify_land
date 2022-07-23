using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private MainMenu mainMenu;

    // Scene 관리
    public void MainScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GameScene()
    {
        SceneManager.LoadScene("Main");
    }
}
