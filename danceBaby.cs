using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class danceBaby : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip backMusic;
    public AudioClip danceMusic;
    public GameObject player;
    private Animator animator;
    private bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = player.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    public void letsDance()
    {
        clicked = !clicked;
        if (clicked)
        {
            animator.SetTrigger("playerDance");
            AudioSource.PlayClipAtPoint(danceMusic, player.transform.position);
        }
        else
        {
            animator.SetTrigger("playerIdle");
            AudioSource.PlayClipAtPoint(backMusic, player.transform.position);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
