using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingDestination : Destination
{
    public Collection[] collections;

    public void Start()
    {
        collections = GetComponentsInChildren<Collection>();

        for (int i = 0; i < collections.Length; i++)
        {
            collections[i].gameObject.SetActive(true);
        }

    }

    public void Update()
    {
        //if (character.agent.remainingDistance < 0.1)
        //{

        //}


    }

    public IEnumerator CoActive(Collection collection)
    {
        yield return new WaitForSeconds(5);
        collection.gameObject.SetActive(true);
    }

    public override bool CanTake()
    {
        for (int i = 0; i < collections.Length; i++)
        { 
            if (collections[i].gameObject.activeSelf && isUsing == false)
            {
                return true;
            }

        }
        

        return false;

        //collections �߿� ������ �� �ִ°� �ִ��� Ȯ��;

    }


    public override void Arrived(Character c)
    {
        base.Arrived(c);



    }

    public override IEnumerator CoAction(float waitTime)
    {
        while (true)
        {
            //�����ϰ� collections �� �ϳ� �̱� + ĳ���� �̵���Ű��
            Collection collection = RandomCollection();
            if (collection == null)
                break;

            character.agent.SetDestination(collection.transform.position);

            while (true)
            {
                yield return null;

                if (character.agent.pathPending == false && character.agent.remainingDistance < 0.1)
                {
                    break;
                }

            }

            yield return new WaitForSeconds(1f); //���� �ִϸ��̼� + ����! ���

            collection.gameObject.SetActive(false);

            StartCoroutine(CoActive(collection));

        }
        


        //�� �̻� ������ �� �ִ� ����� ������
        //���� ��ҷ� �̵�




        yield return null;

        Leave(character);
        

    }

    List<Collection> activeCollections = new List<Collection>();
    public Collection RandomCollection()
    {
        //collections = GetComponentsInChildren<Collection>();
        
        //return collections[Random.Range(0, collections.Length)];

        activeCollections.Clear();

        for (int i = 0;i < collections.Length; i++)
        {
            if (collections[i].gameObject.activeSelf)
            {
                activeCollections.Add(collections[i]);
            }
        }

        if (activeCollections.Count <= 0)
            return null;

        return activeCollections[Random.Range(0, activeCollections.Count)];




    }
}
