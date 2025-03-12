using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public float cutsceneTimer;
    public int loadingSceneIndex; // this is name of the cutscene
    [SerializeField] public string sceneName;

    private void Update()
    {
        cutsceneTimer -= Time.deltaTime;
        if (cutsceneTimer <= 0)
        {
            SwitchScene.Instance.SetSceneIndex(loadingSceneIndex);
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnSkipClick()
    {
        cutsceneTimer = 0; // skip cutscene to loadingScreen
        Debug.Log("SkipCutscene");
    }
}
