using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class CharacterBase : MonoBehaviour
{
    public Guid Id;
    public WeaponController WeaponController;
    public BoosterContainer BoosterContainer;
    public CharacterController Controller;
    public HealthManager HealthManager;
    public Transform ShootOrigin;
    public float Gravity = -9.81f * 2;
    public float MoveSpeed = 5f;
    public float JumpSpeed = 5f;
    public abstract CharacterKind CharacterKind { get; }
    public abstract int Team { get; protected set; }

    public bool IsCanAttackOther(CharacterBase other)
    {
        var isBotVsBot = other.CharacterKind == CharacterKind.bot && CharacterKind == CharacterKind.bot;

        // bot can not
        if (isBotVsBot)
            return false;

        var isTeammates = other.Team == Team;

        // players in one team can not attack teammate
        if (isTeammates)
            return false;

        // in other cases can attack
        return true;
    }

    public void ChooseWeapon(int leftOrRigth)
    {
        WeaponController.ChangeWeapon(leftOrRigth);
        RoomSendClient.PlayerChooseWeapon(this);
    }

    protected void TryChooseWeaponByIndex(int index)
    {
        WeaponController.TryChooseWeaponByIndex(index, out var result);

        if (result)
        {
            RoomSendClient.PlayerChooseWeapon(this);
        }
    }

    public void MoveTo(Vector3 position)
    {
        Controller.enabled = false;
        transform.position = position;
        Controller.enabled = true;

        RoomSendClient.PlayerPosition(this);
    }

    public void Shoot(Vector3 viewDuraction)
    {
        if (HealthManager.IsDie)
            return;

        Vector3 pos = this.transform.position;
        Debug.DrawLine(pos, pos + viewDuraction * 10, Color.green, Mathf.Infinity);

        WeaponController.GetCurrentWeapon().Shoot(this, viewDuraction, ShootOrigin.position);
    }

    public void TakeDamage(float damage, CharacterBase attacker)
    {
        Debug.Log("Character taking damage!!");

        if (HealthManager.IsDie)
            return;

        HealthManager.TakePureDamage(damage);

        TakeDamagePostprocess(attacker);
    }

    protected virtual void TakeDamagePostprocess(CharacterBase attacker) { }
}
