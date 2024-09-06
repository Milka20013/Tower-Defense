using UnityEngine;

public class ArealProjectile : MonoBehaviour, ISimpleProjectile
{
    [SerializeField] private GameObject effect;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private TowerAttributeContainer attributeContainer;

    private float damage;
    private int pierce;
    private float auraSize;
    private float lifeTime;

    public void Init(TowerStats towerStats)
    {
        Instantiate(effect, transform.position, transform.rotation);

        towerStats.TryGetAttributeValue(attributeContainer.damage, out damage);
        towerStats.TryGetAttributeValue(attributeContainer.pierce, out float pierceF);
        pierce = Mathf.RoundToInt(pierceF);
        towerStats.TryGetAttributeValue(attributeContainer.projectileSize, out auraSize);
        towerStats.TryGetAttributeValue(attributeContainer.projectileLifeTime, out lifeTime);


        Hit();
    }

    private void Hit()
    {
        if (effect != null)
        {
            var effectObj = Instantiate(effect, transform.position, transform.rotation);
            Destroy(effectObj, lifeTime);
        }
        Collider[] colliders = new Collider[pierce];
        int numberOfColliders = Physics.OverlapSphereNonAlloc(transform.position, auraSize, colliders, enemyLayer);
        for (int i = 0; i < numberOfColliders; i++)
        {
            colliders[i].GetComponent<IDamageable>().OnHit(damage);
        }
        Destroy(gameObject);
    }
    public void SetTarget(Transform _)
    {
    }
}
