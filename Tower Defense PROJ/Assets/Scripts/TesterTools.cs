using UnityEngine;
using TMPro;

public class TesterTools : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;

    private float fps;
    private int enemyCount;
    private float updateTimer;

    void Update()
    {
        updateTimer += Time.unscaledDeltaTime;
        if (updateTimer >= 0.5f)
        {
            updateTimer = 0f;
            fps = 1f / Time.unscaledDeltaTime;
            enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
            UpdateStatsText();
        }

        if (Input.GetKeyDown(KeyCode.F))
            Time.timeScale = (Time.timeScale == 1f) ? 3f : 1f;

        if (Input.GetKeyDown(KeyCode.E))
            foreach (Enemy e in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
                e.TakeDamage(1f);

        if (Input.GetKeyDown(KeyCode.T))
            foreach (Tower t in FindObjectsByType<Tower>(FindObjectsSortMode.None))
                t.TakeDamage(1000f);
    }

    void UpdateStatsText()
    {
        if (statsText == null) return;
        statsText.text = $"FPS: {fps:0} | Enemies: {enemyCount} |  | E: dmg enemies | T: kill towers";
    }
}
