using Photon.Pun;
using UnityEngine;

public class Shell : MonoBehaviour, IHittable
{
    public int damage = 20;
    public float radius = 5f;
    public float detonateTime = 4f;
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
            // self detonation 
            if (Time.time - _createdAt > detonateTime) {
                Explosion();
            }
        }
    }

    public void HitObject(Collider other) {
        // Debug.Log("hit object "+ other.gameObject.name);
        
        // detonate only if character was hit
        if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            Explosion();
    }

    public void CollideObject(Collision other) {
        // Debug.Log("hit object "+ other.gameObject.name);
        // detonate only if character was hit
        if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            Explosion();
    }

    private void Explosion() {
        
        // looking for players around to apply damage from explosion
        Collider[] hitColliders = Physics.OverlapSphere(visual.transform.position, radius);
        float radiusSqr = radius * radius;
        foreach (Collider hitCollider in hitColliders) {
            if (hitCollider.gameObject &&
                hitCollider.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth health) &&
                hitCollider.gameObject.TryGetComponent<PhotonView>(out PhotonView view)) {
                float distanceSqr = (visual.transform.position - hitCollider.gameObject.transform.position).sqrMagnitude;

                // far from explosion center will deal less damage
                int realDamage = Mathf.CeilToInt(damage / (radiusSqr - distanceSqr));

                view.RPC("DamageRPC", RpcTarget.All, realDamage);
            }
        }
        
        
        _inMotion = false;
        Destroy(gameObject, 3f);
        explosionEffect.gameObject.transform.position = visual.position;
        explosionEffect.Play();
        visual.gameObject.SetActive(false);
    }
}