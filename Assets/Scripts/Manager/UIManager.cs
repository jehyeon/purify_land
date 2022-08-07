using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance { get { return instance; } }

    [SerializeField]
    private GameObject goOptionUI;

    private bool isActivatedOptionUI;

    // 체력바 시스템
    [SerializeField]
    private Transform hpBarParent;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

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

    // -------------------------------------------------------------------------
    // 체력바
    // -------------------------------------------------------------------------
    public HpBar CreateHpBar()
    {
        Object hpBarPref = Resources.Load("Prefabs/UI/HpBar");
        GameObject hpBar = Instantiate(hpBarPref) as GameObject;
        hpBar.transform.SetParent(hpBarParent);

        return hpBar.GetComponent<HpBar>();
    }

    // -------------------------------------------------------------------------
    // UI 조작
    // -------------------------------------------------------------------------
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
