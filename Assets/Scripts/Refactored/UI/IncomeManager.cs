using System.Collections;
using TMPro;
using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] private float breakBetweenDisplays;
    private float incomeSum;

    private float currentTimeBetweenDisplays;
    private bool doDisplay = false;

    private void Update()
    {
        currentTimeBetweenDisplays += Time.deltaTime;
        if (currentTimeBetweenDisplays >= breakBetweenDisplays)
        {
            if (!doDisplay)
            {
                return;
            }
            DisplayIncome();
            currentTimeBetweenDisplays = 0f;
        }
    }
    public void OnIncomeGenerated(object incomeObj)
    {
        incomeSum += (float)incomeObj;
        doDisplay = true;
    }

    private void DisplayIncome()
    {
        moneyText.gameObject.SetActive(true);
        moneyText.text = "+" + Mathf.RoundToInt(incomeSum).ToString() + "$";
        StartCoroutine(MoveMoneyText());
        PlayerStats.instance.AddMoney(incomeSum);
        incomeSum = 0f;
    }

    IEnumerator MoveMoneyText()
    {
        Vector3 pos = moneyText.gameObject.transform.localPosition;
        int i = 0;
        while (moneyText.gameObject.transform.localPosition.y <= 30f)
        {
            moneyText.gameObject.transform.localPosition = new Vector3(pos.x, pos.y + i * Time.timeScale);
            i++;
            yield return null;
        }
        moneyText.gameObject.transform.localPosition = pos;
        moneyText.gameObject.SetActive(false);
        doDisplay = false;
    }
}
