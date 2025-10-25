using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance; //변수
    public static CharacterManager Instance //프로퍼티
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<CharacterManager>();

            return instance;
        }
    }


    public List<Character> curCharacters = new List<Character>();

    private void Start()
    {
        curCharacters = FindObjectsOfType<Character>().ToList();
    }



    public Character FindClosest(Vector3 pos)
    {
        //가장 가까운 캐릭터 찾는 코드 들어가야함.
        
        return curCharacters[0];
    }

}
