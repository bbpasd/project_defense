using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using UnityEngine;

public abstract class Object_Base : MonoBehaviour
{
    // 각종 스탯
    public string name;
    public float currentHP;
    public float maxHP;
    public float attackPower = 10.0f;
    
    // 비용
    public int upgradeCost = 1;
    public int healCost = 1;
    
    // 업그레이드
    public int upgradeLevel = 0;
    
    public virtual void OnAttack(float amount) {
        if (this == MyPlayerController.Instance.selectedObject) {
            MyPlayerController.Instance.UpdateSelectObjectData();
        }
    }
    
    public virtual void Death() {
    }

    public virtual void Heal(float percentage) {
        float healAmount = maxHP * percentage / 100;
        currentHP = Mathf.Min(currentHP + healAmount, maxHP);
        
        if (this == MyPlayerController.Instance.selectedObject) {
            MyPlayerController.Instance.UpdateSelectObjectData(UpdateAnim.Heal);
        }

        if (this == MyPlayerController.Instance.hqBuilding) {
            MyPlayerController.Instance.UpdateHQ(UpdateAnim.Heal);
        }
    }
}
