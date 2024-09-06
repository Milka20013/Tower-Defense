using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerStatInfoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject statInfoPanel;
    [SerializeField] private Transform statInfosContainer;
    [SerializeField] private GameObject statInfoPrefab;
    private Tower tower;
    private TowerStats stats;
    public void OnTowerSelected(object towerObj)
    {
        tower = (Tower)towerObj;
        stats = tower.GetComponent<TowerStats>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        PopulateContainer();
        statInfoPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statInfoPanel.SetActive(false);
    }

    private void PopulateContainer()
    {
        int childCountDifference = tower.blueprint.attributes.Length - statInfosContainer.childCount;
        List<GameObject> objectsOnContainer = new();
        for (int i = 0; i < statInfosContainer.childCount; i++)
        {
            var child = statInfosContainer.GetChild(i).gameObject;
            objectsOnContainer.Add(child);
            child.SetActive(false);
        }
        for (int i = 0; i < childCountDifference; i++)
        {
            var obj = Instantiate(statInfoPrefab, statInfosContainer);
            objectsOnContainer.Add(obj);
            obj.SetActive(false);
        }
        for (int i = 0; i < tower.blueprint.attributes.Length; i++)
        {
            SetStatInfoToPrefab(objectsOnContainer[i], tower.blueprint.attributes[i].attribute);
            objectsOnContainer[i].SetActive(true);
        }
    }

    private void SetStatInfoToPrefab(GameObject statInfoObject, TowerAttribute attribute)
    {
        statInfoObject.GetComponentInChildren<Image>().sprite = attribute.icon;
        stats.TryGetAttributeValue(attribute, out float value);
        statInfoObject.GetComponentInChildren<TextMeshProUGUI>().text = Math.Round(value, 2).ToString();

    }
}
