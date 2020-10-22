﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LevelTracker
{     
    private static readonly LevelTracker instance = new LevelTracker();
    private LevelTracker()
    {
        overlay = GameObject.Instantiate(Resources.Load("overlays/ReadyScreen") as GameObject, Camera.main.transform).GetComponent<SpriteRenderer>();
        overlay.sortingLayerName = "Overlay";
    }
    public static LevelTracker _i
    {
        get { return instance; }
    }

    private int num_enemies;
    public void clearEnemies()
    {
        num_enemies = 0;
    }
    public void trackEnemy()
    {
        ++num_enemies;
    }
    public void enemyDeath()
    {
        num_enemies--;
        if(num_enemies <= 0)
        {
            endLevel();
        }
    }

    public enum LevelState
    {
        LS_WAIT,
        LS_PLAY,
        LS_WIN
    }
    LevelState level_state = LevelState.LS_WAIT;
    public LevelState getState() { return level_state; }
    public void startLevel()
    {
        Debug.Log("LEVEL START");
        level_state = LevelState.LS_PLAY;
        enemyBehaviour[] enemies = GameObject.FindObjectsOfType<enemyBehaviour>();
        foreach(enemyBehaviour enemy in enemies)
        {
            enemy.wake();
        }
        // Hide "READY?" msg
        GameObject.Destroy(overlay.gameObject);
        overlay = null;        
    }
    private void endLevel()
    {
        Debug.Log("LEVEL COMPLETE");
        // Show "SUPER FLAT" msg
        level_state = LevelState.LS_WIN;

        overlay = GameObject.Instantiate(Resources.Load("overlays/WinScreen") as GameObject, Camera.main.transform).GetComponent<SpriteRenderer>();
        overlay.sortingLayerName = "Overlay";
    }

    SpriteRenderer overlay = null;    
}