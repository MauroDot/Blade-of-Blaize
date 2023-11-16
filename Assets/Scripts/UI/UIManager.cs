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
    public TMP_Text gemCountText;
    public Image[] healthBars;
    public void OpenShop(int gemCount)
    {
        playerGemCount.text = "" + gemCount + "G";
    }
    private void Awake()
    {
        _instance = this;
    }
    public void UpdateShopSelection(int ypos)
    {
        selectionImage.rectTransform.anchoredPosition = new Vector2(selectionImage.rectTransform.anchoredPosition.x, ypos);
    }

    public void UpdateGemCount(int count)
    {
        gemCountText.text = "" + count;
    }

    public void UpdateLives(int livesRemaining)
    {
        for (int i = 0; i <= livesRemaining; i++)
        {
            if(i == livesRemaining)
            {
                healthBars[i].enabled = false;
            }
        }
    }
}
