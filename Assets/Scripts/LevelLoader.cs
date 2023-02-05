using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LevelLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public bool isOutro;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayNextLevel((float)videoPlayer.clip.length));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Return)) {
            if(isOutro) {
                //close game
                Application.Quit();
            } else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public IEnumerator PlayNextLevel(float delay) {

        // wait
        yield return new WaitForSeconds(delay);

        if(isOutro) {
            //close game after outro
            Application.Quit();
        } else {
            // load main game
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
