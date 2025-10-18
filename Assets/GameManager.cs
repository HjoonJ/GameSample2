using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //5초마다 게임모드 전환하기
    public GameMode gameMode ;
    public bool inBattleMode = false;
    public float duration;
    //옵저버 패턴
    public static List<IChangedGameMode> changedGameModeListener = new List<IChangedGameMode>();


    private void Start()
    {
        SetMode(GameMode.Normal);

        StartCoroutine(ChangingGameMode());



    }

    IEnumerator ChangingGameMode()
    {
        while (true)
        {
            // 5초 대기 후 배틀 모드로 전환

            yield return new WaitForSeconds(duration);
            SetMode(GameMode.Battle);
            inBattleMode = true;

            // 전투 중, 적이 모두 사라질 때까지 대기
            yield return StartCoroutine(CheckEnemiesCleared());

            // 다시 노말 모드로 전환
            SetMode(GameMode.Normal);
            inBattleMode = false;

        }
        
    }


    // 적을 다 소멸시켰는지 확인하는 코드
    IEnumerator CheckEnemiesCleared()
    {
        while (true) 
        {


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
