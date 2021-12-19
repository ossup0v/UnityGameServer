using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public WeaponController WeaponController;
    public BoosterContainer BoosterContainer;

    public MeshRenderer Model;

    public int Id;
    public string Username;
    public CharacterController Controller;
    public Transform ShootOrigin;
    public float Gravity = -9.81f * 2;
    public float MoveSpeed = 5f;
    public float JumpSpeed = 5f;
    public float ThrowForce = 600f * 2 * 4 * 10;
    public float ShiftMultiplayer = 2f;
    public Vector3[] SpawnPoints;
    public float RespawnTime = 2f;
    public int grenadeCount = 0;
    public int maxItemAmount = 3;
    float controllHeight;
    private bool[] _inputs;
    private float yVelocity = 0;

    //health
    public HealthManager HealthManager = new HealthManager();


    private void Start()
    {
        Gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        MoveSpeed *= Time.fixedDeltaTime;
        JumpSpeed *= Time.fixedDeltaTime;
    }

    internal bool AttemptPickupItem()
    {
        if (grenadeCount >= maxItemAmount)
            return false;

        grenadeCount++;
        ServerSend.PlayerGrenadeCount(Id, grenadeCount);

        return true;
    }

    public void Initialize(int id, string username)
    {
        Id = id;
        Username = username;
        WeaponController = new WeaponController(new List<WeaponBase> { new RocketLaucnherWeapon(), new TeleportWeapon() });
        BoosterContainer = new BoosterContainer();
        HealthManager.OwnerId = Id;
        _inputs = new bool[7];
        controllHeight = Controller.height;
    }

    public void FixedUpdate()
    {
        if (HealthManager.IsDie)
            return;

        HealthManager.Tick();

        Vector2 inputDirection = Vector2.zero;
        if (_inputs[0])
        {
            inputDirection.y += 1;
        }
        if (_inputs[1])
        {
            inputDirection.y -= 1;
        }
        if (_inputs[2])
        {
            inputDirection.x -= 1;
        }
        if (_inputs[3])
        {
            inputDirection.x += 1;
        }
        if (_inputs[4])
        {
            //space
        }
        if (_inputs[5])
        {
            //shift
        }
        if (_inputs[6])
        {
            //ctrl
        }

        Move(inputDirection);
    }

    public void ChooseWeapon(int leftOrRigth)
    {
        WeaponController.ChangeWeapon(leftOrRigth);
        ServerSend.PlayerChooseWeapon(this);
    }

    private void Move(Vector2 inputDirection)
    {
        Vector3 moveDirection = transform.right * inputDirection.x + transform.forward * inputDirection.y;

        if (GetShitftDown())
            moveDirection *= MoveSpeed * ShiftMultiplayer;
        else
            moveDirection *= MoveSpeed;

        if (Controller.isGrounded)
        {
            yVelocity = 0;
            if (_inputs[4])
            {
                yVelocity = JumpSpeed;
            }
        }

        if (_inputs[6])
        {
            Controller.height = controllHeight / 2;
        }
        else
        {
            Controller.height = controllHeight;
        }

        Model.transform.lossyScale.Set(Model.transform.lossyScale.x, Controller.height, Model.transform.lossyScale.z);

        yVelocity += Gravity;
       
        moveDirection.y += yVelocity;

        Controller.Move(moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
        ServerSend.PlayerScale(this);
    }

    public void MoveTo(Vector3 position)
    {
        Controller.enabled = false;
        transform.position = position;
        Controller.enabled = true;
    }

    public void SetInput(bool[] inputs, Quaternion rotation)
    {
        _inputs = inputs;
        transform.rotation = rotation;
    }

    public void Shoot(Vector3 viewDuraction)
    {
        if (HealthManager.IsDie)
            return;

        WeaponController.GetCurrentWeapon().Shoot(this, viewDuraction, ShootOrigin.position);
    }

    public void ThrowItem(Vector3 viewDuraction)
    {
        if (HealthManager.IsDie)
            return;

        if (grenadeCount > 0)
        {
            grenadeCount--;
            ServerSend.PlayerGrenadeCount(Id, grenadeCount);
            NetworkManager.Instance.InstantiatePrjectile(ShootOrigin)
                .Initialize(viewDuraction, ThrowForce, Id);
        }
    }

    public void TakeDamage(float damage, int? attackerPlayerId)
    {
        Debug.Log("Player taking damage!!");

        if (HealthManager.IsDie)
            return;

        HealthManager.TakePureDamage(damage);

        if (HealthManager.IsDie)
        {
            Controller.enabled = false;
            transform.position = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];

            if (attackerPlayerId.HasValue == false)
            {
                //player suicide
            }
            else if (attackerPlayerId != Id)
            {
                RatingManager.KillAndDeath(attackerPlayerId.Value, Id);
                ServerSend.UpdateRatingTable(attackerPlayerId.Value, Id);
            }
            else
            {
                RatingManager.AddDeath(Id);
                ServerSend.UpdateRatingTableDeath(Id);
            }

            ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }

        ServerSend.PlayerHealth(HealthManager);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(RespawnTime);

        HealthManager.SetPureHealth(HealthManager.MaxHealth);
        Controller.enabled = true;
        ServerSend.PlayerRespawn(this);
    }

    public void Suicide()
    {
        TakeDamage(HealthManager.CurrentHealth, null);
    }

    private bool GetShitftDown() => _inputs[5];
}
