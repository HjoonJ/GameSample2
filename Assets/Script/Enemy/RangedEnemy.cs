using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RangedEnemy : Enemy
{
    // 발사 타이밍에 총을 발사하게 처리

    [Header("공격 구의 중심")]
    public Transform attackPoint; // 공격 범위할 구의 중심

    public GameObject bulletPrefab;
    

    public override void ShootingTiming()
    {
        //총알 생성
        GameObject bulletObject = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);

        //총알이 날아가는 것은 EnemyBullet 에서 적용.

        bulletObject.GetComponent<EnemyBullet>().Shoot(this.gameObject.transform.forward, attackPower);

    }

}
