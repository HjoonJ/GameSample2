using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour, IChangedGameMode
{
    private void Awake()
    {
        
        GameManager.changedGameModeListener.Add(this);
    }

    public void ChangedGameMode(GameMode gm)
    {

    }
}
