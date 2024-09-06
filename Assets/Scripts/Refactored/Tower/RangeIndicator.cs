using System;
using System.Linq;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    [SerializeField] private GameObject rangeIndicator;
    [SerializeField] private TowerAttributeContainer attributeContainer;
    private Transform target;
    private bool followTarget;

    private void Update()
    {
        if (followTarget)
        {
            transform.position = target.position;
        }
    }
    public void ChangeRadius(object obj)
    {
        float range = GetRange(obj);
        rangeIndicator.transform.localScale = new Vector3(range, 1, range);
    }
    private float GetRange(object obj)
    {
        var bp = ConverToBlueprint(obj);
        if (bp != null)
        {
            return bp.attributes.Where(x => x.attribute == attributeContainer.range).First().value;
        }
        try
        {
            var tower = (Tower)obj;
            tower.GetComponent<TowerStats>().TryGetAttributeValue(attributeContainer.range, out float range);
            return range;
        }
        catch
        {
            Debug.LogError("The object is not a bp or tower object." + obj + " " + name);
            throw new InvalidCastException();
        }
    }
    private TowerBlueprint ConverToBlueprint(object obj)
    {
        try
        {
            var bp = (TowerBlueprint)obj;
            return bp;
        }
        catch
        {
            return null;
        }
    }
    public void FollowObject(object obj)
    {
        rangeIndicator.SetActive(true);
        target = ((GameObject)obj).transform;
        followTarget = true;
    }
    public void ShowAtPosition(object obj)
    {
        rangeIndicator.SetActive(true);
        transform.position = ((Tower)obj).transform.position;
    }
    public void Hide(object _)
    {
        target = null;
        followTarget = false;
        rangeIndicator.SetActive(false);
    }
}
