using UnityEngine;

public class PickupWin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WinScreen winScreen = FindFirstObjectByType<WinScreen>();
            if (winScreen != null)
                winScreen.ShowWinScreen();

            Destroy(gameObject);
        }
    }
}