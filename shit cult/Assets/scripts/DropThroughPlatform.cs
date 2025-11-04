using UnityEngine;
using System.Collections;

public class DropThroughPlatform : MonoBehaviour
{
    private Collider2D playerCollider;
    private Collider2D currentPlatform;
    private Collider2D currentPlatform_bufffer;

    private void Start()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Когда игрок нажимает S — падаем вниз
        if (Input.GetKey(KeyCode.S)&& currentPlatform != null)
        {
            StartCoroutine(DisableCollisionTemporarily());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, есть ли у объекта тег "Platform"
        if (collision.collider.CompareTag("Platform"))
        {
            currentPlatform = collision.collider;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider == currentPlatform)
        {
            currentPlatform = null;
        }
    }

    private IEnumerator DisableCollisionTemporarily()
    {
        Physics2D.IgnoreCollision(playerCollider, currentPlatform, true);
        currentPlatform_bufffer = currentPlatform;
        yield return new WaitForSeconds(0.6f); // время, пока игрок проходит
        Physics2D.IgnoreCollision(playerCollider, currentPlatform_bufffer, false);
    }
}
