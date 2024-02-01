using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SystemManager 
{
    private static SystemManager instance;
    static public SystemManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SystemManager();
            }
            return instance;
        }
    }

    public delegate void MoneyChamge(int money);
    public event MoneyChamge OnMoneyChamge;

    private const string MoneyKey = "Money";
    private int _money = 0;

    public int Money
    {
        get => _money;
    }

    public bool ModifyMoney(int value)
    {
        if(Money + value < 0)
        {
            return false;
        }
        _money += value;
        OnMoneyChamge?.Invoke(_money);
  
        return true;
    }

    private SystemManager()
    {
        LoadData();
    }

    private void LoadData()
    {
        LoadMoney();
        //Other load
    }

    private void LoadMoney()
    {
        //Load From File or other DB
        _money = PlayerPrefs.GetInt(MoneyKey, 0);
    }
}
