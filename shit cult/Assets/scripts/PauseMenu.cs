using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private Scenes sceneManager;

    void Start()
    {
        sceneManager = Object.FindFirstObjectByType<Scenes>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            sceneManager.ReturnToMenu();
    }

}
