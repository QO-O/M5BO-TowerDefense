using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get { return instance; } }
    private static PauseManager instance;

    [SerializeField] private Button pauseButton;
    [SerializeField] private TMP_Text pauseButtonText;

    private bool isPaused = false;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResetPause()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
}