using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public string Username;
    public float ThrowForce = 600f * 2 * 4 * 10;
    public float ShiftMultiplayer = 2f;
    public Vector3[] SpawnPoints;
    public float RespawnTime = 2f;
    public int grenadeCount = 0;
    public int maxItemAmount = 3;

    private bool[] _inputs;
    private float yVelocity = 0;

    private void Start()
    {
        Gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        MoveSpeed *= Time.fixedDeltaTime;
        JumpSpeed *= Time.fixedDeltaTime;
    }

    public bool AttemptPickupItem()
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
        _inputs = new bool[6];
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

        Move(inputDirection);
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

        yVelocity += Gravity;

        moveDirection.y += yVelocity;

        Controller.Move(moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] inputs, Quaternion rotation)
    {
        _inputs = inputs;
        transform.rotation = rotation;
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

    protected override void TakeDamagePostprocess(int? attackerId)
    {
        if (HealthManager.IsDie)
        {
            Controller.enabled = false;
            transform.position = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];

            if (attackerId.HasValue == false)
            {
                //player suicide
            }
            else if (attackerId != Id)
            {
                RatingManager.KillAndDeath(attackerId.Value, Id);
                ServerSend.UpdateRatingTable(attackerId.Value, Id);
            }
            else
            {
                RatingManager.AddDeath(Id);
                ServerSend.UpdateRatingTableDeath(Id);
            }

            ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }
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
