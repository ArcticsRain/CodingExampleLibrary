using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMod : MonoBehaviour
{
   [SerializeField] int healthModValue = -1;
   [SerializeField] bool isDestroyable = true;
    [SerializeField] string tagName;
    [SerializeField] float MaxHpEnemy = 100;
    public float currentEnemyHp;

    private void Awake()
    {
        currentEnemyHp = MaxHpEnemy;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(tagName)) 
        {
            other.gameObject.GetComponent<TpcHealthSystem>().OnHealthUpdate(healthModValue); // player HP script
            if (isDestroyable)
            {
                Destroy(gameObject); // PlayerActions takes dmg from plant melee
            }
        }
    }

    // AI takes dmg
    public void ONTakeDmg(float incomingDamage)  // Enemy / plant dies
    {
        currentEnemyHp -= incomingDamage;
        if (currentEnemyHp <= 0)
        {
            Debug.Log("Enemy OBJ destroyed");
            Destroy(gameObject); // reload scene 
            
            //Destroy(gameObject,4); 
            
        }
    }
}
