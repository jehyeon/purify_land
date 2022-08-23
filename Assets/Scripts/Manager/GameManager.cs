using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    public string Nickname { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Scene 관리
    public void MainScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GameScene(string nickname)
    {
        this.Nickname = nickname;
        SceneManager.LoadScene("ServerTest");
    }

    public void ExitGame()
    {
        // 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //
    public void Enter()
    {
        C_EnterGame enter = new C_EnterGame();
        enter.nickname = Nickname;

        NetworkManager.Instance.Send(enter.Write());
    }
}
