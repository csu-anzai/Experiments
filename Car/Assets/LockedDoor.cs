using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Interactable
{
    public InventoryManager inven;
    public bool onLock = true;

    SpriteRenderer sp;
    BoxCollider2D col;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

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
        sp.enabled = status;
        col.enabled = status;
    }
}
