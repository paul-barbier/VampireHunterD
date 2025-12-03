using Unity.VisualScripting;
using UnityEngine;

public class PlayFootsteps : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlaySound()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0,-1), 3f, LayerMask.GetMask("PremierPlan", "SecondPlan"));
        string groundType = hit.collider.gameObject.tag;

        if (groundType == null)
            return;

        if (groundType == "Wood")
        {
            SoundManager.PlaySound(SoundType.WoodFootsteps, 0.4f);
            SoundManager.PlaySound(SoundType.JacketSound, 3f);
            //Debug.Log("kebrezegerh");
        }
        else if (groundType == "Tile")
        {
            SoundManager.PlaySound(SoundType.TilesFootsteps, 0.4f);
            SoundManager.PlaySound(SoundType.JacketSound, 1f);
        }
        else if (groundType == "Catacombes")
        {
            SoundManager.PlaySound(SoundType.CataFootsteps, 0.4f);
            SoundManager.PlaySound(SoundType.JacketSound, 1f);
        }
    }

   

    private void Update()
    {
        Debug.DrawRay(transform.position, new Vector2(0, -1) * 3f, Color.red);
    }

    
}
