using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject winUI;
    private bool hasWon = false;

    void Start()
    {
        if (winUI != null)
            winUI.SetActive(false);
    }

    public void ShowWinScreen()
    {
        if (winUI != null)
            winUI.SetActive(true);

        hasWon = true;

        // Freeze the game
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (hasWon && Input.GetKeyDown(KeyCode.Space))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // unfreeze
        SceneManager.LoadScene("TitleScreen");
    }
}
