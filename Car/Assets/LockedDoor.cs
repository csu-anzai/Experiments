using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Interactable
{
    public InventoryManager inven;
    public bool onLock = true;
    public List<Transform> jails;

    public override void Interact()
    {
        var invenList = inven.inventory;
        if (invenList != null)
        {
            foreach (var item in invenList)
            {
                if (item.tag == "Key")
                {
                    OnOff(false);
                    inven.RemoveItem(item);
                    break;
                }
            }
        }
    }

    void OnOff(bool status)
    {
        foreach (var jail in jails)
        {
            jail.GetComponent<SpriteRenderer>().enabled = status;
            jail.GetComponent<BoxCollider2D>().enabled = status;
        }
    }
}
