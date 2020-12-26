using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableCollider : MonoBehaviour
{
    private IHittable _shell => GetComponentInParent<IHittable>();

    private void OnTriggerEnter(Collider other) {
        _shell.HitObject(other);
    }

    private void OnCollisionEnter(Collision other) {
        _shell.CollideObject(other);
    }
}
