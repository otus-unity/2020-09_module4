using Photon.Pun;
using UnityEngine;

public class RocketLauncher : AbstractWeapon
{
    public Transform source;
    public int damage;

    /**
     * добавьте в игру тип оружия "ракетница": при выстреле создается ракета, которая летит по прямой и
     * взрывается при столкновении с любым препятствием, нанося повреждение всем персонажам
     * на небольшом удалении от места столкновения.
     */
    public override void Shoot() {
        var obj = PhotonNetwork.Instantiate("rocket", source.position + Vector3.forward, source.rotation);
    }
}