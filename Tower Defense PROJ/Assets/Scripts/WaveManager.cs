using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class WaveData
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private float easySpawnInterval = 2f;
    [SerializeField] private float hardSpawnInterval = 3f;
    [SerializeField] private int easyEnemies = 5;
    [SerializeField] private int hardEnemies = 2;

    public float Duration { get { return duration; } }
    public float EasySpawnInterval { get { return easySpawnInterval; } }
    public float HardSpawnInterval { get { return hardSpawnInterval; } }
    public int EasyEnemies { get { return easyEnemies; } }
    public int HardEnemies { get { return hardEnemies; } }
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveData[] waves;
    [SerializeField] private Button startWaveButton;
    [SerializeField] private GameObject easyEnemyPrefab;
    [SerializeField] private GameObject hardEnemyPrefab;
    [SerializeField] private SpawnRoute[] routes;
    [SerializeField] private TMPro.TextMeshProUGUI waveText;

    private int currentWaveIndex = 0;
    private bool waveRunning = false;

    [System.Serializable]
    public class SpawnRoute
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform[] waypoints;

        public Transform SpawnPoint { get { return spawnPoint; } }
        public Transform[] Waypoints { get { return waypoints; } }
    }

    public int CurrentWaveIndex { get { return currentWaveIndex; } }
    public int TotalWaves { get { return waves != null ? waves.Length : 0; } }
    public bool WaveRunning { get { return waveRunning; } }

    void Start()
    {
        startWaveButton.onClick.AddListener(StartWave);
        UpdateWaveText();
    }

    public void StartWave()
    {
        if (waveRunning) return;
        if (currentWaveIndex >= waves.Length) return;
        StartCoroutine(RunWave());
    }

    IEnumerator RunWave()
    {
        waveRunning = true;
        startWaveButton.interactable = false;
        UpdateWaveText();

        WaveData wave = waves[currentWaveIndex];

        for (int i = 0; i < wave.EasyEnemies; i++)
        {
            SpawnEnemy(easyEnemyPrefab);
            yield return new WaitForSeconds(wave.EasySpawnInterval);
        }
        for (int i = 0; i < wave.HardEnemies; i++)
        {
            SpawnEnemy(hardEnemyPrefab);
            yield return new WaitForSeconds(wave.HardSpawnInterval);
        }
        yield return new WaitForSeconds(wave.Duration / 3);
        waveRunning = false;
        startWaveButton.interactable = true;
        currentWaveIndex++;
        UpdateWaveText();
    }

    void SpawnEnemy(GameObject prefab)
    {
        SpawnRoute route = routes[Random.Range(0, routes.Length)];
        GameObject e = Instantiate(prefab, route.SpawnPoint.position, Quaternion.identity);
        e.GetComponent<Enemy>().SetRoute(route.Waypoints);
    }

    void UpdateWaveText()
    {
        if (waveText == null) return;
        if (currentWaveIndex >= waves.Length) { waveText.text = "All waves cleared!"; return; }
        waveText.text = $"Wave {currentWaveIndex + 1}/{waves.Length}";
    }
}