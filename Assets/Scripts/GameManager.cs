using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Into and Outro Clips")]
    public  VideoClip intro;
    public VideoClip outro;
    public VideoPlayer videoPlayer;

    [Header("Main Menu")]
    public GameObject MainMenu;
    public MenuManager menu;

    [Header("Game Over Screen")]
    public GameObject GameOverScreen;

    [Header("Game Settings")]
    public GameObject levelLayout;
    public int totalTries;

    private int totalBossEnemies;
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

        // hide game level
        levelLayout.gameObject.SetActive(false);

        // play intro
        videoPlayer.clip = intro;
        videoPlayer.Play();
    }

    private void Start() {

        Time.timeScale = 0;

        // find all boss enemy types on map
        var totalEnemies = GameObject.FindObjectsOfType<EnemySpawner>();
        totalBossEnemies = totalEnemies.Length;
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
            GameOver();
        }

        if(videoPlayer.clip != null) {
            CheckPlaying();
        }
    }

    public void CheckPlaying() {
        // intro
        if(videoPlayer.clip == intro) {
            if (!videoPlayer.isPlaying) {
                MainMenu.gameObject.SetActive(true);
            } else {
                MainMenu.gameObject.SetActive(false);
            }
        }

        // outro
        if(videoPlayer.clip == outro) {
            if(!videoPlayer.isPlaying) {
                GameOverScreen.SetActive(true);
            } else {
                GameOverScreen.SetActive(false);
            }
        }
        
    }

    public void GameOver() {
        Time.timeScale = 0;

        //hide game
        levelLayout.gameObject.SetActive(false);

        // play Outro
        videoPlayer.clip = outro;
        videoPlayer.Play();
    }

    public void ReloadGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
