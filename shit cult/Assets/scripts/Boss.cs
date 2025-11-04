using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class Boss : MonoBehaviour
{
    public bool done = false;
    private Player playerScript;
    private PlayerInventory playerInventoryScript;
    [SerializeField] public GameObject Bed;
    [SerializeField] public GameObject Cabinet;
    [SerializeField] public GameObject Wardrode;
    private BossRoomObject BedScript;
    private BossRoomObject CabinetScript;
    private BossRoomObject WardrodeScript;
    [SerializeField] public float cooldown = 10f;
    [SerializeField] public float cooldownglobal = 30f;
    [SerializeField] public float animcooldown = 10f;
    private float cool = 0;
    float currentTime = 0f;
    public Vector3 offset = new Vector3(0.5f, 0, 0);
    [SerializeField] public GameObject spawninfo;

    private List<System.Func<bool>> conditions = new List<System.Func<bool>>();
    private List<System.Func<bool>> conditionsglobal = new List<System.Func<bool>>();

    [SerializeField] public int chanceglobal = 10;
    [SerializeField] public int arraysize = 5;
    [SerializeField] private GameObject[] allItems;
    [SerializeField] public GameObject clock;
    [SerializeField] private string[] conditionDescriptions = new string[]
    {
        "Ты должен стоять на месте",
        "Руки должны быть пустыми",
        "Неси минимум 3 предмета",
        "Заправь мою кровать",
        "Приберись в тумбочке",
        "Приберись у меня в шкафу",
        "Работай"
    };

    [SerializeField] private TextMeshProUGUI uiText;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        playerScript = playerObj.GetComponent<Player>();
        playerInventoryScript = playerObj.GetComponent<PlayerInventory>();
        BedScript = Bed.GetComponent<BossRoomObject>();
        CabinetScript = Cabinet.GetComponent<BossRoomObject>();
        WardrodeScript = Wardrode.GetComponent<BossRoomObject>();
        conditions.Add(Condition1);
        conditions.Add(Condition2);
        conditions.Add(Condition3);
        conditions.Add(Condition4);
        conditions.Add(Condition5);
        conditions.Add(Condition6);
        conditions.Add(Condition7);

        StartCoroutine(Cooldown());

    }
    void Update()
    {
        if (cool != 0)
        {
            clockrotate(cool);
        }
    }
    private bool Condition1()
    {
        if (playerScript.speed == 0)
        {
            return true;
        }
        else return false;
    }
    private bool Condition2()
    {
        if (playerInventoryScript.heldItems.Count != 0)
        {
            return false;
        }
        else return true;
    }
    private bool Condition3()
    {
        if (playerInventoryScript.heldItems.Count >= 3)
        {
            return true;
        }
        else return false;
    }
    private bool Condition4()
    {
        return BedScript.done;
    }
    private bool Condition5()
    {
        return CabinetScript.done;
    }
    private bool Condition6()
    {
        return WardrodeScript.done;
    }
    private bool Condition7()
    {
        return playerScript.isWork;
    }
    private IEnumerator Cooldown()
    {
        while (true)
        {
            int randomglobal = Random.Range(0, chanceglobal);
            if (randomglobal >= 1)
            {
                int randomIndex = Random.Range(0, conditions.Count);
                uiText.text = $"Условие: \n{conditionDescriptions[randomIndex]}";
                cool = cooldown;
                if (randomIndex == 3) BedScript.done = false;
                if (randomIndex == 4) CabinetScript.done = false;
                if (randomIndex == 5) WardrodeScript.done = false;
                yield return new WaitForSeconds(cooldown);
                cool = 0;
                currentTime = 0f;
                bool result = conditions[randomIndex].Invoke();
                Debug.Log($"Проверка условия #{randomIndex + 1}: результат = {result}");


                if (result)
                {
                    Debug.Log("Условие выполнено!");
                    ScoreManagerTMP.Instance.AddScore(200);
                    AnimFinger();
                    uiText.text = $" ";
                }
                else Lose();
            } else
            {
                List<int> inventory = new List<int>();
                int randomIndex = Random.Range(3, arraysize+1);
                for (int i = 0; i < randomIndex; i++)
                {
                    int randomValue = Random.Range(0, 4);
                    inventory.Add(randomValue);
                    Debug.Log($"число{randomValue}");
                }
                uiText.text = $"хочу такую башню";
                foreach (int n in inventory) {
                    GameObject newItem = Instantiate(allItems[n], spawninfo.transform.position + offset * n, Quaternion.identity);
                    }
                cool = cooldownglobal;
                yield return new WaitForSeconds(cooldownglobal);
                cool = 0;
                currentTime = 0f;
                List<int> indices = playerInventoryScript.GetCurrentItemIndices();
                if (inventory.SequenceEqual(indices))
                {
                    Debug.Log("Условие выполнено!");
                    ScoreManagerTMP.Instance.AddScore(500);
                    AnimFinger();
                    uiText.text = $" ";
                }
                else Lose();
            }

            
        }
    }
    private void Lose()
    {
        Debug.Log("Проиграл");
    }
    private void AnimFinger()
    {

    }
    private void clockrotate(float cool)
    {
        if (cool <= 0f) return;

        currentTime += Time.deltaTime;
        float fraction = currentTime / cool;

        fraction = fraction % 1f;

        float angle = fraction * 360f;
        clock.transform.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
