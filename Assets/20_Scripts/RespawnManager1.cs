using UnityEngine;

public class RespawnManager1 : MonoBehaviour
{
    [SerializeField] private SpriteRenderer RespawnAnim;
    [SerializeField] private float _respawnDelay;

    private float time;

    private void Respawn()
    {
        if (RespawnAnim == false)
        {
            time += Time.deltaTime;
            if (time >= _respawnDelay)
            {
                RespawnAnim.enabled = false;
                time = 0f;
            }
        }
    }
}
