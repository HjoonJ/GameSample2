using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Enemy enemyPrefab;
    public float spawnTime;
    
    public int enemyCounting = 0;
    
    //적이 몇명까지 생성될지
    public int spawnCount;
   
    
    //적이 소환되기 시작할때
    public void StartSpawnEnemy()
    {
        enemyCounting = 0;
        StartCoroutine(CoSpawn());
    }

    IEnumerator CoSpawn()
    {
        while (true)
        {
            if (enemyCounting == spawnCount)
            {
                break;
            }

            Enemy enemy = EnemyManager.Instance.GetEnemy(EnemyKind.Melee);
            enemy.Spawn();
            enemy.gameObject.transform.position = this.transform.position;
            //Enemy enemy = Instantiate(enemyPrefab,transform.position, Quaternion.identity);
            //EnemyManager.Instance.enemies.Add(enemy);
            enemyCounting++;
            
            GameManager.Instance.liveEnemyCount++;
            GameManager.Instance.spawnEnemyCount++;

            yield return new WaitForSeconds(spawnTime);



            //참고용
            //if (enemyPrefab.kind == EnemyKind.Melee)
            //    yield return new WaitForSeconds(2f);
            //else if (enemyPrefab.kind == EnemyKind.Ranged)
            //    yield return new WaitForSeconds(3f);


        }
        

    }

}
