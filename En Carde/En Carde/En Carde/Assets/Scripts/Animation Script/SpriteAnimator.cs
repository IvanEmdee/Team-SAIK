using System.Collections.Generic;
using NUnit;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteAnimator
{
    SpriteRenderer spriteRenderer;
    List<Sprite> frame;
    float frameRate;

    int currentFrame;
    float timer;

    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate = 0.16f)
    {
        this.frame = frames;
        this.spriteRenderer = spriteRenderer;
        this.frameRate = frameRate;
    }

    public void Start()
    {
        currentFrame = 0;
        timer = 0;
        spriteRenderer.sprite = frame[0];
    }
    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % frame.Count;
            spriteRenderer.sprite = frame[currentFrame];
            timer -= frameRate;
            Debug.Log("reached innner update");
        }
    }
    public List<Sprite> Frames
    {
        get { return frame; }
    }

}
