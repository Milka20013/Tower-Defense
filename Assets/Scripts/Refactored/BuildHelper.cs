using UnityEngine;

public class BuildHelper : MonoBehaviour
{
    public static BuildHelper instance;
    private TowerBlueprint towerToBuild;
    public GameObject particles;

    [SerializeField] private GameEventContainer eventContainer;
    private void Awake()
    {
        instance = this;
    }
    public void SelectTowerToBuild(object towerBlueprintObj)
    {
        if (towerBlueprintObj == null)
        {
            Debug.LogError("Selected tower to build was null");
            return;
        }
        towerToBuild = (TowerBlueprint)towerBlueprintObj;
    }

    public void DeselectTowerToBuild()
    {
        towerToBuild = null;
        eventContainer.onTowerToBuildDeSelected.RaiseEvent();
    }
    public TowerBlueprint GetTowerToBuild()
    {
        return towerToBuild;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DeselectTowerToBuild();
        }
    }
}
