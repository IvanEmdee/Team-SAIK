using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Sprite cardImage;
    [SerializeField] string name;
    [SerializeField] int cost;
    [SerializeField] string effect;
    [SerializeField] int attack;
    [SerializeField] Cards card;

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
        Setup(card);
        Debug.Log("Setup completed.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        card.performEffect();
    }
}
