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
public class Top10
{
    public bool status;
    public List<PlayerBackup> data;
}
[Serializable]
public class loginorregister
{
    public bool status;
    public Player data;
}
