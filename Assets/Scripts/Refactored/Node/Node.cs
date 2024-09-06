using UnityEngine;
using UnityEngine.EventSystems;


public class Node : MonoBehaviour
{

    [Header("Optional")]
    public GameObject placedObject;
    private TowerBlueprint towerBlueprint;
    public NodeAttribute[] nodeAttributes;
    public Transform spawnPoint;

    [SerializeField] GameEventContainer eventContainer;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        ClickNode();

    }

    private void ClickNode()
    {
        if (placedObject != null)
        {
            placedObject.GetComponent<INodePlaceable>().OnSelect();
            return;
        }
        TryBuildTower(BuildHelper.instance.GetTowerToBuild());
        BuildHelper.instance.DeselectTowerToBuild();
    }

    private bool TryBuildTower(TowerBlueprint blueprint)
    {
        if (blueprint == null)
        {
            return false;
        }
        bool hasNoObj = blueprint.prefab != null;
        bool isAcceptableTower = NodeTowerRelationship.IsAcceptableTower(blueprint.relations, nodeAttributes);

        if (hasNoObj && isAcceptableTower)
        {
            BuildTower(blueprint);
            return true;
        }
        return false;
    }
    public void BuildTower(TowerBlueprint blueprint)
    {
        PlayerStats.instance.AddMoney(-blueprint.cost);
        GameObject tower = Instantiate(blueprint.prefab, GetBuildPosition(), blueprint.prefab.transform.rotation);
        tower.GetComponent<Tower>().node = this;
        placedObject = tower;
        towerBlueprint = blueprint;
        GameObject effect = Instantiate(BuildHelper.instance.particles, GetBuildPosition(), blueprint.prefab.transform.rotation);
        eventContainer.onTowerPlaced.RaiseEvent(blueprint);
        Destroy(effect, 3f);
    }
    public Vector3 GetBuildPosition()
    {
        return spawnPoint.position;
    }
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //display something if tower is selected and nothing is on the tile
        var towerToBuild = BuildHelper.instance.GetTowerToBuild();
        eventContainer.onNodeHovered.RaiseEvent(this);

    }
    public void OnMouseExit()
    {
        eventContainer.onNodeHoverExit.RaiseEvent(this);
    }
    public void SellTower()
    {
        PlayerStats.instance.AddMoney(SellAmount());
        eventContainer.onTowerSold.RaiseEvent(placedObject);
        Destroy(placedObject);
        placedObject = null;
        towerBlueprint = null;
    }
    public float SellAmount()
    {
        float Amount = towerBlueprint.cost;
        return Amount * 3 / 4;
    }
}
