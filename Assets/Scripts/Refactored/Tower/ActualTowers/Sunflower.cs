using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Reloading))]
public class Sunflower : MonoBehaviour, IHarvestable
{
    private Reloading reloading;
    private TowerStats stats;
    [SerializeField] private TowerAttributeContainer attributeContainer;

    private float projectileLifeTime;
    private float tickRate;
    private float damage;

    public Gradient gradient;

    private float checkInterval;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private LineRenderer beam;
    [SerializeField] private Transform attackOrigin;

    private Transform target;

    private bool cantAttack = true;

    private void Awake()
    {
        reloading = GetComponent<Reloading>();
        stats = GetComponent<TowerStats>();
        stats.onValueChanged += GetStats;
        GetStats();
    }

    private void Start()
    {
        StartCoroutine(CheckReadiness());
    }
    private void GetStats()
    {
        stats.TryGetAttributeValue(attributeContainer.tickRate, out tickRate);
        stats.TryGetAttributeValue(attributeContainer.projectileLifeTime, out projectileLifeTime);
        stats.TryGetAttributeValue(attributeContainer.damage, out damage);
        checkInterval = 1 / 30;
    }

    public void StartAttacking(Transform target)
    {

        cantAttack = false;
        GetNextTarget(target);
        beam.gameObject.SetActive(true);

        StartCoroutine(LockOnTarget());
        StartCoroutine(DamageTarget());
        StartCoroutine(StopAttacking());
    }

    public void GetNextTarget(Transform target)
    {
        if (cantAttack)
        {
            return;
        }
        if (target == null)
        {
            beam.gameObject.SetActive(false);
            return;
        }
        beam.gameObject.SetActive(true);
        this.target = target;

    }
    IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(projectileLifeTime);
        cantAttack = true;
        beam.gameObject.SetActive(false);
        reloading.Reload();
    }

    IEnumerator LockOnTarget()
    {
        for (; ; )
        {
            if (cantAttack)
            {
                break;
            }
            if (target != null)
            {
                beam.SetPosition(0, attackOrigin.position);
                beam.SetPosition(1, target.position);
            }
            yield return null;
        }
    }

    IEnumerator DamageTarget()
    {
        for (; ; )
        {
            if (cantAttack)
            {
                break;
            }
            if (target != null)
            {
                if (target.TryGetComponent(out IDamageable damageable))
                {
                    damageable.OnHit(damage);
                }
            }
            yield return new WaitForSeconds(1 / tickRate);
        }
    }
    IEnumerator CheckReadiness()
    {
        for (; ; )
        {
            float readiness = reloading.GetReloadReadiness();
            meshRenderer.material.color = gradient.Evaluate(readiness);
            yield return new WaitForSeconds(checkInterval);
        }
    }

    public Vector3 GetHarvestPoint()
    {
        return attackOrigin.position;
    }
}
