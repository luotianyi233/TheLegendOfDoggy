using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire
    {
        public string name;
        public int requireAmount;
        public int currentAmount;
    }

    public string questName;
    [TextArea]

    public string description;

    public bool isStarted;
    public bool isCompleted;
    public bool isFinished;

    public List<QuestRequire> questRequires = new List<QuestRequire>();
    public List<InventoryItem> rewards = new List<InventoryItem>();

    public void CheckQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount);
        isCompleted = finishRequires.Count() == questRequires.Count;

        if (isCompleted)
        {
            Debug.Log("任务完成");
        }
    }

    public void GiveRewards()
    {
        foreach (var reward in rewards)
        {
            if (reward.amount < 0)//需要上交任务物品的情况
            {
                int requireCount = Mathf.Abs(reward.amount);

                if (InventoryManager.Instance.QuestItemInBag(reward.itemData) != null)//背包当中有需要交的物品
                {
                    if (InventoryManager.Instance.QuestItemInBag(reward.itemData).amount <= requireCount)//背包当中需要上交物品的数量刚好够或者不够的情况
                    {
                        requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemData).amount;
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount = 0;

                        if (InventoryManager.Instance.QuestItemInAction(reward.itemData) != null)
                            InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                    }
                    else//背包当中上交物品的数量充足
                    {
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount -= requireCount;
                    }
                }
                else//背包当中没有上交物品代表Action中一定满足了任务物品的数量
                {
                    InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                }
            }
            else//正常获得的额外物品奖励添加到背包中
            {
                InventoryManager.Instance.inventoryData.AddItem(reward.itemData, reward.amount);
            }

            InventoryManager.Instance.inventoryUI.RefreshUI();
            InventoryManager.Instance.actionUI.RefreshUI();
        }
    }

    //当前任务需要收集/消灭的目标名字列表
    public List<string> RequireTargetName()
    {
        List<string> targetNameList = new List<string>();

        foreach(var require in questRequires)
        {
            targetNameList.Add(require.name);
        }

        return targetNameList;
    }
}
