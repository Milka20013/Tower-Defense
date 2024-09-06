using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Blueprint")]
public class EnemyBlueprint : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public EnemyAttributeSelector[] attributes;


    //for editor only
    public float GetRelativeStrength()
    {
        var health = attributes.Where(x => x.attribute.attributeName == "Health").First().value;
        var speed = attributes.Where(x => x.attribute.attributeName == "Speed").First().value;
        return health * Mathf.Sqrt(speed + 1);
    }
}
