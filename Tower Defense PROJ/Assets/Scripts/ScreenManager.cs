using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button loseRestartButton;
    [SerializeField] private Button loseQuitButton;

    void Start()
    {
        restartButton.onClick.AddListener(Restart);
        quitButton.onClick.AddListener(Quit);
        loseRestartButton.onClick.AddListener(Restart);
        loseQuitButton.onClick.AddListener(Quit);

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void ShowWin()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowLose()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}