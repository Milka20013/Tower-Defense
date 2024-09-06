using TMPro;
using UnityEngine;

public class GameLostMenu : GameOverUIBase
{
    [SerializeField] private TextMeshProUGUI roundCountText;
    public override void Show(object currentRoundObj)
    {
        base.Show(currentRoundObj);
        roundCountText.text = ((int)currentRoundObj).ToString();
    }
}
