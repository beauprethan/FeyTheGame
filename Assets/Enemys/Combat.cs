﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField]
    public float attackCooldown = 1f;

    [SerializeField] public int damageDealt;
    //cooldown before object can take damage again
    private bool _immunity = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.CompareTag("Player") 
              && this.CompareTag("Player"))  
            && !(other.CompareTag("Player") && this.CompareTag("Fey")) 
            && !(other.CompareTag("Fey") 
                 && this.CompareTag("Player"))
            && !(other.CompareTag("Fey") 
                 && this.CompareTag("Fey"))
            && !(other.CompareTag("Enemy") && this.CompareTag("Enemy")))
        {

            //Debug.Log("hit: " + other.name);
        //call our interface in order to access its methods related to the object this is attached to
        //should be attached to the fight hitbox.
        IDamage hit = other.GetComponent<IDamage>();
        if (hit != null)
        {
            if (_immunity == false)
            {
                hit.Damage(damageDealt);
                _immunity = true;
                StartCoroutine(EnemyImmunityCoolDown());
            }
        } 
        }
    }

    IEnumerator EnemyImmunityCoolDown()
    {
        yield return new WaitForSeconds(attackCooldown);
        _immunity = false;
    }
}
