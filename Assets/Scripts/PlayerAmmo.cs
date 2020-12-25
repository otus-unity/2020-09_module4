using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAmmo : MonoBehaviour, IPunObservable
{
    public const int Max = 20;

    [Range(0, Max)]
    public int ammo;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
            ammo = (int)stream.ReceiveNext();
        else
            stream.SendNext(ammo);
    }

    [PunRPC]
    void GiveAmmoRPC(int amount)
    {
        ammo += amount;
        if (ammo > Max)
            ammo = Max;
    }
}
