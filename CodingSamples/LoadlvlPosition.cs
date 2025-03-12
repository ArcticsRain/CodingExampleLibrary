using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadlvlPosition : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform monochromeToHub;
    [SerializeField] private Transform forestToHub;
    [SerializeField] private Transform cavesToHub;
    [SerializeField] private Transform RestartGame;
    void Start()
    {
        LoadTransform();
    }

    public void LoadTransform()
    {
        int transformNumber = SwitchScene.Instance.spawnIndex;

        if (transformNumber == 1)
        {
            playerTransform.position = RestartGame.position;
        }
        if (transformNumber == 2)
        {
            playerTransform.position = forestToHub.position;
        }
        if (transformNumber == 3)
        {
            playerTransform.position = monochromeToHub.position;
        }
        if (transformNumber == 4)
        {
            playerTransform.position = cavesToHub.position;
        }
        else
        {
            Debug.Log("no transform set");
        }
    }
}
