using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationManager : MonoBehaviour
{
    public static DestinationManager Instance;
    public Destination[] destinations; //���� �ִ� ��������
    public int targetIdx; //���ߵǴ� �������� �ε���

    private void Awake()
    {
        Instance = this;

        targetIdx = -1;
        destinations = FindObjectsOfType<Destination>();
        
        //����Ƽ6��
        //destinations = FindObjectsByType<Destination>(FindObjectsSortMode.InstanceID);
    }

    List<Destination> canTakeDestinations = new List<Destination>();

    public Destination GetEmptyDestination()
    {

        canTakeDestinations.Clear();

        for (int i = 0; i < destinations.Length; i++)
        {
            if (destinations[i].CanTake() == true)
            {
                canTakeDestinations.Add(destinations[i]);
            }

        }

        if (canTakeDestinations.Count <= 0)
        {
            return null;
        }

        int randomNum = Random.Range(0, canTakeDestinations.Count);

        return canTakeDestinations[randomNum]; 


        //targetIdx = RandomNumber();
        //return destinations[targetIdx];
    }

}
