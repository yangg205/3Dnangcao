using Fusion;
using UnityEngine;
//tao cac doi tuong player trong moi truong mang
public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    //tao doi tuong player
    public void PlayerJoined(PlayerRef player)
    {
        //kiem tra nguoi choi co dang dieu khien hay khong
        if (player == Runner.LocalPlayer)
        {
            var positon = new Vector3(494.799988f, 9, 145.369995f);
            Runner.Spawn(PlayerPrefab, positon, Quaternion.identity);
        }
        //goi api de lay thong tin nguoi choi
        //vi tri tao doi tuong player
    }
}
