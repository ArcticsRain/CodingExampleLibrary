using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    public static SwitchScene Instance;
    [SerializeField] public int sceneIndex;// change based on scene or lvl
    [SerializeField] public int spawnIndex;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this; 
        
    }
  
    public void SetSceneIndex(int targetIndex)
    {
        sceneIndex = targetIndex;
        Debug.Log($" scene Index {sceneIndex}");
    }

}
