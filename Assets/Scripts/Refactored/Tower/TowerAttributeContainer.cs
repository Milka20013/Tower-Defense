using UnityEngine;

//[CreateAssetMenu(menuName = "Tower/AttributeContainer")]
//This class should be used as an enum.
//Only one instance should exist
public class TowerAttributeContainer : ScriptableObject
{
    [Header("Tower Specific")]
    public TowerAttribute attackSpeed;
    public TowerAttribute range;
    public TowerAttribute tickRate;
    public TowerAttribute tickCount;
    public TowerAttribute endOfRoundMoney;
    public TowerAttribute moneyPerTick;


    [Header("Projectile Specific")]
    public TowerAttribute pierce;
    public TowerAttribute damage;
    public TowerAttribute explosionRadius;
    public TowerAttribute projectileLifeTime;
    public TowerAttribute projectileSize;
    public TowerAttribute projectileSpeed;

}
