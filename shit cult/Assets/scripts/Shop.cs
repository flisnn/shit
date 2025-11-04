using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject[] allItems; // 4 возможных товара (префабы)
    [SerializeField] private Transform shopTransform; // объект магазина
    [SerializeField] private float itemSpacing = 1.5f; // расстояние между товарами
    [SerializeField] private float heightAboveShop = 2f; // высота над магазином

    private List<GameObject> currentItems = new List<GameObject>();
    private List<int> currentOrderIndices = new List<int>();       // индексы заказанных товаров
    int j = 0;
    bool gg = false;

    void Start()
    {
        GenerateRandomItems();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var item = currentItems[j];

            Destroy(item);
            j++;
            if (j == 3)
            {
                currentItems.Clear();
                j = 0;
            }
        }

        if (currentItems.Count == 0)
        {
            GenerateRandomItems();
        }
        if (gg)
        {
            gg = false; // отключаем повторный вызов
            StartCoroutine(TryDeliverItemWithDelay());
        }
    }
    private IEnumerator TryDeliverItemWithDelay()
    {
        yield return new WaitForSeconds(0.2f); // задержка 0.5 сек
        TryDeliverItem();
    }
    public void GenerateRandomItems()
    {
        // создаём список индексов
        List<int> indices = new List<int> { 0, 1, 2, 3 };

        // перемешиваем индексы (чтобы выбрать случайные)
        for (int i = 0; i < indices.Count; i++)
        {
            int rand = Random.Range(i, indices.Count);
            (indices[i], indices[rand]) = (indices[rand], indices[i]);
        }

        // берём первые 3 товара
        for (int i = 0; i < 3; i++)
        {
            int index = indices[i];

            // позиция спавна — над магазином с отступом по X
            Vector3 spawnPos = shopTransform.position + new Vector3((i - 1) * itemSpacing, heightAboveShop, 0);

            // создаём товар
            GameObject newItem = Instantiate(allItems[index], spawnPos, Quaternion.identity);
            currentItems.Add(newItem);
            //добавляем в список индексов
            currentOrderIndices.Add(index);
            Debug.Log($"{index}");

        }
    }

    public void TryDeliverItem()
    {
        gg = false;
        var player = PlayerInventory.Instance;
        if (player == null || !player.HasItem)
            return;

        // Получаем все предметы игрока и их индексы
        var playerIndices = player.CurrentItemIndices;

        // Проходим по всем предметам игрока, чтобы проверить, есть ли совпадение с заказами
        for (int i = 0; i < playerIndices.Count; i++)
        {
            int playerItemIdx = playerIndices[i];

            // Проверяем, запрашивает ли магазин этот товар
            if (currentOrderIndices.Contains(playerItemIdx))
            {
                int shopIndex = currentOrderIndices.IndexOf(playerItemIdx);

                Debug.Log($"✅ Магазин принял товар {playerItemIdx} (позиция в заказе: {shopIndex})");
                if (playerItemIdx < 2) ScoreManagerTMP.Instance.AddScore(50); else ScoreManagerTMP.Instance.AddScore(150);
                gg = true;
                // Удаляем визуальный объект над магазином
                if (shopIndex >= 0 && shopIndex < currentItems.Count)
                {
                    Destroy(currentItems[shopIndex]);
                    currentItems.RemoveAt(shopIndex);  // удаляем из списка визуальных объектов
                }

                // Удаляем заказ из списка
                currentOrderIndices.RemoveAt(shopIndex);

                // Убираем предмет у игрока (по индексу в башне)
                player.RemoveHeldItem(i);

                // Проверяем — все ли заказы выполнены
                if (currentOrderIndices.Count == 0)
                {
                    Debug.Log("🎉 Все заказы выполнены! Создаём новые...");
                    currentItems.Clear();
                    currentOrderIndices.Clear();
                    GenerateRandomItems();
                }

                // Важное: после удаления предмета из списка игрока — индекс смещается,
                // поэтому выходим из цикла, чтобы не сломать перебор
                return;
            }
        }

        // Если ни один предмет не подошёл
        Debug.Log("❌ Магазин не заказывал эти товары.");
    }

}
