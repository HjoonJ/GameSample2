using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        Debug.Log($"Player HP: {currentHP}");
        if (currentHP <= 0)
        {
            currentHP = 0;
            // 필요시 여기에 사망 처리 (비활성화 등)
            Destroy(gameObject);
            Debug.Log("Player Dead");
        }
    }
}
