using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Reloading))]
public class BerryBasket : MonoBehaviour
{
    private Reloading reloading;
    [SerializeField] private GameObject berry;
    [SerializeField] private TowerAttributeContainer attributeContainer;
    private TowerStats stats;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float scaleMultiplier = 1;

    private float damage;
    private float pierce;
    private float range;

    private readonly float checkInterval = 1 / 30;
    private void Awake()
    {
        stats = GetComponent<TowerStats>();
        reloading = GetComponent<Reloading>();
    }

    private void Start()
    {
        stats.onValueChanged += UpdateStats;
        UpdateStats();
        StartCoroutine(CheckReadiness());
    }

    private void UpdateStats()
    {
        stats.TryGetAttributeValue(attributeContainer.damage, out damage);
        stats.TryGetAttributeValue(attributeContainer.pierce, out pierce);
        stats.TryGetAttributeValue(attributeContainer.range, out range);
    }
    public void Explode()
    {
        Collider[] colliders = new Collider[(int)pierce];
        int size = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, enemyLayer);
        var effect = Instantiate(explosionEffect, berry.transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        for (int i = 0; i < size; i++)
        {
            if (colliders[i].TryGetComponent(out IDamageable damageable))
            {
                damageable.OnHit(damage);
            }
        }
    }

    IEnumerator CheckReadiness()
    {
        for (; ; )
        {
            float readiness = reloading.GetReloadReadiness();
            float scale = 0.1f + readiness * scaleMultiplier;
            berry.transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForSeconds(checkInterval);
        }
    }

}
