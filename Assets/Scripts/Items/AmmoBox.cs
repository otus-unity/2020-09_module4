using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AmmoBox : MonoBehaviourPun, IPunObservable
{
    public GameObject visual;
    [Range(0, PlayerAmmo.Max)] public int amount;
    public float respawnTime;
    float timeLeftUntilRespawn;

    public bool IsActive => visual.activeSelf;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
            timeLeftUntilRespawn = (float)stream.ReceiveNext();
        else
            stream.SendNext(timeLeftUntilRespawn);
    }

    void OnTriggerEnter(Collider other)
    {
        if (timeLeftUntilRespawn > 0.0f)
            return;

        if (other.TryGetComponent<PlayerAmmo>(out var ammo) && other.TryGetComponent<PhotonView>(out var view)) {
            timeLeftUntilRespawn = respawnTime;
            this.photonView.RPC("CollectAmmoRPC", RpcTarget.All);
            view.RPC("GiveAmmoRPC", RpcTarget.All, amount);
        }
    }

    [PunRPC]
    void CollectAmmoRPC()
    {
        timeLeftUntilRespawn = respawnTime;
        visual.SetActive(false);
    }

    [PunRPC]
    void RespawnAmmoRPC()
    {
        timeLeftUntilRespawn = 0.0f;
        visual.SetActive(true);
    }

    void Update()
    {
        if (photonView.IsMine && timeLeftUntilRespawn > 0.0f) {
            timeLeftUntilRespawn -= Time.deltaTime;
            if (timeLeftUntilRespawn <= 0.0f)
                photonView.RPC("RespawnAmmoRPC", RpcTarget.All);
        }
    }
}
