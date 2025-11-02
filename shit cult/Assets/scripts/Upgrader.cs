using UnityEngine;
using System.Collections;

public class Upgrader : Interactable
{
    public Player playerScript;
    [SerializeField] public Transform TPpositionPlayer;
    public override void Use()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        playerScript = playerObj.GetComponent<Player>();
        playerScript.isWork = true;
        playerScript.speed = 0;
        playerObj.transform.position = TPpositionPlayer.position;
        StartCoroutine(Working());
    }
    private IEnumerator Working()
    {
        Debug.Log("Действие началось");
        yield return new WaitForSeconds(2f);
        Debug.Log("Действие завершено");
        playerScript.isWork = false;
    }
}
