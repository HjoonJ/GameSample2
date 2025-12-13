using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Enemy enemyPrefab;
    public float spawnTime;
    
    public int enemyCounting = 0;
    public int spawnCount;
   
    
    public void SpawnEnemy()
    {
        enemyCounting = 0;
        StartCoroutine(CoSpawn());
    }

    IEnumerator CoSpawn()
    {
        while (true)
        {
            Instantiate(enemyPrefab,transform.position, Quaternion.identity);
            enemyCounting++;
            
            GameManager.Instance.liveEnemyCount++;
            GameManager.Instance.spawnEnemyCount++;

            if (enemyCounting == spawnCount)
            {
                break;
            }
            
            yield return new WaitForSeconds(spawnTime);



            //Âü°í¿ë
            //if (enemyPrefab.kind == EnemyKind.Melee)
            //    yield return new WaitForSeconds(2f);
            //else if (enemyPrefab.kind == EnemyKind.Ranged)
            //    yield return new WaitForSeconds(3f);


        }
        

    }

}
