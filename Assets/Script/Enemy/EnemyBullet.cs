using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // 날아가야되는 방향을 알아야되고 -> 그 방향으로 이동시키는 코드가 필요.

    public Vector3 direction;
    public float moveSpeed;
    public float attackDamage;
    
    public void Shoot(Vector3 dir, float damage)
    {
        direction = dir;
        attackDamage = damage;
    }


    private void Update()
    {
        //설정된 방향으로 이동하기
        transform.position = transform.position + direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") || other.CompareTag("Building"))
        {

        }

        IEnemyTarget enemyTarget = other.GetComponent<IEnemyTarget>();
        if (enemyTarget == null)
        {
            return;
        }
        enemyTarget.TakeDamage(attackDamage);
    }


    // 총알이 타겟에 부딪혔을 때 타겟에게 데미지를 입히도록. 

    // 총알이 타겟에 부딪히면 사라짐.

}

// 객체 A에서 객체 B로 C라는 데이터를 전달하고자 함.
// RangedEnemy 에서 EnemyBullet으로 AttackPower 를 전달

// 1.  스타트 함수에서 GetComponent => 사실상 한 객체 안에서 움직일 때 쓸수 있음
// 2.  FindObjectOfType => 서로 다른 객체에서 가져올 수 있지만 한개만 있을 때 사용하면 좋음
// 3.  RangedEnemy 안에서 Bullet 이라는 객체를 생성하면서 거기 딸려오는 컨포넌트를 활용해서 데이터를 가져옴.

// 3-1 받고자 하는 변수 선언
// 3-2 