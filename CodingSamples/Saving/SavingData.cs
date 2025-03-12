using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavingData : MonoBehaviour
{
    public static SavingData instanceOfSavingData;
    [SerializeField] PlayerGameData playerGameData;

    [SerializeField] PlayerMovementController playerMovementController;
    private void Awake()
    {
        instanceOfSavingData = this;
        playerGameData = new PlayerGameData();
    }
    private void Start()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
    }

    [ContextMenu("Load")]
    public void LoadPlayerData()
    {
        playerGameData = JsonUtility.FromJson<PlayerGameData>(PlayerPrefs.GetString("playerGameData"));
        // playerGameData = JsonUtility.FromJson<PlayerGameData>(PlayerPrefs.GetString("playerGameData"));

        // Debug.Log("player data loaded");
        // Debug.Log($"amountbananas :{playerGameData.amountbananas}");
        // Debug.Log($"amountshipParts :{playerGameData.amountshipParts}");
        // Debug.Log($"amountGems :{playerGameData.amountGems}");
        // Debug.Log($"amounthealth:{playerGameData.amounthealth}");
        // Debug.Log($"jetpackActive :{playerGameData.jetpackActive}");

    }

    public void SavePlayerData(PlayerGameData data)
    {

        playerGameData = data;
        Debug.Log("SaveingPlayer Called");
        PlayerPrefs.SetString("playerGameData", JsonUtility.ToJson(playerGameData));
        PlayerPrefs.Save();
        Debug.Log("player data saved");

        Debug.Log($"amountbananas :{PlayerGameData.amountbananas}");
        Debug.Log($"amountshipParts :{PlayerGameData.amountshipParts}");
        Debug.Log($"amountGems :{PlayerGameData.amountGems}");
        Debug.Log($"amounthealth:{PlayerGameData.amounthealth}");
        Debug.Log($"jetpackActive :{PlayerGameData.jetpackActive}");

    }

    [ContextMenu("Save Data")]
    void SavePlayerDataFunction()
    {
        SavePlayerData(playerGameData);
    }

    public void OnResetData()
    {
        PlayerGameData.amountbananas = 0;
        PlayerGameData.amountGems = 0;
        PlayerGameData.amounthealth = 0;
        PlayerGameData.amountshipParts = 0;
        PlayerGameData.jetpackActive = false;

        // playerGameData.amountbananas = 0;
        // playerGameData.amountGems = 0;
        // playerGameData.amounthealth = 0;
        // playerGameData.amountshipParts = 0;
        // playerGameData.jetpackActive = false;

        SavePlayerData(playerGameData);

        //add fade out fade in
        if (playerMovementController != null)
        {

        this.transform.position = TpcHealthSystem.instantOfTpcHealthSystem.startingWaypoint.transform.position;// move player back to where they were
        PlayerCollected.instantOfPlayerCollected.ReloadUiAfterRestart(); // reload UI using Game Data after reset
        }

    }


}