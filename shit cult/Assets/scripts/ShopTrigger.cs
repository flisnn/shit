using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        shopManager.TryDeliverItem();
    }
  
}
