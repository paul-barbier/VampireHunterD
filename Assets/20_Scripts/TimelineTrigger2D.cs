using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider2D))]
public class TimelineTrigger2D : MonoBehaviour
{
    public PlayableDirector director;
    public bool playOnce = true;

    private bool _hasPlayed = false;

    private void Awake()
    {
        if (director != null)
            director.Stop();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasPlayed && playOnce)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (director != null)
        {
            director.Play();
            _hasPlayed = true;
        }
    }
}
