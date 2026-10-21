using UnityEngine;

public class TesterTools : MonoBehaviour
{
    private float lastCount;
    private int enemyCount;
    private float timer;
    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer > 0.5f)
        {
            timer = 0f;
            enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        }
        if (Input.GetKeyDown(KeyCode.F))
        Time.timeScale = (Time.timeScale == 1f) ? 3f : 1f;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 400, 20),
            $"FPS: {1f / Time.unscaledDeltaTime:0}  |  Enemies: {enemyCount}");
    }
}
