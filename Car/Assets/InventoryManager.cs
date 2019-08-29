using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Transform> inventory;

    public void AddItem(Transform item)
    {
        if (!inventory.Contains(item))
        {
            inventory.Add(item);
        }
    }

    public void RemoveItem(Transform item)
    {
        inventory.Remove(item);
        item.gameObject.SetActive(false);
    }

    public void ItemFollowing(Transform target)
    {
        if (inventory != null)
        {
            foreach (var item in inventory)
            {
                item.position = Vector2.Lerp(item.position, target.position, Time.deltaTime * 5);
            }
        }
    }
}
