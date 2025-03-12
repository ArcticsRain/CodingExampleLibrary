using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsCollection : MonoBehaviour
{
    [HideInInspector] public int amountPartsCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerCollected>(out PlayerCollected player))
        {
            PlayerCollected.instantOfPlayerCollected.OnPartsUpdate(1);
            gameObject.SetActive(false);
        }
    }
}
