using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class WaveData
{
    public float duration = 10f;
    public float easySpawnInterval = 2f;
    public float hardSpawnInterval = 3f;
    public int easyEnemies = 5;
    public int hardEnemies = 2;
}

public class WaveManager : MonoBehaviour
{
    public WaveData[] waves;
    public Button startWaveButton;

    public GameObject easyEnemyPrefab;
    public GameObject hardEnemyPrefab;

    private int currentWaveIndex = 0;
    private bool waveRunning = false;

    public TMPro.TextMeshProUGUI waveText;
   
    void Start()
    {
        startWaveButton.onClick.AddListener(StartWave);
        UpdateWaveText();
    }

    public void StartWave()
    {
        Debug.Log($"StartWave clicked. waveRunning={waveRunning}, currentWaveIndex={currentWaveIndex}, waves.Length={waves.Length}");
        if (waveRunning) return;
        if (currentWaveIndex >=waves.Length) return;

        StartCoroutine(RunWave());
    }

   IEnumerator RunWave()
{
    waveRunning = true;
    startWaveButton.interactable = false;
    UpdateWaveText();

    WaveData wave = waves[currentWaveIndex];

    for (int i = 0; i < wave.easyEnemies; i++)
    {
        SpawnEnemy(easyEnemyPrefab);
        yield return new WaitForSeconds(wave.easySpawnInterval);
    }
    for (int i = 0; i < wave.hardEnemies; i++)
    {
        SpawnEnemy(hardEnemyPrefab);
        yield return new WaitForSeconds(wave.hardSpawnInterval);
    }
    yield return new WaitForSeconds(wave.duration / 3);

    waveRunning = false;
    startWaveButton.interactable = true;
    currentWaveIndex++;
}

    [System.Serializable]
    public class SpawnRoute
    {
        public Transform spawnPoint;
        public Transform[] waypoints;
    }
    public SpawnRoute[] routes;

    void SpawnEnemy(GameObject prefab)
    {
        Debug.Log($"SpawnEnemy called with {prefab?.name}");
        SpawnRoute route = routes[Random.Range(0, routes.Length)];
        Debug.Log($"Spawning at route: {route?.spawnPoint?.name}, position {route?.spawnPoint?.position}");
        GameObject e = Instantiate(prefab, route.spawnPoint.position, Quaternion.identity);
        e.GetComponent<Enemy>().waypoints = route.waypoints;
        Debug.Log($"Spawned: {e.name} at {e.transform.position}");
    }

    void UpdateWaveText()
    {
        if (waveText == null) return;
        if (currentWaveIndex >= waves.Length) { waveText.text = "All waves cleared!"; return; }
        waveText.text = $"Wave {currentWaveIndex + 1}/{waves.Length}";
    }
}
