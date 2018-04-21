using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int health;
    public gameManager gm;
    // Dont know if we are going to use this!
    private int _health { get; set; }
    /* USE later
	void Start () {
		
	}
	
	void Update () {
		
	}*/

    public void die ()
    {
        // ask the gameManager if we can respawn 
        bool canRespawn = gm.canRespawnPlayer();
        // if we are allowed , RESPAWN
        if ( canRespawn)
        {
            Debug.Log("Player is respawning...");
            this.gameObject.SetActive(false);
            this.gameObject.transform.position = gm.getSpawnLocation();
            // reset health and or other stuff??
            this.gameObject.SetActive(true);
            Debug.Log("Player has respawned!");
        }
        else
        {
            Debug.Log("Game over!");
        }
    }
}
