using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public MeshRenderer Model;

    public string Username;
    public float ThrowForce = 600f * 2 * 4 * 10;
    public float ShiftMultiplayer = 2f;
    public Vector3[] SpawnPoints;
    public float RespawnTime = 2f;
    public int grenadeCount = 0;
    public int maxItemAmount = 3;
    float controllHeight;
    private bool[] _inputs;
    private float yVelocity = 0;

    public override CharacterKind CharacterKind { get; } = CharacterKind.player;

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

    public void Initialize(Guid id, string username)
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
                .Initialize(viewDuraction, ThrowForce, this);
        }
    }

    protected override void TakeDamagePostprocess(CharacterBase attacker)
    {
        ServerSend.PlayerHealth(HealthManager);

        if (HealthManager.IsDie)
        {
            Controller.enabled = false;
            transform.position = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];

            if (attacker == null)
            {
                //player suicide
            }
            else if (attacker.Id != Id)
            {
                if (attacker.CharacterKind == CharacterKind.player)
                {
                    RatingManager.KillAndDeath(attacker.Id, Id);
                    ServerSend.UpdateRatingTable(attacker.Id, Id);
                }
                else if (attacker.CharacterKind == CharacterKind.bot)
                {
                    RatingManager.AddDeath(Id);
                    ServerSend.UpdateRatingTableDeath(Id);
                }
                else
                {
                    Debug.LogError($"Unknow character kind {attacker.CharacterKind}");
                }
            }
            else
            {
                //player kill self
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
