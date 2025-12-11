using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class slashCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Sprite cardImage;
    [SerializeField] string name;
    [SerializeField] int cost;
    [SerializeField] string effect;
    [SerializeField] int attack;
    [SerializeField] Cards card;
    [SerializeField] GameObject attackSprite;

    //Attack references
    [Header("Reference")]
    [SerializeField] Transform player;
    [SerializeField] GameObject slashEffectPrefab;
    private CharacterAnimator charAnim;

    [Header("Attack Settings")]
    [SerializeField] float slashDistance = 1f;
    [SerializeField] int attackDamage = 5;

    //audio



    public void Setup(Cards card)
    {
        this.card = card;
        cardImage = card.getCardArt;
        name = card.getName;
        cost = card.getCost;
        attack = card.getAttack;
    }
    public void Start()
    {
        charAnim = player.GetComponent<CharacterAnimator>();
        Setup(card);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogWarning("clicked");
        PlaySlashAttack();
    }
    //attack logic
    void PlaySlashAttack()
    {
        if(player == null)
        {
            Debug.LogWarning("Player not assigned");
            return;
        }
        if(slashEffectPrefab == null)
        {
            Debug.LogWarning("Slash Effect prefab is not assigned.");
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

        // Instantiate slash
        GameObject slash = Instantiate(slashEffectPrefab, spawnPos, Quaternion.identity);

        // Determine rotation based on facing
        float angle = 0f;
        if (facing.y > 0) angle = 0f;        // Up
        else if (facing.x < 0) angle = 90f;  // Left
        else if (facing.x > 0) angle = -90f; // Right
        else if (facing.y < 0) angle = 180f; // Down

        slash.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        slash.GetComponent<SlashEffect>().damage = attackDamage;
    }
}
