using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private int amount = 0;

    private bool collected = false;

    public void CoinCollected() {
        if (!collected) {
            collected = true;
            /* 
                FindGameObjectWithTag лучше не использовать из-за её неээфективности, но
                т.к. она вызывается только один раз, я решил не использовать её, чтобы
                не хранить ссылку на CoinsManager 
            */
            GameObject.FindGameObjectWithTag("CoinsManager")
                .GetComponent<CoinsManager>().AddCoins(amount);

            GetComponent<Animation>().Play();
        }
    }

    public void DestroyCoin() {
        Destroy(gameObject);
    }
}
