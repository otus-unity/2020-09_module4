using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviour, IPunObservable
{
    public const int Max = 100;

    [Range(0, Max)]
    public int health;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
            health = (int)stream.ReceiveNext();
        else
            stream.SendNext(health);
    }

    [PunRPC]
    void DamageRPC(int damage)
    {
        health -= damage;
        if (health < 0)
            health = 0;
    }

    [PunRPC]
    void GiveHealthRPC(int amount)
    {
        health += amount;
        if (health > Max)
            health = Max;
    }
}
