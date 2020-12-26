using UnityEngine;

public interface IHittable
{
    public void HitObject(Collider other);
    public void CollideObject(Collision other);
}