using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayRandom : MonoBehaviour
{
    public SoundType soundType;

    [Header("Timer settings")]
    [SerializeField] private float minDelay = 1f;
    [SerializeField] private float maxDelay = 5f;

    private float remainingTime;

    public void Start()
    {
        remainingTime = Random.Range(minDelay, maxDelay);
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            SoundManager.PlaySound(soundType);
            remainingTime = Random.Range(minDelay, maxDelay);
        }
    }
}

