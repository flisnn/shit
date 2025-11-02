using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;
    private void Awake()
    {
        if (!player)
            player = Object.FindAnyObjectByType<Player>().transform;
    }

    private void Update()
    {
        pos = player.position;
        pos.z -= 1;
        transform.position = pos;
    }
}
