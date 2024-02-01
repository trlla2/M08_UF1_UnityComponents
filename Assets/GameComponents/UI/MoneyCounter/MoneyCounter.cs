using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private const string MoneyLabelTranslationKey = "money_label";

    private void OnEnable()
    {
        SystemManager sM = SystemManager.Instance;
        UpdateTextWithCurrentMoney(sM.Money);
        sM.OnMoneyChamge += UpdateTextWithCurrentMoney; //SIEMPRE QUE MANEGES LA SUBCRIPCION DE UN EVENTO TIENES QUE MANEGAR LA DESUBCRIBCION
    }

    private void UpdateTextWithCurrentMoney(int money)
    {
        //string text = TranslationManager.GetString(MoneyLabelTranslationKey, money.ToString());
        string text = "Money: " + money.ToString();
        _text.text = text;
    }

    private void OnDisable()
    {
        SystemManager sM = SystemManager.Instance;
        sM.OnMoneyChamge -= UpdateTextWithCurrentMoney;
    }
}
