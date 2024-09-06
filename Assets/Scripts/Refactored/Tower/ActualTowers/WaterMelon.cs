using System.Collections;
using UnityEngine;

public class WaterMelon : MonoBehaviour
{
    [SerializeField] private GameObject melon;
    private Reloading reloading;
    private float checkInterval = 1 / 30;

    private void Awake()
    {
        reloading = GetComponent<Reloading>();
    }

    private void Start()
    {
        StartCoroutine(GrowMelon());
    }

    IEnumerator GrowMelon()
    {
        for (; ; )
        {
            float readiness = reloading.GetReloadReadiness();
            float scale = 0.1f + readiness;
            melon.transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForSeconds(checkInterval);
        }
    }
}
