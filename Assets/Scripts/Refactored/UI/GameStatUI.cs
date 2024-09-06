using TMPro;
using UnityEngine;

public class GameStatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI roundText;

    public void UpdateHealthText(object health)
    {
        healthText.text = ((int)health).ToString();
    }

    public void UpdateMoneyText(object money)
    {
        moneyText.text = "$" + System.Math.Round((float)money, 1).ToString();
    }
    public void UpdateRoundText(object roundNumber)
    {
        roundText.text = "Round: " + ((int)roundNumber).ToString();
    }

}
