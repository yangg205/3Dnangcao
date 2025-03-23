using Fusion;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    //setup camera cho nguoi choi
    public void SetupCamera()
    {
        if(Object.HasInputAuthority)
        {
            //tim doi tuong camera
            var camera = FindFirstObjectByType<CameraFollow>();
            if(camera != null)
            {
                camera.AssignCamera(transform);
            }
        }
    }
    //setup stats: health, mana,...
    public void SetupStats()
    {
        var health = FindAnyObjectByType<PlayerProperties>();
        if(health != null)
        {
            health.OnHealthChanged();
        }    
          
    }
    //setup info: ten, mau,...
    //setup items: vu khi, ao giap,...
}
