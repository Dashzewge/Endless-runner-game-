
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver = false;
    public static bool isGameStarted = false;
    public static int numberOfCoins = 0;

    [Header("UI References")]
    public GameObject gameOverPanel;
    public GameObject startingText;
    public TMP_Text coinsText;

    private bool gameOverSoundPlayed = false;

    void Start()
    {
        gameOver = false;
        isGameStarted = false;
        numberOfCoins = 0;
        gameOverSoundPlayed = false;

        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (startingText != null)
            startingText.SetActive(true);

        UpdateCoinsText();
    }

    void Update()
    {
        if (gameOver)
        {
            if (!gameOverSoundPlayed)
            {
                gameOverSoundPlayed = true;

                AudioManager audioManager =
                    FindObjectOfType<AudioManager>();

                if (audioManager != null)
                    audioManager.PlaySound("GameOver");
                else
                    Debug.LogWarning("AudioManager олдсонгүй!");
            }

            Time.timeScale = 0f;

            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);

            return;
        }

        UpdateCoinsText();

        if (!isGameStarted && SwipeManager.tap)
        {
            isGameStarted = true;

            if (startingText != null)
                startingText.SetActive(false);
        }
    }

    void UpdateCoinsText()
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + numberOfCoins;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        gameOver = false;
        isGameStarted = false;
        numberOfCoins = 0;
        gameOverSoundPlayed = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
