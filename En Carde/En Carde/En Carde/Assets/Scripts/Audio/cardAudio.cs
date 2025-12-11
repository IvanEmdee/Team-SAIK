using UnityEngine;

public class cardAudio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource source;
    [SerializeField] AudioClip slash;
    [SerializeField] AudioClip slide;
    [SerializeField] AudioClip guard;


    void Start()
    {
        source = GetComponent<AudioSource>();
        Debug.Log("Card audio assigned");
    }
    
    public void playSlash()
    {
        source.PlayOneShot(slash);
    }
    public void playSlide()
    {
        source.PlayOneShot(slide);
    }
    public void playGuard()
    {
        source.PlayOneShot(guard);
    }
}

