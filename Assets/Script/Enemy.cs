using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour
{
    public float maxHp;
    public float curHp;
    public float moveSpeed;
    [SerializeField] public NavMeshAgent agent;

    public Character character;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }


    private void Start()
    {
        Character character = CharacterManager.Instance.FindClosest(transform.position);

        //ĳ�������� ����!
        agent.SetDestination(character.transform.position);

    }

    private void Update()
    {
        if (character != null) 
        {
            agent.SetDestination(character.transform.position);

        }

        

    }


}
