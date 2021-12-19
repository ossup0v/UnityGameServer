using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int Id;
    public WeaponController WeaponController;
    public BoosterContainer BoosterContainer;
    public CharacterController Controller;
    public HealthManager HealthManager = new HealthManager();
    public Transform ShootOrigin;
    
    public float Gravity = -9.81f * 2;
    public float MoveSpeed = 5f;
    public float JumpSpeed = 5f;

    private void Start()
    {
        Gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        MoveSpeed *= Time.fixedDeltaTime;
        JumpSpeed *= Time.fixedDeltaTime;
    }

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

        WeaponController.GetCurrentWeapon().Shoot(this, viewDuraction, ShootOrigin.position);
    }
    public void TakeDamage(float damage, int? attackerPlayerId)
    {
        Debug.Log("Player taking damage!!");

        if (HealthManager.IsDie)
            return;

        HealthManager.TakePureDamage(damage);
        
        ServerSend.PlayerHealth(HealthManager);

        TakeDamagePostprocess(attackerPlayerId);
    }

    protected virtual void TakeDamagePostprocess(int? attackerId) { }
}
