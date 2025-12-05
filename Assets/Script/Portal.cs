using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Enemy enemyPrefab;
    public float spawnTime;
   
    
    public void SpawnEnemy()
    {  
        StartCoroutine(CoSpawn());
    }

    IEnumerator CoSpawn()
    {
        while (true)
        {
            Instantiate(enemyPrefab);
            GameManager.Instance.enemyCount++;
            yield return new WaitForSeconds(spawnTime);




            if (enemyPrefab.kind == EnemyKind.Melee)
                yield return new WaitForSeconds(2f);
            else if (enemyPrefab.kind == EnemyKind.Ranged)
                yield return new WaitForSeconds(3f);


        }
        

    }

}
