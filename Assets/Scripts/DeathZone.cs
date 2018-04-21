using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.tag == "Player")
        {
            // the player should either respawn or its gameover 
            // we can set a flag for it to the game manager 
            // either way will let the player decide what todo next 

            Player p = collision.gameObject.GetComponent<Player>();
            Debug.Log("Player Fell and died!");
            p.die();


        }
    }
}
