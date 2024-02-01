using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField, Min(0)] private int _coinValue = 1;
    public void GetCoin()
    {
        SystemManager sm = SystemManager.Instance;

        sm.ModifyMoney(_coinValue);
       
        Destroy(this.gameObject);
    }
}
