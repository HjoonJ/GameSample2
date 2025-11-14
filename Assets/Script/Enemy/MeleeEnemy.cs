using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("공격 구의 중심")]
    public Transform attackPoint; // 공격 범위할 구의 중심
    [Header("공격 구의 반지름")]
    public float attackLength; // 구의 반지름
    public override void Attack()
    {
        base.Attack();

        
        // Attack 재정의하기
        
        // 부딪힌 모든 게임오브젝트가 배열로 반환이 됨 => 건물 or 캐릭터만 뽑아서 선택되어야함 (Tag)
        
        Collider[] targets = Physics.OverlapSphere(attackPoint.position, attackLength);
        for (int i = 0; i < targets.Length; i++)
        { 
            IEnemyTarget target = targets[i].GetComponent<IEnemyTarget>();
            if (target== null)
            {
                continue;
            }

            target.TakeDamage(10);
        }

        SetState(EnemyState.Idle);
    }

    
}
