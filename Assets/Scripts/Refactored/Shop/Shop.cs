using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private TowerBlueprint[] towerBlueprints;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject panel;

    private void OnEnable()
    {
        for (int i = 0; i < towerBlueprints.Length; i++)
        {
            var obj = Instantiate(shopItemPrefab, container);
            obj.GetComponent<ShopItem>().towerBlueprint = towerBlueprints[i];
        }
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
