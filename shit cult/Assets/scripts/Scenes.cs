using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public static bool FromGame = false;      // Флаг — игрок пришёл из игры
    public static string lastGameScene = "";  // Имя последней игровой сцены
    public GameObject continueButton;         // Кнопка "Продолжить" в меню

    private void Start()
    {
        // Показываем кнопку "Продолжить" только если игрок уже был в игре
        if (continueButton != null)
            continueButton.SetActive(FromGame);
    }

    // Запуск новой игры (по номеру сцены)
    public void StartScene(int sceneNumber)
    {
        FromGame = true;
        string sceneName = SceneManager.GetSceneByBuildIndex(sceneNumber).name;
        lastGameScene = sceneName;
        SceneManager.LoadScene(sceneNumber);
    }

    // Продолжить игру
    public void ContinueGame()
    {
        if (!string.IsNullOrEmpty(lastGameScene))
        {
            SceneManager.LoadScene(lastGameScene);
        }
        else
        {
            Debug.LogWarning("❗ Нет сохранённой сцены для продолжения!");
        }
    }

    // Перезапуск игры (например, при поражении)
    public void RestartGame()
    {
        FromGame = false;
        lastGameScene = "";
        SceneManager.LoadScene("MainMenu");
    }

    // Выход из игры
    public void QuitGame()
    {
        Application.Quit();
    }

    // Возврат в меню из игры (через ESC)
    public void ReturnToMenu()
    {
        FromGame = true;
        lastGameScene = SceneManager.GetActiveScene().name;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
