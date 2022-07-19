using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject goOptionUI;

    private bool isActivatedOptionUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isActivatedOptionUI)
            {
                CloseOptionUI();
            }
            else
            {
                OpenOptionUI();
            }
        }
    }

    public void OpenOptionUI()
    {
        isActivatedOptionUI = true;
        goOptionUI.SetActive(true);
    }

    public void CloseOptionUI()
    {
        isActivatedOptionUI = false;
        goOptionUI.SetActive(false);
    }

    public void Close()
    {
        CloseOptionUI();
    }
}
