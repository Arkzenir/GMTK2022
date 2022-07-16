using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;

    private List<InventoryItem> UIItemsList = new List<InventoryItem>();

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent((contentPanel));
            UIItemsList.Add(uiItem);
            uiItem.OnLMB += HandleItemSelection;
            uiItem.OnRMB += HandleShowItemActions;
        }
    }

    private void HandleItemSelection(InventoryItem obj)
    {
        Debug.Log(obj.name);
    }

    private void HandleShowItemActions(InventoryItem obj)
    {
        Debug.Log(obj.name);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
