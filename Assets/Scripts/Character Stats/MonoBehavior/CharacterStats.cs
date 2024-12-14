using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int,int> UpdateHealthBarOnAttack;

    public CharacterData_SO templateData;

    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    private AttackData_SO baseAttackData;

    private RuntimeAnimatorController baseAnimator;

    [Header("Weapon")]
    public Transform weaponSlot;

    [HideInInspector]

    public bool isCritical;

    private void Awake()
    {
        if(templateData != null)
        {
            characterData = Instantiate(templateData);
        }
        baseAttackData = Instantiate(attackData);
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }

    #region Read from CharacterData_SO
    public int MaxHealth 
    { 
        get
        {
            if(characterData != null)
                return characterData.maxHealth;
            else
                return 0;
        }
        set
        {
            if (characterData != null)
                characterData.maxHealth = value;
        }
    }

    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
                return characterData.currentHealth;
            else
                return 0;
        }
        set
        {
            if (characterData != null)
                characterData.currentHealth = value;
        }
    }

    public int BaseDenfense
    {
        get
        {
            if (characterData != null)
                return characterData.baseDefence;
            else
                return 0;
        }
        set
        {
            if (characterData != null)
                characterData.baseDefence = value;
        }
    }

    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
                return characterData.currentDefence;
            else
                return 0;
        }
        set
        {
            if (characterData != null)
                characterData.currentDefence = value;
        }
    }
    #endregion

    #region Character Combat
    public void TakeDamage(CharacterStats attacker,CharacterStats defener)
    {
        int damage=Mathf.Max(attacker.CurrentDamage()-defener.CurrentDefence,0);
        CurrentHealth = Mathf.Max(CurrentHealth-damage,0);

        if(attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }

        //update UI
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //update exp
        if(CurrentHealth<=0)
            attacker.characterData.UpdateExp(characterData.killCount);
    }

    public void TakeDamage(int damage,CharacterStats defener)
    {
        int currentDamage = Mathf.Max(damage - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        if(CurrentHealth<=0)
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killCount);
    }

    private int CurrentDamage()
    {
        float coreDamage=UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("Critical Hit!"+coreDamage);
        }

        return (int)coreDamage;
    }
    #endregion

    #region Equip Weapon
    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }

    public void EquipWeapon(ItemData_SO weapon)
    {
        if(weapon.weaponPrefab!= null)
        {
            Instantiate(weapon.weaponPrefab, weaponSlot);
        }

        //TODO:更新属性
        //TODO:切换动画
        attackData.ApplyWeaponData(weapon.weaponData);
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
        //InventoryManager.Instance.UpdateStatsText(MaxHealth, attackData.minDamage, attackData.maxDamage);

    }

    public void UnEquipWeapon()
    {
        if(weaponSlot.transform.childCount!=0)
        {
            for(int i=0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }
        attackData.ApplyWeaponData(baseAttackData);
        //TODO:切换动画
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
    }
    #endregion

    #region Apply Data Change
    public void ApplyHealth(int amount)
    {
        if(CurrentHealth+amount<=MaxHealth)
            CurrentHealth += amount;
        else
            CurrentHealth = MaxHealth;
    }

    public void ApplyAttack(int amount)
    {
        baseAttackData.minDamage += amount;
        baseAttackData.maxDamage += amount;  
    }

    public void ApplyDefense(int amount)
    {
        characterData.currentDefence += amount;
    }
    #endregion
}
