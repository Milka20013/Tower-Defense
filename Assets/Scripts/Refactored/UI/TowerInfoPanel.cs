using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private Image icon;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeCostText;

    private Targeting targeting;
    [SerializeField] private GameObject targetingContainer;
    [SerializeField] private TextMeshProUGUI primaryTargetingFilterText;
    [SerializeField] private TextMeshProUGUI finalTargetingFilterText;


    private Tower currentTower;

    public void OnTowerSelected(object towerObj)
    {
        currentTower = (Tower)towerObj;
        TowerBlueprint bp = currentTower.blueprint;

        towerNameText.text = bp.towerName;
        icon.sprite = bp.icon;

        TowerUpgrader upgrader = currentTower.GetComponent<TowerUpgrader>();
        string upgradeText;
        if (upgrader.upgradeBp == null)
        {
            upgradeText = "Max Level";
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeText = upgrader.upgradeBp.cost.ToString() + "$";
            upgradeButton.interactable = true;
        }

        InitializeTargetingOptions();

        upgradeCostText.text = upgradeText;
        panel.SetActive(true);
    }



    private void InitializeTargetingOptions()
    {
        if (currentTower.TryGetComponent(out targeting))
        {
            targetingContainer.SetActive(true);
            primaryTargetingFilterText.text = targeting.primaryTargetingFilter.ToFriendlyString();
            finalTargetingFilterText.text = targeting.finalTargetingFilter.ToFriendlyString();
        }
        else
        {
            targetingContainer.SetActive(false);
        }
    }
    public void ChangeFinalTargeting(int direction)
    {
        if (targeting == null)
        {
            return;
        }

        int index = (int)targeting.finalTargetingFilter;
        int next = Utility.Mod(index + direction, Enum.GetNames(typeof(FinalTargetingFilter)).Length);

        targeting.finalTargetingFilter = (FinalTargetingFilter)next;

        finalTargetingFilterText.text = targeting.finalTargetingFilter.ToFriendlyString();
    }

    public void ChangePrimaryTargeting(int direction)
    {
        if (targeting == null)
        {
            return;
        }

        int index = (int)targeting.primaryTargetingFilter;
        int next = Utility.Mod(index + direction, Enum.GetNames(typeof(PrimaryTargetingFilter)).Length);

        targeting.primaryTargetingFilter = (PrimaryTargetingFilter)next;

        primaryTargetingFilterText.text = targeting.primaryTargetingFilter.ToFriendlyString();
    }
    public void Upgrade()
    {
        currentTower.GetComponent<TowerUpgrader>().UpgradeTower();
        ClosePanel();
    }
    public void ClosePanel(object _ = null)
    {
        panel.SetActive(false);
    }
}
