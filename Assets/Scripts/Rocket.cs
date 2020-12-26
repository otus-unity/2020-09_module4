using System;
using Photon.Pun;
using UnityEngine;

public class Rocket : MonoBehaviour, IHittable
{
    public float speed = 3f;
    public int damage = 10;
    public float radius = 3f;
    public ParticleSystem explosionEffect;
    public Transform visual;

    private float _createdAt;
    private bool _inMotion;

    private void Start() {
        _createdAt = Time.time;
        _inMotion = true;
    }

    private void Update() {
        if (_inMotion) {
            // have to adjust the vector with the speed value 
            transform.Translate(transform.forward, Space.World);

            // self detonation 
            if (Time.time - _createdAt > 10f) {
                Explosion();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        HitObject(other);
    }


    public void HitObject(Collider hit) {
        // Debug.Log("hit object "+ hit.gameObject.name);

        // looking for players around to apply damage from explosion
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        float radiusSqr = radius * radius;
        foreach (Collider hitCollider in hitColliders) {
            if (hitCollider.gameObject &&
                hitCollider.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth health) &&
                hitCollider.gameObject.TryGetComponent<PhotonView>(out PhotonView view)) {
                float distanceSqr = (transform.position - hitCollider.gameObject.transform.position).sqrMagnitude;

                // far from explosion center will deal less damage
                int realDamage = Mathf.CeilToInt(damage / (radiusSqr - distanceSqr));

                view.RPC("DamageRPC", RpcTarget.All, realDamage);
            }
        }

        Explosion();
    }

    public void CollideObject(Collision other) {
        
    }

    private void Explosion() {
        _inMotion = false;
        Destroy(gameObject, 3f);
        explosionEffect.Play();
        visual.gameObject.SetActive(false);
    }
}