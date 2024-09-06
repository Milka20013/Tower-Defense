using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [HideInInspector] public TowerBlueprint towerBlueprint;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image icon;
    [SerializeField] private GameEventContainer eventContainer;

    private bool isAnimationPlaying = false;
    private void Start()
    {
        costText.text = "$" + towerBlueprint.cost.ToString();
        icon.sprite = towerBlueprint.icon;
    }

    public void SelectTowerToBuild()
    {
        if (PlayerStats.instance.GetMoney() < towerBlueprint.cost)
        {
            if (isAnimationPlaying)
            {
                return;
            }
            StartCoroutine(PlayWiggleAnimation());
            return;
        }
        eventContainer.onTowerToBuildSelected.RaiseEvent(towerBlueprint);
    }

    IEnumerator PlayWiggleAnimation()
    {
        isAnimationPlaying = true;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                transform.Rotate(new Vector3(0, 1, 0), 15);
                yield return null;
            }
            for (int j = 0; j < 4; j++)
            {
                transform.Rotate(new Vector3(0, 1, 0), -15);
                yield return null;
            }
        }
        isAnimationPlaying = false;
    }
}
