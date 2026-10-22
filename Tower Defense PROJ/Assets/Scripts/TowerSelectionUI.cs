using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
   private static GameObject selectedTowerPrefab; 

    public static void SelectTower(GameObject prefab)
    {
        selectedTowerPrefab = prefab;
    }

    public static GameObject SelectedTowerPrefab
    {
        get { return selectedTowerPrefab; }
    }
}
