using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LevelManager
{
    const int NUM_LEVELS = 3;
    
    // Establish Singleton
    private static readonly LevelManager instance = new LevelManager();
    private LevelManager()
    {
        // Create initial READY overlay
        overlay = GameObject.Instantiate(Resources.Load("overlays/ReadyScreen") as GameObject, Camera.main.transform).GetComponent<SpriteRenderer>();

        // Tie local onSceneLoaded() function to the Scene Manager
        SceneManager.sceneLoaded += onSceneLoaded;
    }
    public static LevelManager _i
    {
        get { return instance; }
    }

    // ENEMY TRACKING
    private int num_enemies;
    public void countEnemies()
    {
        EnemySpawn[] enemies = GameObject.FindObjectsOfType<EnemySpawn>();
        num_enemies = enemies.Length;
    }
    public void enemyDeath()
    {
        num_enemies--;
        if(num_enemies <= 0)
        {
            endLevel();
        }
    }

    // STATE CONTROL
    public enum LevelState
    {
        LS_WAIT,
        LS_PLAY,
        LS_WIN
    }
    LevelState level_state = LevelState.LS_WAIT;

    public LevelState getState() { return level_state; }

    // When the player performs their first input
    public void startLevel()
    {
        Debug.Log("LEVEL START");
        level_state = LevelState.LS_PLAY;
        countEnemies();
        enemyBehaviour[] enemies = GameObject.FindObjectsOfType<enemyBehaviour>();
        foreach(enemyBehaviour enemy in enemies)
        {
            enemy.wake();
        }
        if(overlay != null)
        {
            GameObject.Destroy(overlay.gameObject);
            overlay = null;
        }
    }

    // When all enemies have died, change State and load Overlay
    private void endLevel()
    {
        Debug.Log("LEVEL COMPLETE");
        level_state = LevelState.LS_WIN;

        overlay = GameObject.Instantiate(Resources.Load("overlays/WinScreen") as GameObject, Camera.main.transform).GetComponent<SpriteRenderer>();
    }

    SpriteRenderer overlay = null;

    // Level Switching
    int current_level_id = 1;
    public void nextLevel()
    {
        // Remove Winstate overlay
        GameObject.Destroy(overlay.gameObject);
        overlay = null;

        // Load next level
        ++current_level_id;
        if(current_level_id > NUM_LEVELS)
        {
            SceneManager.LoadScene("Menu");
        }
        SceneManager.LoadScene("Level" + current_level_id);
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        level_state = LevelState.LS_WAIT;

        if(scene.name == "Menu")
        {
            // Don't load overlay on the menu
            return;
        }

        // Reset self for the new level
        overlay = GameObject.Instantiate(Resources.Load("overlays/ReadyScreen") as GameObject, Camera.main.transform).GetComponent<SpriteRenderer>();
        GameObject.FindObjectOfType<Player>().setReverseMode(reverse);
    }

    public void loadLevel(int id)
    {
        current_level_id = id;
        SceneManager.LoadScene("Level" + current_level_id);
    }

    bool reverse = false;
    public void setReverseMode(bool state)
    {
        reverse = true;
    }
}
