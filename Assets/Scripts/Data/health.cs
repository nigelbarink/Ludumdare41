using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour {
    public Player player;
    public const int lives = 50;
    private int amount = lives;
	
    public int getHealth()
    {
        return this.amount;
    }


    public void loseHealth ( int amount)
    {
        this.amount = this.amount - amount;
        if ( this.amount <= 0)
        {
            if (this.tag == "Player")
            {
                player.die();
            } else
            {
                die();
            }
        }
    }

    public void die()
    {
        this.gameObject.SetActive(false);
        Debug.Log(gameObject.name + " died!");
    }
}

