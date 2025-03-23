using Fusion;
using UnityEngine;

//class này dùng để spawn player vào trong network
public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    //khi vào mạng thì sẽ tạo nhân vật cho người chơi
    public void PlayerJoined(PlayerRef player)
    {
        //kiểm tra xem người chơi này có phải là người đang chơi không
        if (player == Runner.LocalPlayer)
        {
            //tạo nhân vật ở vị trị (0,1,0);
            //gọi api lấy thông tin player
            var position = new Vector3(0, 0.98f, 0);
            //spawn nhân vật ở vị trí này
            Runner.Spawn(
                PlayerPrefab,
                position,
                Quaternion.identity,
                Runner.LocalPlayer,
                (runner, obj) =>
                {
                    var playerSetup = obj.GetComponent<PlayerSetup>();
                    if (playerSetup != null)
                    {
                        playerSetup.SetupCamera();
                        playerSetup.SetupStats();
                    }
                });
        }

    }
}
