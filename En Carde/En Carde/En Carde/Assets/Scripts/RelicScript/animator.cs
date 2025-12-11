using UnityEngine;
using System.Collections.Generic;

public class crownScript : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    SpriteAnimator spriteAnimator;
    void Start()
    {
        spriteAnimator = new SpriteAnimator(sprites, GetComponent<SpriteRenderer>());
        spriteAnimator.Start();
    }
    void Update()
    {
        spriteAnimator.HandleUpdate();
    }
}
