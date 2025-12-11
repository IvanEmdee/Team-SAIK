using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slide : MonoBehaviour, IPointerClickHandler
{
    public PlayerController2D player; // Assign your player in Inspector
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private Vector2 dashDirection;
    [SerializeField] GameObject coolDown;
    [SerializeField] PlayerHealth iFrame;

    [Header("Audio")]
    [SerializeField] cardAudio audio;

    void FixedUpdate()
    {
        if (!isDashing) return;

        // Keep dash velocity
        player.rb.linearVelocity = dashDirection * dashSpeed;

        dashTimer -= Time.fixedDeltaTime;
        if (dashTimer <= 0f)
        {
            isDashing = false;
            player.disableMovement = false;
            player.rb.linearVelocity = Vector2.zero; // stop movement after dash
        }
    }

    // This is called automatically when the Image is clicked
    public async void OnPointerClick(PointerEventData eventData)
    {
        coolDown.SetActive(true);

        if (player == null || isDashing)
        {
            coolDown.SetActive(false);
            return;
        }


        Vector2 inputDir = player.GetMovementDirection();
        if (inputDir == Vector2.zero)
        {
            coolDown.SetActive(false);
            return; // ignore if no input
        }
        
        dashDirection = inputDir.normalized;
        dashTimer = dashTime;
        isDashing = true;
        iFrame.invincibilityTimer = 0.25f;
        iFrame.isInvincible = true;
        // Disable normal movement
        player.disableMovement = true;

        // Apply initial dash velocity
        audio.playSlide();
        player.rb.linearVelocity = dashDirection * dashSpeed;
        iFrame.invincibilityTimer = 0.25f;
        iFrame.isInvincible = true;
        await Task.Delay(3000);
        coolDown.SetActive(false);
    }
}
