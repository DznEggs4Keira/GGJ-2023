<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int totalBossEnemies;
    public int totalTries;

    public MenuManager menu;

    private int currentTries;
    private int currentBossEnemiesKilled;

    public int CurrentTries {
        get { return currentTries; }
        set { currentTries = value; }
    }

    public int CurrentBossEnemiesKilled {
        get { return currentBossEnemiesKilled; }
        set { currentBossEnemiesKilled = value; }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {

            Destroy(gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTries >= totalTries || currentBossEnemiesKilled >= totalBossEnemies) {

            // reset current tries
            CurrentTries = 0;
            // reset current bosses killed
            CurrentBossEnemiesKilled = 0;

            // Game over
            menu.GameOver();
        }
    }

    public void ReloadGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int totalBossEnemies;
    public int totalTries;

    public MenuManager menu;

    private int currentTries;
    private int currentBossEnemiesKilled;

    public int CurrentTries {
        get { return currentTries; }
        set { currentTries = value; }
    }

    public int CurrentBossEnemiesKilled {
        get { return currentBossEnemiesKilled; }
        set { currentBossEnemiesKilled = value; }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {

            Destroy(gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) {
            AudioManager.instance.StopAllSFX();
        }

        if (currentTries >= totalTries || currentBossEnemiesKilled >= totalBossEnemies) {

            // reset current tries
            CurrentTries = 0;
            // reset current bosses killed
            CurrentBossEnemiesKilled = 0;

            // Game over
            menu.GameOver();
        }
    }

    public void ReloadGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
>>>>>>> main
