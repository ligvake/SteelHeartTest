using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsManager : MonoBehaviour
{
    public TMP_Text CoinsText;

    private int _currentCoinsAmount;
    public int currentCoinsAmount {
        get { return _currentCoinsAmount; }
        set { 
            _currentCoinsAmount = value;
            // задание текста я реализовал прямо в сеттере, для удобства
            CoinsText.text = _currentCoinsAmount.ToString();
        }
    }

    public void AddCoins(int amount) {
        currentCoinsAmount += amount;
    }
}
