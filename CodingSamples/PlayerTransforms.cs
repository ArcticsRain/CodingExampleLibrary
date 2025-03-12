using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransforms : MonoBehaviour
{
    public static PlayerTransforms instanceOfPlayerTransforms;
    private Transform playerTransform;
    private Transform monochromeToHub;
    private Transform forestToHub;
    private Transform cavesToHub;
    private Transform OriginalHub;
    private Transform RestartGame;
    public int transformNumber;


    public void Awake()
    {
        instanceOfPlayerTransforms = this;
        // transformNumber = 0;
    }

    public void LoadTransform()
    {
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

    public void StoreTransformValue()
    {
        SwitchScene.Instance.spawnIndex = transformNumber;
    }
}
