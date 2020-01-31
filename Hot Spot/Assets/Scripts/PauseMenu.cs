using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCtrl;

    public void Resume()
    {
        InputController.LockCursor(true);
        pauseMenuCtrl.SetActive(false);
        InputController.game_paused = false;
    }

    void Pause()
    {
        InputController.LockCursor(false);
        pauseMenuCtrl.SetActive(true);
        InputController.game_paused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Menu");
        InputController.LockCursor(false);
        InputController.game_paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void QuitGame()
    {
        Debug.Log("Quiting the game");
        Application.Quit();
    }

    void Update()
    {
        if (InputController.esc_pressed) {
            if (InputController.game_paused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
        InputController.esc_pressed = false;
    }
}
