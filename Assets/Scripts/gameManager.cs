using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {
    public GameObject SpawnOBJ;
    public health enemyH;
    public int playerLives;

    // UI elements
    public Text lives;
    public Text enemyLivesDEBUG;


    private int _playerLives { get; set; }
    private Transform SpawnLocation { set; get; }


	void Start () {
        Debug.Assert(lives != null, "UI element not set!");
        _playerLives = playerLives;
        SpawnLocation = SpawnOBJ.transform;
	}

    void Update () {
        // update UI
        uiUpdate();
	}

    void uiUpdate()
    {
        lives.text = string.Format("Lives left: {0}", _playerLives);
        enemyLivesDEBUG.text = string.Format("Enemy Lives: {0}", enemyH.getHealth().ToString());
    }

    public bool canRespawnPlayer()
    {
        if (_playerLives <= 0)
            return false;
        _playerLives--;
        return true;
    }

    public Vector3 getSpawnLocation()
    {
        return SpawnLocation.position;
    }

}
