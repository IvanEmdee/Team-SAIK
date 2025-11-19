using System;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Cards", menuName = "Cards/Create a new Card")]
public class Cards : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] cardAttribute cardType;
    [SerializeField] int attack;
    [SerializeField] Sprite cardArt;
    [SerializeField] Animation cardAnimation;
    [SerializeField] int cost;

    //Exposing the variables
    public string getName
    {
        get { return name; }
    }
    public cardAttribute getCardType
    {
        get { return cardType; }
    }
    public int getAttack
    {
        get { return attack; }
    }
    public Animation getCardAnimation
    {
        get { return cardAnimation; }
    }
    public int getCost
    {
        get { return cost; }
    }
    public Sprite getCardArt
    {
        get{ return cardArt; }
    }
    public void performEffect()
    {
        Debug.Log("Effect is being performed.");
    }
    

    //types
    public enum cardAttribute
    {
        Attack,
        Defend,
        Functional
    }

}
