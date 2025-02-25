using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Player
{
    public int playerID;
    public string name;
    public string email;
    public string password;
    public string password_salt;
    public int total_point;
}
[Serializable]
public class PlayerBackup
{
    public string name;
    public int total_point;
    public override string ToString() => $"{name}  {total_point}";
}
[Serializable]
public class Top10Response
{
    public PlayerBackup[] data; // Danh sách player trong BXH
    public string mess; // Tin nhắn phản hồi
}
[Serializable]
public class loginorregister
{
    public bool status;
    public Player data;
}
[System.Serializable]
public class LoginResponse
{
    public LoginData data;
}

[System.Serializable]
public class LoginData
{
    public bool status;
    public string message;
}
[System.Serializable]
public class RegisterResponse
{
    public RegisterData data;
}

[System.Serializable]
public class RegisterData
{
    public bool status;
    public string message;
}

[System.Serializable]
public class UpdatePointResponse
{
    public Player player; // Dữ liệu trả về từ API
}
[System.Serializable]
public class PlayerResponse
{
    public PlayerData data;
}

[System.Serializable]
public class PlayerData
{
    public string name;
    public int total_money;
}
