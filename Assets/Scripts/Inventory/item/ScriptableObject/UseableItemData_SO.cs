using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Useable Item", menuName = "Inventory/Useable Item")]
public class UseableItemData_SO : ScriptableObject
{
    public int healthPoint;
    public int attackPoint;
    public int defensePoint;
    public int criticalPoint;

    public string portalName;
}
