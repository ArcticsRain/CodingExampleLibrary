using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TpcHealthSystem : MonoBehaviour
{

    public static TpcHealthSystem instantOfTpcHealthSystem;

    public int maxHP;
    public int currentHP;

    [SerializeField] public GameObject startingWaypoint;

    //  [Header("saving HP")]
    //  public int totalHP;


    private void Awake()
    {
        instantOfTpcHealthSystem = this;
        setHealth();

        // total hp is in PlayerGameData and gets loaded on playercollected start so on scene loaded
        //set on awake because playerCollected start needs to access it
        currentHP = PlayerGameData.amounthealth;
    }

    public void setHealth()
    {
        currentHP = maxHP;

        PlayerUIScript.instantOfPlayerUIScript.setHealthUI();
    }


    public void OnHealthUpdate(int incomingDamage)
    {
        //Debug.Log("incomingDamage= " + incomingDamage);
        currentHP += incomingDamage;
        AudioScript.InstanceOfAudioScript.PlayHitSound();
        // currentHP = Mathf.Clamp(currentHP, 3, maxHP);
        PlayerUIScript.instantOfPlayerUIScript.setHealthUI();
        // Debug.Log("currentHP " + currentHP);
        if (currentHP >= 3)
        {

            this.transform.position = startingWaypoint.transform.position;
            PlayerCollected.instantOfPlayerCollected.OnBananaUpdate(-PlayerCollected.instantOfPlayerCollected.amountOfBananaLost);
            currentHP = maxHP;
            PlayerUIScript.instantOfPlayerUIScript.setHealthUI();

        }

        // totalHP = currentHP; // set total to current for saving
        PlayerGameData.amounthealth = currentHP; //set data to PlayerGameData

    }



}
