using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;

    private bool isDead = false;

    void Awake()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        if (deathPanel != null)
            deathPanel.SetActive(true);

        isDead = true;

        // Pause the game
        Time.timeScale = 0f;

        // Unlock and show cursor for clarity (optional)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!isDead) return;

        // Restart game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
