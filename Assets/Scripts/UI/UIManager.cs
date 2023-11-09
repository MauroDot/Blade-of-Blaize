using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI Manager is Null!");
            }
            return _instance;
        }
    }

    public TMP_Text playerGemCount;
    public Image selectionImage;
    public void OpenShop(int gemCount)
    {
        playerGemCount.text = "" + gemCount + "G";
    }
    private void Awake()
    {
        _instance = this;
    }
}
