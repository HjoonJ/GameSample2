using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyKind kind;

    public float initHp; //초기 체력

    // 카운팅 횟수에 따른 적의 능력치 상승 (밀리 - 5% 레인지드 - 3%)
    public float hpPercentUpgrade;

    public float moveSpeed;

    public float attackRange;

    public float initAttackPower;

    public float attackPercentUpgrade;

    public float attackSpeed;
}
