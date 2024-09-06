using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform statInfosContainer;
    [SerializeField] private GameObject statInfoPrefab;
    [SerializeField] private TextMeshProUGUI currentHealthText;

    private Enemy enemy;
    private EnemyStats stats;
    private TakeDamage takeDamage;
    public void OnEnemySelected(object enemyObj)
    {
        enemy = (Enemy)enemyObj;
        stats = enemy.GetComponent<EnemyStats>();
        takeDamage = enemy.GetComponent<TakeDamage>();

        takeDamage.onHealthChanged += RefreshCurrentHealthText;

        PopulateContainer();
        RefreshCurrentHealthText(enemy.GetCurrentHealth());
        Open();
    }

    public void OnEnemyKilled(object enemyObj)
    {
        if ((Enemy)enemyObj == enemy)
        {
            Close();
        }
    }

    public void Open()
    {
        panel.SetActive(true);
    }
    public void Close()
    {
        if (takeDamage != null)
        {
            takeDamage.onHealthChanged -= RefreshCurrentHealthText;
        }
        panel.SetActive(false);
    }

    private void RefreshCurrentHealthText(float newHealth)
    {
        currentHealthText.text = Math.Round(newHealth, 2).ToString();
    }
    private void PopulateContainer()
    {
        int childCountDifference = enemy.blueprint.attributes.Length - statInfosContainer.childCount;
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
        for (int i = 0; i < enemy.blueprint.attributes.Length; i++)
        {
            SetStatInfoToPrefab(objectsOnContainer[i], enemy.blueprint.attributes[i].attribute);
            objectsOnContainer[i].SetActive(true);
        }
    }

    private void SetStatInfoToPrefab(GameObject statInfoObject, EnemyAttribute attribute)
    {
        statInfoObject.GetComponentInChildren<Image>().sprite = attribute.icon;
        stats.TryGetAttributeValue(attribute, out float value);
        statInfoObject.GetComponentInChildren<TextMeshProUGUI>().text = Math.Round(value, 2).ToString();

    }

    private void OnDestroy()
    {
        if (takeDamage != null)
        {
            takeDamage.onHealthChanged -= RefreshCurrentHealthText;
        }
    }
}
