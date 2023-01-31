using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject PauseMenu;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(isPaused) {
                isPaused = false;
                PauseMenu.SetActive(false);
                Time.timeScale = 1;
            } else {
                Pause();
                isPaused = true;
            }
            
        }
    }

    public void Play() {
        Time.timeScale = 1;
    }

    public void Pause() {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }

    public void Exit() {
        Application.Quit();
    }
}
