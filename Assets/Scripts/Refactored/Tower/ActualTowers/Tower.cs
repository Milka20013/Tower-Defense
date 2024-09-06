using UnityEngine;

[RequireComponent(typeof(TowerUpgrader))]
[RequireComponent(typeof(TowerStats))]
[RequireComponent(typeof(BoxCollider))]
public class Tower : MonoBehaviour, INodePlaceable
{
    [HideInInspector] public Node node;
    public TowerBlueprint blueprint;
    [SerializeField] protected GameEventContainer eventContainer;
    public void OnDeselect()
    {
        throw new System.NotImplementedException();
    }

    public void OnSelect()
    {
        eventContainer.onTowerSelected.RaiseEvent(this);
    }

    private void OnMouseDown()
    {
        OnSelect();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            eventContainer.onTowerDeselected.RaiseEvent(this);
        }
    }
}
