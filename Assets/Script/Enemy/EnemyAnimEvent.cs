using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAnimEvent : MonoBehaviour
{
    public Action startAttackListener;
    public Action endAttackListener;
    public void StartAttack()
    {
        Debug.Log("EnemyAnimEvent StartAttack()");
        startAttackListener.Invoke();
    }

    public void EndAttack()
    {
        Debug.Log("EnemyAnimEvent EndAttack()");
        endAttackListener.Invoke();
    }
}
