using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Header("Настройки")]
    [SerializeField] private GameObject[] itemPrefabs;  // Префабы товаров
    [SerializeField] public Vector3 offset = new Vector3(0f, 2f, 0f); // Нижний элемент башни
    [SerializeField] private float followSpeed = 5f;    // Скорость следования
    [SerializeField] private float hoverAmplitude = 0.2f; // Амплитуда парения
    [SerializeField] private float hoverFrequency = 2f;   // Частота парения
    [SerializeField] public float itemHeight = 0.6f;   // Расстояние между предметами в башне
    [SerializeField] private float intervalFollowSpeed = 0.1f;/// интервал скорости следования

    public List<GameObject> heldItems = new List<GameObject>(); // Все предметы игрока
    private List<int> currentItemIndices = new List<int>();     // Их индексы (для логики)

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // Взятие предмета
        if (Input.GetKeyDown(KeyCode.Alpha1)) TryTakeItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TryTakeItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TryTakeItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TryTakeItem(3);

        UpdateItemsFollow();
    }

    public void TryTakeItem(int index)
    {
        if (index < 0 || index >= itemPrefabs.Length) return;

        // Определяем высоту для нового предмета (над последним)
        Vector3 spawnPos = transform.position + offset + Vector3.up * (heldItems.Count * itemHeight);

        GameObject newItem = Instantiate(itemPrefabs[index], spawnPos, Quaternion.identity);
        heldItems.Add(newItem);
        currentItemIndices.Add(index);

        Debug.Log($"Взяли товар {index}. Сейчас в башне: {heldItems.Count}");
    }

    public void UpdateItemsFollow()
    {
        for (int i = 0; i < heldItems.Count; i++)
        {
            GameObject item = heldItems[i];
            if (item == null) continue;

            // Целевая позиция
            Vector3 targetPos = transform.position + offset + Vector3.up * (i * itemHeight);
            targetPos.y += Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;

            // Чем дальше предмет в списке, тем медленнее он движется
            float speedMultiplier = 1f / (1f + i * intervalFollowSpeed);

            float adjustedSpeed = followSpeed * speedMultiplier;

            // Плавное движение
            item.transform.position = Vector3.Lerp(item.transform.position, targetPos, Time.deltaTime * adjustedSpeed);
        }
    }

    public bool HasItem => heldItems.Count > 0;
    public List<int> CurrentItemIndices => new List<int>(currentItemIndices);

    // Убираем один предмет (например, для сдачи в магазин)
    public void RemoveHeldItem(int index)
    {
        if (index < 0 || index >= heldItems.Count) return;

        Destroy(heldItems[index]);
        heldItems.RemoveAt(index);
        currentItemIndices.RemoveAt(index);
    }

    // Убираем все предметы
    public void RemoveAllItems()
    {
        foreach (var item in heldItems)
        {
            if (item != null) Destroy(item);
        }
        heldItems.Clear();
        currentItemIndices.Clear();
    }
    public List<int> GetCurrentItemIndices()
    {
        return new List<int>(currentItemIndices);
    }
}
