using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class Guard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Transform player;
    [SerializeField] GameObject GuardPrefab;
    [SerializeField] GameObject coolDown;
    private CharacterAnimator charAnim;
    [SerializeField] float slashDistance = 0.1f;
    private GameObject activeShield;

    [Header("Audio")]
    [SerializeField] cardAudio audio;

    public void Start()
    {
        charAnim = player.GetComponent<CharacterAnimator>();
    }
    public async void OnPointerClick(PointerEventData eventData)
    {
        Destroy(activeShield);
        coolDown.SetActive(true);
        Debug.LogWarning("clicked");
        ActivateGuard();

        //cooldown
        await Task.Delay(1000);
        coolDown.SetActive(false);


    }
    //attack logic
    void ActivateGuard()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned");
            return;
        }
        if (GuardPrefab == null)
        {
            Debug.LogWarning("Guard Effect prefab is not assigned.");
            return;
        }

        Vector2 facing = new Vector2(charAnim.MoveX, charAnim.MoveY);

        // Default to up if stationary
        if (facing == Vector2.zero) facing = Vector2.up;

        // Snap to cardinal directions
        if (Mathf.Abs(facing.x) >= Mathf.Abs(facing.y))
            facing = new Vector2(Mathf.Sign(facing.x), 0f); // horizontal
        else
            facing = new Vector2(0f, Mathf.Sign(facing.y));  // vertical

        // Compute spawn position
        Vector3 spawnPos = player.position + (Vector3)(facing * slashDistance);
        audio.playGuard();

        // Instantiate slash
        activeShield = Instantiate(GuardPrefab, spawnPos, Quaternion.identity);
        // Determine rotation based on facing
        float angle = 0f;
        if (facing.y > 0) angle = 0f;        // Up
        else if (facing.x < 0) angle = 90f;  // Left
        else if (facing.x > 0) angle = -90f; // Right
        else if (facing.y < 0) angle = 180f; // Down

        activeShield.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    public void DestroyShield()
    {
        if (activeShield != null)
        {
            Destroy(activeShield);
            activeShield = null;  
        }
        
    }
}
