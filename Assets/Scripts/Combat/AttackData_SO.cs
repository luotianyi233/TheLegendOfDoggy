using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attack",menuName ="Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attckRange;

    public float skillRange;

    public float coolDown;

    public int minDamage;

    public int maxDamage;

    public float criticalMultiplier;

    public float criticalChance;

    public void ApplyWeaponData(AttackData_SO weapon)
    {
        attckRange = weapon.attckRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;

        minDamage = weapon.minDamage;
        maxDamage = weapon.maxDamage;

        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }
}