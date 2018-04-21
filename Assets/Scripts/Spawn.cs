using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    private bool shouldSpawn = true;

    public int amount = 1;

    public GameObject enemyType;

	void Update () {
	    if (shouldSpawn)
	    {
            Debug.Log(string.Format("{0} is spawning",  this.gameObject.name));
	        for (int i = 0; i < amount; i++)
	        {
	            GameObject go =GameObject.Instantiate(enemyType, transform.position, Quaternion.identity);
	        }
	        shouldSpawn = false;
	    }
	}
}
