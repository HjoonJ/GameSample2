using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance; //����
    public static CharacterManager Instance //������Ƽ
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
        //���� ����� ĳ���� ã�� �ڵ� ������.
        
        return curCharacters[0];
    }

}
