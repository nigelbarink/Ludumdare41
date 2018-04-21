using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {
    public GameObject SpawnOBJ;
    public health enemyH;
    public int playerLives;

    // timers 
    public const int abilityTimerVal = 2; // 2 for 2 seconds
    public float abilityTimeState = 0;

    // UI elements
    public Text lives;
    public Text enemyLivesDEBUG;
    public Text abilityTimer;

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

        updateTimers();
	}

    void updateTimers()
    {
        if ( abilityTimeState > 0)
        {
            abilityTimeState -= Time.deltaTime;
        } 

    }

    void uiUpdate()
    {
        lives.text = string.Format("Lives left: {0}", _playerLives);
        //enemyLivesDEBUG.text = string.Format("Enemy Lives: {0}", enemyH.getHealth().ToString());
        abilityTimer.text = string.Format("Ability ready in {0}", Mathf.Round(abilityTimeState).ToString());
    }
    public void resetAbilityTimer()
    {
        abilityTimeState = abilityTimerVal;
    }
    public bool isAbilityReady()
    {
        if (abilityTimeState <= 0)
            return true;
        else
            return false;
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
