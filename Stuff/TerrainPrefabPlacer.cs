using System.Collections;
using UnityEditor;
using UnityEngine;

public class TerrainPrefabPlacer : MonoBehaviour
{
    [Header("Основные настройки")]
    public GameObject prefab;
    public Terrain targetTerrain;

    [Header("Параметры размещения")]
    [Range(0.1f, 100f)]
    public float density = 10f; // объектов на 100x100 единиц
    public bool randomYRotation = true;
    public Vector3 uniformScale = Vector3.one;

    [Header("Высота размещения")]
    public bool alignToTerrain = true;
    public float heightOffset = 0f;

    [Header("Оптимизация")]
    public bool combineMeshes = false;
    public string parentObjectName = "PlacedPrefabs";

    [ContextMenu("Разместить префабы")]
    public void PlacePrefabs()
    {
        if (prefab == null || targetTerrain == null)
        {
            Debug.LogError("Префаб или Terrain не назначены!");
            return;
        }

        // Создаем родительский объект для организации
        GameObject parentObject = GameObject.Find(parentObjectName);
        if (parentObject != null)
        {
            DestroyImmediate(parentObject);
        }
        parentObject = new GameObject(parentObjectName);

        TerrainData terrainData = targetTerrain.terrainData;
        Vector3 terrainSize = terrainData.size;
        Vector3 terrainPos = targetTerrain.transform.position;

        // Рассчитываем количество объектов на основе плотности
        float area = terrainSize.x * terrainSize.z;
        int objectCount = Mathf.RoundToInt((area / 10000f) * density);

        Debug.Log($"Размещаем {objectCount} объектов на территории {terrainSize.x}x{terrainSize.z}");

        for (int i = 0; i < objectCount; i++)
        {
            // Случайная позиция в пределах Terrain
            float randomX = Random.Range(0f, terrainSize.x);
            float randomZ = Random.Range(0f, terrainSize.z);

            // Получаем высоту Terrain в этой точке
            float terrainHeight = targetTerrain.SampleHeight(new Vector3(
                randomX + terrainPos.x,
                0,
                randomZ + terrainPos.z
            ));

            Vector3 spawnPosition = new Vector3(
                randomX + terrainPos.x,
                terrainHeight + heightOffset,
                randomZ + terrainPos.z
            );

            // Случайное вращение по Y
            Quaternion rotation = randomYRotation ?
                Quaternion.Euler(0, Random.Range(0f, 360f), 0) :
                Quaternion.identity;

            // Создаем экземпляр префаба
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            instance.transform.SetParent(parentObject.transform);
            instance.transform.position = spawnPosition;
            instance.transform.rotation = rotation;
            instance.transform.localScale = uniformScale;

            // Опционально: переименовываем для порядка
            instance.name = $"{prefab.name}_{i:000}";
        }

        Debug.Log($"Успешно размещено {objectCount} объектов!");
    }

    [ContextMenu("Очистить все объекты")]
    public void ClearAllObjects()
    {
        GameObject parentObject = GameObject.Find(parentObjectName);
        if (parentObject != null)
        {
            DestroyImmediate(parentObject);
            Debug.Log("Все объекты удалены!");
        }
        else
        {
            Debug.Log("Объекты для удаления не найдены!");
        }
    }

    // Автоматически находим Terrain если не назначен
    private void Reset()
    {
        if (targetTerrain == null)
        {
            targetTerrain = FindObjectOfType<Terrain>();
            if (targetTerrain != null)
            {
                Debug.Log("Terrain автоматически назначен!");
            }
        }
    }
}