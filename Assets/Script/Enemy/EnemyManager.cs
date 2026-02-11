using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : MonoBehaviour, IChangedGameMode
{
    public static EnemyManager Instance;
    //적들 담김
    public List<Enemy> enemies = new List<Enemy>(); //생성된 모든 적
    public Enemy meleeEnemeyPrefab;
    private void Awake()
    {
        Instance = this;
        GameManager.changedGameModeListener.Add(this);
    }


    //오브젝트 풀링으로 적 생성 
    public Enemy GetEnemy(EnemyKind enemyKind)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemyKind == EnemyKind.Melee && enemies[i].gameObject.activeSelf == false)
            {
                enemies[i].gameObject.SetActive(true);
                return enemies[i];
            }
        }

        Enemy enemy = Instantiate(meleeEnemeyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        enemies.Add(enemy);
        return enemy;
    }

    
    public void ChangedGameMode(GameMode gm)
    {
        if (gm == GameMode.Battle)
        {
            
        }
    }

    public Enemy FindClosestEnemy(Vector3 pos)
    {

        
        float minDistance = float.MaxValue;
        int enemyIdx = -1;


        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].gameObject.activeSelf)
                continue;
            
            float dis = Vector2.Distance(enemies[i].transform.position, pos);
            if (minDistance > dis)
            {
                minDistance = dis;
                enemyIdx = i;
            }
        }

        if (enemyIdx == -1)
        {
            return null;
        }

        return enemies[enemyIdx];
    }
}

