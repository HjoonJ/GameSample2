using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyTarget
{
   Transform Transform //프로퍼티 o 함수 x 
    {
        get; 
    }

    void TakeDamage(float damage);
}
