using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TowerStats))]
public class Reloading : MonoBehaviour
{
    [SerializeField] private TowerAttributeContainer attributeContainer;
    private float attackSpeed;
    private float attackDelay;

    private TowerStats stats;

    private bool readyToShoot = false;
    [SerializeField] private UnityEvent onReadyToShoot;

    private void Start()
    {
        stats = GetComponent<TowerStats>();

        stats.onValueChanged += GetAttackSpeedValue;

        GetAttackSpeedValue();
        attackDelay = 0.5f / attackSpeed;
    }

    private void Update()
    {
        attackDelay -= Time.deltaTime;
        if (attackDelay <= 0 && !readyToShoot)
        {
            readyToShoot = true;
            onReadyToShoot.Invoke();
        }
    }

    public void Reload()
    {
        readyToShoot = false;
        attackDelay = 1 / attackSpeed;
    }
    public void GetAttackSpeedValue()
    {
        stats.TryGetAttributeValue(attributeContainer.attackSpeed, out attackSpeed);
    }

    public float GetReloadReadiness()
    {
        if (readyToShoot)
        {
            return 1f;
        }
        return 1f - attackDelay * attackSpeed;
    }
    private void OnDisable()
    {
        stats.onValueChanged -= GetAttackSpeedValue;
    }
}
