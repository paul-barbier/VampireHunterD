using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("PlayerSpawner et NextSpawnPoint = " + SpawnManager.NextSpawnPoint);

        if (!string.IsNullOrEmpty(SpawnManager.NextSpawnPoint))
        {
            GameObject spawnPoint = GameObject.Find(SpawnManager.NextSpawnPoint);

            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
                Debug.Log("Joueur placé sur : " + SpawnManager.NextSpawnPoint);
            }
            else
            {
                Debug.LogWarning("SpawnPoint non trouvé : " + SpawnManager.NextSpawnPoint);
            }
        }
    }
}
