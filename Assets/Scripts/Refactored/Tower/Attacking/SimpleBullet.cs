using UnityEngine;

public class SimpleBullet : MonoBehaviour, ISimpleProjectile
{
    [SerializeField] private TowerAttributeContainer attributeContainer;

    private Vector3 travelDirection;

    private float damage;

    private float pierce;
    private float currentPierce;

    private float initialSpeed;
    private float speed;

    private float lifeTime;
    private float currentLifeTime;

    private bool isDead;

    public void Init(TowerStats towerStats)
    {
        towerStats.TryGetAttributeValue(attributeContainer.damage, out damage);
        towerStats.TryGetAttributeValue(attributeContainer.pierce, out pierce);
        towerStats.TryGetAttributeValue(attributeContainer.projectileSpeed, out initialSpeed);
        initialSpeed *= 1.5f;
        speed = initialSpeed;
        towerStats.TryGetAttributeValue(attributeContainer.projectileLifeTime, out lifeTime);

        //might not need
        currentLifeTime = lifeTime;
        currentPierce = pierce;
    }

    public void SetTarget(Transform target)
    {
        travelDirection = (target.position - transform.position).normalized;
    }

    private void Update()
    {
        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0 && !isDead)
        {
            isDead = true;
            Destroy(gameObject);
            return;
        }
        if (!isDead)
        {
            //this is necessary, cuz colliding destroys the object
            float distanceThisFrame = speed * Time.deltaTime;
            DecreaseSpeed();
            transform.Translate(travelDirection.normalized * distanceThisFrame, Space.World);
        }
    }

    private void DecreaseSpeed()
    {
        float treshold = initialSpeed / 1.5f;
        if (speed <= treshold)
        {
            speed = treshold;
            return;
        }
        speed -= 1f;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isDead)
        {
            return;
        }
        if (collision.transform.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.OnHit(damage);
            currentPierce--;
            if (Mathf.FloorToInt(currentPierce) <= 0)
            {
                isDead = true;
                Destroy(gameObject);
                return;
            }
        }
    }
}



