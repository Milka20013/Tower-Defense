using UnityEngine;

public class TowerUpgrader : MonoBehaviour
{
    public TowerBlueprint upgradeBp;

    public void UpgradeTower()
    {
        if (PlayerStats.instance.GetMoney() < upgradeBp.cost)
        {
            return;
        }
        Node node = GetComponent<Tower>().node;
        node.BuildTower(upgradeBp);
        Destroy(gameObject);
    }
}
