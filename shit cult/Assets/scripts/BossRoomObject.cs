using UnityEngine;
using System.Collections;

public class BossRoomObject : Interactable
{
    public bool done = true;
    public Player playerScript;
    public PlayerInventory playerInventoryScript;
    [SerializeField] public Transform TPpositionPlayer;
    [SerializeField] public float cooldown = 2f;
    public override void Use()
    {
        if (!done)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            playerScript = playerObj.GetComponent<Player>();
            playerInventoryScript = playerObj.GetComponent<PlayerInventory>();
            playerScript.isWork = true;
            playerScript.speed = 0;
            playerObj.transform.position = TPpositionPlayer.position;
            StartCoroutine(Working());
        }
    }
    private IEnumerator Working()
    {
        Debug.Log("Действие началось");
        yield return new WaitForSeconds(cooldown);
        Debug.Log("Действие завершено");
        playerScript.isWork = false;
        done = true;
    }
}
