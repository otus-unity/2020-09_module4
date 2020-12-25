using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthBox : MonoBehaviourPun, IPunObservable
{
    public GameObject visual;
    [Range(0, PlayerHealth.Max)] public int amount;
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

        if (other.TryGetComponent<PlayerHealth>(out var health) && other.TryGetComponent<PhotonView>(out var view)) {
            timeLeftUntilRespawn = respawnTime;
            this.photonView.RPC("CollectHealthRPC", RpcTarget.All);
            view.RPC("GiveHealthRPC", RpcTarget.All, amount);
        }
    }

    [PunRPC]
    void CollectHealthRPC()
    {
        timeLeftUntilRespawn = respawnTime;
        visual.SetActive(false);
    }

    [PunRPC]
    void RespawnHealthRPC()
    {
        timeLeftUntilRespawn = 0.0f;
        visual.SetActive(true);
    }

    void Update()
    {
        if (photonView.IsMine && timeLeftUntilRespawn > 0.0f) {
            timeLeftUntilRespawn -= Time.deltaTime;
            if (timeLeftUntilRespawn <= 0.0f)
                photonView.RPC("RespawnHealthRPC", RpcTarget.All);
        }
    }
}
