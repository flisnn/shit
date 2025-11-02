using UnityEngine;

public class TPdoors : MonoBehaviour
{
    [SerializeField]public Transform TPpositionPlayer;
    [SerializeField]public Transform TPpositionCamera;
    public Player playerScript;
    public PlayerInventory playerInventoryScript;
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            player.transform.position = TPpositionPlayer.position;
            playerInventoryScript = playerObj.GetComponent<PlayerInventory>();
            playerInventoryScript.UpdateItemsFollow();
            {
                for (int i = 0; i < playerInventoryScript.heldItems.Count; i++)
                {
                    GameObject item = playerInventoryScript.heldItems[i];
                    Vector3 endpos = new Vector3(0f, 2f, 0f); 
                    endpos = player.transform.position + playerInventoryScript.offset;
                    endpos.y += playerInventoryScript.itemHeight * i;
                    item.transform.position = endpos;
                }
            }
            Camera mainCam = Camera.main;
            Vector3 cameraPos = TPpositionCamera.position;
            cameraPos.z = -5;
            mainCam.transform.position = cameraPos;
        }
    }
}
