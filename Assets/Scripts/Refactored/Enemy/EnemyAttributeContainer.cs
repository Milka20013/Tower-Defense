using UnityEngine;

//[CreateAssetMenu(menuName = "Enemy/AttributeContainer")]
//This class should be used as an enum.
//Only one instance should exist
public class EnemyAttributeContainer : ScriptableObject
{
    public EnemyAttribute speed;
    public EnemyAttribute bounty;
    public EnemyAttribute damageToPlayer;
    public EnemyAttribute health;
    public EnemyAttribute rank;
    public EnemyAttribute luck;
    public EnemyAttribute fateThreshold;

}
