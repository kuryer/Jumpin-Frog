using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManagerScript : MonoBehaviour
{
    // tu bede zapisywal, wczytywal i udostepnial dane dla reszty gry (UI, sklep, Gracz itd.)
    //WA�NE NIE ZAPOMNIJ O TYM PLIKU XDDD
    private int coinCount = 0;
    public UnityAction OnCoinAdded;

    public int CoinCount
    {
        get { return coinCount; }
    }


    public void AddCoin()
    {
        coinCount++;
        OnCoinAdded?.Invoke();
    }


}
