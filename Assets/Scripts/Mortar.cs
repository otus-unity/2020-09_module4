using Photon.Pun;
using UnityEngine;

public class Mortar : AbstractWeapon
{
    public Transform source;
    /**
     * добавьте в игру тип оружия "миномет": при выстреле создается мина, которая ведет себя как физический объект -
     * отскакивает от пола и от стен, но взрывается при столкновении с персонажем,
     * а также - через заданное количество секунд, даже если не произошло столкновения с персонажем.
     * При взрыве, также как и ракета, мина наносит повреждения всем персонажам вокруг эпицентра взрыва
     */
    public override void Shoot() {
        var obj = PhotonNetwork.Instantiate("shellNew", source.position + Vector3.forward, source.rotation);
        // have to apply Impulse
        Vector3 impulse = source.transform.forward * 20 + Vector3.up * 5;
        obj.GetComponentInChildren<Rigidbody>().AddForce(impulse, ForceMode.Impulse);
    }
}