using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO:����Ʒ��ӵ�����
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //װ������
            //GameManager.Instance.playerStats.EquipWeapon(itemData);

            //����Ƿ�������
            QuestManager.Instance.UpdateQuestProgress(itemData.itemName, itemData.itemAmount);

            Destroy(gameObject); ;
        }
    }
}
