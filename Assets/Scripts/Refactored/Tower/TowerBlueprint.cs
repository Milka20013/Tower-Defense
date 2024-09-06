using UnityEngine;

[CreateAssetMenu(menuName = "Tower/Blueprint/Basic")]
public class TowerBlueprint : ScriptableObject
{
    public string towerName;
    public Sprite icon;
    public GameObject prefab;
    public float cost;
    public NodeTowerRelationship[] relations;
    public TowerAttributeSelector[] attributes;
    public TowerType[] types;
}
