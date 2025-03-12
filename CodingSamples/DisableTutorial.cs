using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTutorial : MonoBehaviour
{
   //after player leaves tutorial level to hub the tutorial is closed of and can not be returned to unless player dies
    [SerializeField] GameObject tutorialLevel;
    [SerializeField] GameObject caveClosingToTutorial;

    private void OnTriggerEnter(Collider other)
    {
          if (other.TryGetComponent<PlayerCollected>(out PlayerCollected player))
        {
           tutorialLevel.SetActive(false);
           caveClosingToTutorial.SetActive(true);

        }
    }
}
