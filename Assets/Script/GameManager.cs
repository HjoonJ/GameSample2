using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //날짜 카운팅 (전투에서 노말로 전환될 때마다)
    //카운팅 횟수에 따른 적의 능력치 증가 (Hp- 100부터 근거리는  10%, 원거리는 5%)


    public static GameManager Instance;
    //5초마다 게임모드 전환하기
    public GameMode gameMode ;
    public bool inBattleMode = false;
    public float duration;
    //옵저버 패턴
    public static List<IChangedGameMode> changedGameModeListener = new List<IChangedGameMode>();

    public GameObject meleePortalPrefab;
    public GameObject rangedPortalPrefab;

    //실시간 적 카운팅
    public int liveEnemyCount;

    //생성 적 카운팅 (ex. 어느 순간 15에 도달할 예정)
    public int spawnEnemyCount;

    public int enemySum;

    // 날짜 카운팅
    public int date;

    public void Awake()
    {
        Instance = this;
        // 근거리 적을 소환하는 포탈은 2초마다 소환
        // 원거리 적을 소환하는 포탈은 3초마다 소환
    }

    // 포탈에서 적들이 생성되게 처리
    // 생성된 적 카운팅

    
    private void Start()
    {
        date = 0;

        SetMode(GameMode.Normal);

        StartCoroutine(ChangingGameMode());



    }

    private void Update()
    {
        



    }

    IEnumerator ChangingGameMode()
    {
        while (true)
        {
            // 5초 대기 후 배틀 모드로 전환

            yield return new WaitForSeconds(duration);
            SetMode(GameMode.Battle);

            inBattleMode = true;
            EnemySpawn();
            // 전투 중, 적이 모두 사라질 때까지 대기
            yield return StartCoroutine(CheckEnemiesCleared());

            // 다시 노말 모드로 전환
            SetMode(GameMode.Normal);
            date++;
            inBattleMode = false;

        }
        
    }

    public void EnemySpawn()
    {
        //float randumNumber = Random.Range(0f, 4f);
        Vector3 randomPos = new Vector3 (Random.Range(0f, 4f), 0, Random.Range(0f, 4f));

        GameObject meleePortal = Instantiate(meleePortalPrefab, randomPos, Quaternion.identity);

        //randumNumber = Random.Range(0f, 4f);
        randomPos = new Vector3(Random.Range(0f, 4f), 0, Random.Range(0f, 4f));

        GameObject rangedPortal = Instantiate(rangedPortalPrefab, randomPos, Quaternion.identity);

        meleePortal.GetComponent<Portal>().StartSpawnEnemy();
        rangedPortal.GetComponent<Portal>().StartSpawnEnemy();

        enemySum = 0;
        int meleeEnemySum = meleePortal.GetComponent<Portal>().spawnCount;
        int rangedEnemySum = rangedPortal.GetComponent<Portal>().spawnCount;
        enemySum = meleeEnemySum + rangedEnemySum;

    }


    // 적을 다 소멸시켰는지 확인하는 코드
    IEnumerator CheckEnemiesCleared()
    {
        while (true) 
        {
            // 적이 모두 생성될 때까지 기다리다가 다 생성이 된 그 시점에 체크 
            // 그 때 절반보다 적으면 노말 모드로 전환
            if (spawnEnemyCount == enemySum)
            {
                if (liveEnemyCount <= enemySum / 2)
                {
                    break;
                }

            }

            yield return null;
        }


    }


    public void SetMode(GameMode gm)
    {
        gameMode = gm;

        for (int i = 0; i < changedGameModeListener.Count; i++) 
        {
            changedGameModeListener[i].ChangedGameMode(gameMode);
        }

    }


}

public enum GameMode
{
    Normal,
    Battle
}

public interface IChangedGameMode
{
    void ChangedGameMode(GameMode gameMode);// 함수 선언
}
