using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class CharacterBase : MonoBehaviour
{
    public int Id;
    public WeaponController WeaponController;
    public BoosterContainer BoosterContainer;
    public CharacterController Controller;
    public HealthManager HealthManager = new HealthManager();
    public Transform ShootOrigin;
    public abstract CharacterKind CharacterKind { get; }

    public float Gravity = -9.81f * 2;
    public float MoveSpeed = 5f;
    public float JumpSpeed = 5f;

    public void ChooseWeapon(int leftOrRigth)
    {
        WeaponController.ChangeWeapon(leftOrRigth);
        ServerSend.PlayerChooseWeapon(this);
    }

    public void MoveTo(Vector3 position)
    {
        Controller.enabled = false;
        transform.position = position;
        Controller.enabled = true;

        ServerSend.PlayerPosition(this);
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
