using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHub : MonoBehaviour
{
    // on game start Hub and other levels are disabled and later enabled in tutorial>>

    [SerializeField] GameObject hubLevel;
    // [SerializeField] GameObject lvlOne;
    [SerializeField] GameObject lvlTwo;


    private void OnTriggerEnter(Collider other)
    {
          if (other.TryGetComponent<PlayerCollected>(out PlayerCollected player))
        {
           Debug.Log("Hub and levels loading in  now");

           hubLevel.SetActive(true);
        //    lvlOne.SetActive(true);
           lvlTwo.SetActive(true);

        }
    }
}
