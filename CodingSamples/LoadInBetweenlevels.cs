using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadInBetweenlevels : MonoBehaviour
{
    // this script is used on trigger between levels

    [SerializeField] SavingData savingData;
    //[SerializeField] Fades fadeScriptRef;
    public int nextSceneIndex;

    private void Awake()
    {
        savingData = FindObjectOfType<SavingData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerCollected>(out PlayerCollected player))
        {
            Fades.instantOfFades.OnFadeOut(); // FadeOut
            PlayerGameData.amounthealth = TpcHealthSystem.instantOfTpcHealthSystem.currentHP;
            savingData.SavePlayerData(PlayerGameData.instanceOfPlayerGameData);
            SwitchScene.Instance.SetSceneIndex(nextSceneIndex);

            StartCoroutine(OnWaitForFades());
            SceneManager.LoadScene(3); // load into next scene based on index
            Fades.instantOfFades.OnFadeIn(); // fadeIn

            PlayerTransforms.instanceOfPlayerTransforms.StoreTransformValue();

        }
    }

    IEnumerator OnWaitForFades() //use this as a wait before loading the next scene
    {
        yield return new WaitForSeconds(5);
    }
}
// add position for player to spawn in after loading