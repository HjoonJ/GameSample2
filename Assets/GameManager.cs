using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //5�ʸ��� ���Ӹ�� ��ȯ�ϱ�
    public GameMode gameMode ;
    public bool inBattleMode = false;
    public float duration;
    //������ ����
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
            // 5�� ��� �� ��Ʋ ���� ��ȯ

            yield return new WaitForSeconds(duration);
            SetMode(GameMode.Battle);
            inBattleMode = true;

            // ���� ��, ���� ��� ����� ������ ���
            yield return StartCoroutine(CheckEnemiesCleared());

            // �ٽ� �븻 ���� ��ȯ
            SetMode(GameMode.Normal);
            inBattleMode = false;

        }
        
    }


    // ���� �� �Ҹ���״��� Ȯ���ϴ� �ڵ�
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
    void ChangedGameMode(GameMode gameMode);// �Լ� ����
}
