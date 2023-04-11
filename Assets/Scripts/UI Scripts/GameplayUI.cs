using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinCountText;

    void Start()
    {
        UpdateCoinUI();
        Helpers.DataManagerScript.OnCoinAdded += UpdateCoinUI;
    }

    void UpdateCoinUI()
    {
        coinCountText.text = "x" + Helpers.DataManagerScript.CoinCount.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
