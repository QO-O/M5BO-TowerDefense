using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private Tilemap placementMap;
    [SerializeField] private Tilemap nonPlaceableMap;
    [SerializeField] private GameObject ghostPrefab;
    private Dictionary<Vector3Int, Tower> occupiedTiles = new Dictionary<Vector3Int, Tower>();
    private GameObject ghostInstance;

    void Update()
    {
        HandlePlacementHover();
        HandlePlacementClick();
    }

    void HandlePlacementHover()
    {
        
        if (TowerSelectionUI.SelectedTowerPrefab == null)
        {
            if (ghostInstance != null)
                Destroy(ghostInstance);
            return;
        }
        if (ghostInstance == null)
            ghostInstance = Instantiate(ghostPrefab);

        ghostInstance.GetComponent<SpriteRenderer>().sprite =
            TowerSelectionUI.SelectedTowerPrefab.GetComponent<SpriteRenderer>().sprite;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int cellPos = placementMap.WorldToCell(mouseWorldPos);
        Vector3 worldCenter = placementMap.GetCellCenterWorld(cellPos);

        ghostInstance.transform.position = worldCenter + new Vector3(0, placementMap.cellSize.y * 0.25f);

        Tower towerStats = TowerSelectionUI.SelectedTowerPrefab.GetComponent<Tower>();
        bool affordable = towerStats != null
        && CurrencyManager.Instance != null
        && CurrencyManager.Instance.Gold >= towerStats.Cost;

        bool valid = placementMap.HasTile(cellPos) && !occupiedTiles.ContainsKey(cellPos);
        ghostInstance.GetComponent<GhostTower>().SetValid(valid && affordable);
    }

    void HandlePlacementClick()
    {

        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if (TowerSelectionUI.SelectedTowerPrefab == null)
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition); p.z = 0;
            Vector3Int cell = placementMap.WorldToCell(p);
            if (occupiedTiles.TryGetValue(cell, out Tower t))
                t.TryUpgrade();
            return;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int cellPos = placementMap.WorldToCell(mouseWorldPos);

        if (occupiedTiles.ContainsKey(cellPos))
        {
            occupiedTiles[cellPos].TryUpgrade();
            return;
        }

        if (!placementMap.HasTile(cellPos)) return;

        Tower towerStats = TowerSelectionUI.SelectedTowerPrefab.GetComponent<Tower>();
        if (towerStats == null) return;

        if (!CurrencyManager.Instance.TrySpendGold(towerStats.Cost))
            return;

        GameObject tower = Instantiate(TowerSelectionUI.SelectedTowerPrefab,
            ghostInstance.transform.position, Quaternion.identity);
        
        tower.GetComponent<Tower>().Initialize(cellPos);
        
        SetEntityOrder(tower.transform, 500);

        TowerSelectionUI.SelectTower(null); 
        occupiedTiles.Add(cellPos, tower.GetComponent<Tower>());
        
    }

    private static TowerPlacer instance;
    public static TowerPlacer Instance { get { return instance; } }

    void Awake() { if (instance == null) instance = this; }

    public void FreeTile(Vector3Int cell)
        {
            occupiedTiles.Remove(cell);
        }

    void SetEntityOrder(Transform root, int offset)
    {
        var sortingGroup = root.GetComponent<SortingGroup>();
        if (sortingGroup != null)
        {
            sortingGroup.sortingOrder = offset - (int)(root.position.y * 100);
            return;
        }
        foreach (var sr in root.GetComponentsInChildren<SpriteRenderer>())
            sr.sortingOrder = offset - (int)(root.position.y * 100);
    }

    public Vector3Int? FindFreeTileNear(Vector3Int center, int radius)
        {
            for (int attempt = 0; attempt < 20; attempt++)
            {
                Vector3Int candidate = center + new Vector3Int(
                    Random.Range(-radius, radius + 1), Random.Range(-radius, radius + 1), 0);
                if (candidate != center && placementMap.HasTile(candidate) && !occupiedTiles.ContainsKey(candidate))
                    return candidate;
            }
            return null;
        }

        public void MoveTower(Vector3Int from, Vector3Int to)
        {
            if (!occupiedTiles.TryGetValue(from, out Tower t)) return;
            if (occupiedTiles.ContainsKey(to)) return;
            occupiedTiles.Remove(from);
            occupiedTiles.Add(to, t);
            t.transform.position = placementMap.GetCellCenterWorld(to)
                                    + new Vector3(0, placementMap.cellSize.y * 0.25f);
        }
}