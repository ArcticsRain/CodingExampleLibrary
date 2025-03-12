using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool Paused = false;
    [SerializeField] Canvas MainHud;
    [SerializeField] Canvas backGroundCanvas;
    [SerializeField] Canvas pauseScreen;
    [SerializeField] Canvas ControlsScreen;
    [SerializeField] Canvas SettingsScreen;
    FMOD.Studio.EventInstance clickSound;
    private void CursorSettings(bool cursorVisibility, CursorLockMode cursorLockState)
    {
        Cursor.visible = cursorVisibility;
        Cursor.lockState = cursorLockState;
    }
    private void Awake()
    {
        clickSound = FMODUnity.RuntimeManager.CreateInstance("event:/Ui/ButtonClick");
        pauseScreen.enabled = false;
        SettingsScreen.enabled = false;
        ControlsScreen.enabled = false;
        backGroundCanvas.enabled = false;

        MainHud.enabled = true;
    }

    public void onPause()
    {
        if (!Paused)
        {

            gameObject.SetActive(true);
            pauseScreen.enabled = true;
            backGroundCanvas.enabled = true;
            MainHud.enabled = false;
            playClick();
            CursorSettings(true, CursorLockMode.None);
            Paused = true;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
          //  playClick();
            pauseScreen.enabled = false;
            backGroundCanvas.enabled = false;
            SettingsScreen.enabled = false;
            ControlsScreen.enabled = false;


            MainHud.enabled = true;
            CursorSettings(false, CursorLockMode.Locked);
            Paused = false;
        }

    }

    private void playClick()
    {
        //      clickSound.Play();
        clickSound.start();
    }

    public void onMenuReturn()
    {
        Time.timeScale = 1f;
        playClick();
        SceneManager.LoadScene(1);
    }

    public void GoToDestop()
    {
        playClick();
        SavingData.instanceOfSavingData.OnResetData();
        Application.Quit();
    }

    public void onSettings()
    {
        playClick();
        // Debug.Log("onSettings has been clicked");
        pauseScreen.enabled = false;

        SettingsScreen.enabled = true;

    }

    public void onControls()
    {
        playClick();
        pauseScreen.enabled = false;
        ControlsScreen.enabled = true;
    }

    public void onResume()
    {
        //Debug.Log("onResume has been clicked");
        Time.timeScale = 1f;
        playClick();
        backGroundCanvas.enabled = false;
        pauseScreen.enabled = false;

        SettingsScreen.enabled = false;
        ControlsScreen.enabled = false;


        MainHud.enabled = true;
        CursorSettings(false, CursorLockMode.Locked);
        Paused = false;
    }

    public void GoToPauseMainPause()
    {
        Debug.Log("onSettings has been clicked");
        playClick();
        SettingsScreen.enabled = false;
        ControlsScreen.enabled = false;
        pauseScreen.enabled = true;

    }

    public void OnRestart()
    {
        // Debug.Log("onRestart has been clicked");
        playClick();
        SavingData.instanceOfSavingData.OnResetData();

        //change index of playertransform before restarting
        SwitchScene.Instance.SetSceneIndex(1); // set position = restart pos in switch statement
       
        //

        SceneManager.LoadScene(4);
        onResume();
        Fades.instantOfFades.OnFadeIn();
    }


    // IEnumerator RestartGameLoad()
    // {
    //     yield return new WaitForSeconds(1);
        
    //     SceneManager.LoadScene(4);

    //     yield return null;
    // }
}
