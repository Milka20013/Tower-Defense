using UnityEngine;

public class CannonBall : MonoBehaviour, ISimpleProjectile
{
    [SerializeField] private TowerAttributeContainer attributeContainer;
    [SerializeField] private LayerMask enemyLayer = 1 << 6;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    [SerializeField] private Vector3 middlePointOffset = new(0, 10f, 0);
    private Vector3 middlePoint;
    [SerializeField] private float speedOffset = 1f;
    [Header("Effects")]
    [SerializeField] private Transform effectPoint;
    [SerializeField] private GameObject trailingEffect;

    private float damage;
    private float explosionRadius;
    private float pierce;
    private float speed;

    private float time = 0f;

    private bool finished = false;

    public void Init(TowerStats towerStats)
    {
        towerStats.TryGetAttributeValue(attributeContainer.damage, out damage);
        towerStats.TryGetAttributeValue(attributeContainer.pierce, out pierce);
        towerStats.TryGetAttributeValue(attributeContainer.projectileSpeed, out speed);
        towerStats.TryGetAttributeValue(attributeContainer.explosionRadius, out explosionRadius);
    }

    public void SetTarget(Transform target)
    {
        targetPosition = target.position;
        startPosition = transform.position;
        middlePoint = (targetPosition - transform.position) / 2 + startPosition;
        middlePoint += middlePointOffset;
        if (effectPoint == null || trailingEffect == null)
        {
            return;
        }
        Instantiate(trailingEffect, effectPoint);
    }

    private Vector3 EvaluatePointOnQuadraticCurve(float time)
    {
        Vector3 ab = Vector3.Lerp(startPosition, middlePoint, time);
        Vector3 bc = Vector3.Lerp(middlePoint, targetPosition, time);
        return Vector3.Lerp(ab, bc, time);
    }
    private void Update()
    {
        if (finished)
        {
            return;
        }
        if (time >= 1f)
        {
            finished = true;
            Explode();
            return;
        }
        Vector3 vec = EvaluatePointOnQuadraticCurve(time);
        transform.position = vec;
        transform.up = EvaluatePointOnQuadraticCurve(time + 0.01f) - vec;
        time += Time.deltaTime * speed * speedOffset / 20;
    }

    private void Explode()
    {
        Collider[] colliders = new Collider[(int)pierce];
        int size = Physics.OverlapSphereNonAlloc(EvaluatePointOnQuadraticCurve(1f), explosionRadius, colliders, enemyLayer);
        for (int i = 0; i < size; i++)
        {
            if (colliders[i].TryGetComponent(out IDamageable damageable))
            {
                damageable.OnHit(damage);
            }
        }
        Destroy(gameObject);
    }
}
