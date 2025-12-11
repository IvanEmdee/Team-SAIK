using UnityEngine;

public class playerAudio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource source;
    [SerializeField] AudioClip step;
    [SerializeField] AudioClip takeDamage;


    void Start()
    {
        source = GetComponent<AudioSource>();
        Debug.Log("Player audio assigned");
    }
    
    public void playStep()
    {
        source.PlayOneShot(step);
    }
    public void playTakeDamage()
    {
        source.PlayOneShot(takeDamage);
    }
}
