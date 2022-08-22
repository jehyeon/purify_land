using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    public static UIManager Instance { get { return _instance; } }

    [SerializeField]
    private GameObject _goOptionUI;

    public bool IsActivatedOptionUI { get; private set; }

    [SerializeField]
    private GameObject _goInventoryUI;
    public bool IsActivatedInventoryUI { get; private set; }

    // 체력바 시스템
    [SerializeField]
    private Transform _hpBarParent;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // -------------------------------------------------------------------------
    // 체력바
    // -------------------------------------------------------------------------
    public HpBar CreateHpBar()
    {
        // !!! 오브젝트 풀링 적용하기
        Object hpBarPref = Resources.Load("Prefabs/UI/HpBar");
        GameObject hpBar = Instantiate(hpBarPref) as GameObject;
        hpBar.transform.SetParent(_hpBarParent);

        return hpBar.GetComponent<HpBar>();
    }

    // -------------------------------------------------------------------------
    // UI 조작
    // -------------------------------------------------------------------------
    public void OpenOptionUI()
    {
        this.IsActivatedOptionUI = true;
        _goOptionUI.SetActive(true);
    }

    public void CloseOptionUI()
    {
        this.IsActivatedOptionUI = false;
        _goOptionUI.SetActive(false);
    }

    public void OpenInventoryUI()
    {
        this.IsActivatedInventoryUI = true;
        _goOptionUI.SetActive(true);
    }

    public void CloseInventoryUI()
    {
        this.IsActivatedInventoryUI = false;
        _goOptionUI.SetActive(false);
    }

    public void Close()
    {
        CloseOptionUI();
        CloseInventoryUI();
    }
}
