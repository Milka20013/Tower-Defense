using UnityEngine;
using UnityEngine.Splines;

public class EnemyMovement : MonoBehaviour
{
    private Enemy enemy;
    private EnemyStats stats;
    [SerializeField] private EnemyAttributeContainer attributeContainer;
    private float currentSpeed;
    [HideInInspector] public float distanceTravelled = 0f;
    private Spline spline;
    private float distanceRatio;
    private float splineLength;
    [SerializeField] private float positionYOffset = 0.5f;
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        stats = GetComponent<EnemyStats>();
        OnStatChange();
        spline = Path.instance.splineContainer.Spline;
        splineLength = spline.GetLength();
    }
    private void Update()
    {
        if (enemy.isDead)
        {
            return;
        }
        distanceRatio += currentSpeed * Time.deltaTime / splineLength;

        Vector3 currentPosition = spline.EvaluatePosition(distanceRatio);
        transform.position = new Vector3(currentPosition.x, currentPosition.y + positionYOffset, currentPosition.z);

        distanceTravelled += currentSpeed * Time.deltaTime;

        if (distanceRatio >= 1f)
        {
            enemy.ReachEnd();
            return;
        }
        Vector3 nextPosition = spline.EvaluatePosition(distanceRatio + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);

    }

    public void OnStatChange()
    {
        stats.TryGetAttributeValue(attributeContainer.speed, out currentSpeed);
    }
}
