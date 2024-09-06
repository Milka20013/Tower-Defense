using System;
using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviour, ISimpleProjectile
{
    [SerializeField] private GameObject hitEffect;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private TowerAttributeContainer attributeContainer;

    private float damage;
    private int pierce;
    private float lifeTime;
    private float tickRate;
    private float hitAreaSize;

    private IEnumerator HitByTickRate()
    {
        Collider[] colliders = new Collider[pierce];
        int numberOfEnemies = Physics.OverlapSphereNonAlloc(transform.position, hitAreaSize, colliders, enemyLayer);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            colliders[i].GetComponent<IDamageable>().OnHit(damage);
        }
        if (numberOfEnemies > 0)
        {
            var effect = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(effect, 1f);
        }
        yield return new WaitForSeconds(tickRate);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, hitAreaSize);
    }

    public void Init(TowerStats towerStats)
    {
        towerStats.TryGetAttributeValue(attributeContainer.damage, out damage);
        towerStats.TryGetAttributeValue(attributeContainer.pierce, out float pierceF);
        pierce = Convert.ToInt32(pierceF);
        towerStats.TryGetAttributeValue(attributeContainer.projectileLifeTime, out lifeTime);
        towerStats.TryGetAttributeValue(attributeContainer.tickRate, out tickRate);
        tickRate = 1 / tickRate;
        towerStats.TryGetAttributeValue(attributeContainer.projectileSize, out hitAreaSize);
    }

    public void SetTarget(Transform target)
    {
        Vector3 position = target.position;
        transform.position = new Vector3(position.x, position.y - 0.5f, position.z);
        StartCoroutine(HitByTickRate());
        Destroy(gameObject, lifeTime);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

