using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikkenMovement : MonoBehaviour
{
    
    EnemyMovement enemyMovemnt;
    playerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {   
        float daño;
        if(enemyMovemnt.TypeAttack == 1){
            daño = UnityEngine.Random.Range(0.01f, 0.03f);
        }else{
            daño = 0;
        }

        if(other.gameObject.CompareTag("Player"))
        {
            playerHealth.health -= daño;
            Debug.Log("Vida Jugador: " + playerHealth.health);
        }
    }
}
