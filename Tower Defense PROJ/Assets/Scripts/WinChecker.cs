using UnityEngine;

public class WinChecker : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private ScreenManager screenManager;

    private bool gameOver = false;

    void Update()
    {
        if (gameOver) return;

        bool baseAlive = HomeBase.Instance != null && HomeBase.Instance.Health > 0f;
        if (!baseAlive) return;

        if (!waveManager.WaveRunning && waveManager.CurrentWaveIndex >= waveManager.TotalWaves)
        {
            gameOver = true;
            screenManager.ShowWin();
        }
    }
}
