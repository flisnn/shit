using UnityEngine;
using TMPro;

public class ScoreManagerTMP : MonoBehaviour
{
    public static ScoreManagerTMP Instance;

    [Header("UI элементы (TextMeshPro)")]
    public TMP_Text scoreText;      // Текущие очки во время игры
    public TMP_Text bestScoreText;  // Рекорд (в меню)
    public TMP_Text lastScoreText;  // Последний результат (в меню)

    private int score = 0;
    private int bestScore = 0;
    private int lastScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        lastScore = PlayerPrefs.GetInt("LastScore", 0);
    }

    private void Start()
    {
        UpdateUI();
    }

    // Добавить очки
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    // Обновить текст
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Очки: {score}";

        if (bestScoreText != null)
            bestScoreText.text = $"Рекорд: {bestScore}";

        if (lastScoreText != null)
            lastScoreText.text = $"Последний: {lastScore}";
    }

    // Вызывается при поражении / завершении игры
    public void EndGame()
    {
        lastScore = score;

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        PlayerPrefs.SetInt("LastScore", lastScore);
        PlayerPrefs.Save();

        score = 0;
        UpdateUI();
    }

    public int GetScore() => score;
}
