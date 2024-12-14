using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType { BAG,WEAPON,ARMOR,ACTION}

public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem() != null)
            if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount > 0)
            {
               GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableItem.healthPoint);
               GameManager.Instance.playerStats.ApplyAttack(itemUI.GetItem().useableItem.attackPoint);
               GameManager.Instance.playerStats.ApplyDefense(itemUI.GetItem().useableItem.defensePoint);

                if(itemUI.GetItem().useableItem.portalName != null)
                {
                    GameObject root = GameObject.Find("Portals");
                    GameObject portal=root.transform.Find(itemUI.GetItem().useableItem.portalName).gameObject;
                    if(portal!=null)
                    {
                        portal.SetActive(true);
                    }
                }

                itemUI.Bag.items[itemUI.Index].amount-=1;

                QuestManager.Instance.UpdateQuestProgress(itemUI.GetItem().itemName, -1);
            }
         UpdateItem();
    }

    public void UpdateItem()
    {
        switch(slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                //װ��(�л�)����
                if (itemUI.Bag.items[itemUI.Index].itemData != null)
                {
                    GameManager.Instance.playerStats.ChangeWeapon(itemUI.Bag.items[itemUI.Index].itemData);
                }
                else
                {
                    GameManager.Instance.playerStats.UnEquipWeapon();
                }
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
        }

        var item = itemUI.Bag.items[itemUI.Index];

        itemUI.SetupItemUI(item.itemData, item.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemUI.GetItem())
        {
            InventoryManager.Instance.toolTip.SetupToolTip(itemUI.GetItem());
            InventoryManager.Instance.toolTip.gameObject.SetActive(true);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }
}
