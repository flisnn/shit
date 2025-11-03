using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public bool done = false;
    public Player playerScript;
    public PlayerInventory playerInventoryScript;
    [SerializeField] public Transform TPpositionPlayer;
    [SerializeField] public float cooldown = 2f;
    
}
