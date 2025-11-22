using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAnimEvent : MonoBehaviour
{
    public Action startAttackListener;
    public Action endAttackListener;
    public Action shootingTimingListener;
    
    //Invoke 는 함수 실행을 위해 사용.
    public void StartAttack()
    {
        Debug.Log("EnemyAnimEvent StartAttack()");
        //여기이 Invoke는 Action 아래 있는 친구.
        startAttackListener.Invoke(); // MonoBehaviour 의 Invoke 와 아에 기능이 다른 친구(함수실행).
    }

    public void EndAttack()
    {
        Debug.Log("EnemyAnimEvent EndAttack()");
        endAttackListener.Invoke();
    }

    public void ShootingTiming()
    {
        Debug.Log("EnemyAnimEvent ShootingTiming()");
        shootingTimingListener.Invoke();
    }

}
