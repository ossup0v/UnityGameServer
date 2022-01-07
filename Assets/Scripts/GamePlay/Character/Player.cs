using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : CharacterBase
{
    private float controllHeight;
    private bool[] _inputs;
    private float yVelocity = 0;

    public MeshRenderer Model;
    public string Username;
    public float ThrowForce = 600f * 2 * 4 * 10;
    public float ShiftMultiplayer = 2f;
    public Vector3[] SpawnPoints;
    public float RespawnTime = 2f;
    public int grenadeCount = 0;
    public int maxItemAmount = 3;
    public override CharacterKind CharacterKind { get; } = CharacterKind.player;

    public override int Team { get; protected set; } = 0;

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
        RoomSendClient.PlayerGrenadeCount(Id, grenadeCount);

        return true;
    }

    public void Initialize(Guid id, string username, int team)
    {
        Id = id;
        Username = username;
        Team = team;
        HealthManager = new HealthManager(true);
        HealthManager.OwnerId = Id;
        WeaponController = new WeaponController(new List<WeaponBase> { new RocketLaucnherWeapon(), new TeleportWeapon() });
        BoosterContainer = new BoosterContainer();
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

        RoomSendClient.PlayerPosition(this);
        RoomSendClient.PlayerRotation(this);
        RoomSendClient.PlayerScale(this);
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
            RoomSendClient.PlayerGrenadeCount(Id, grenadeCount);
            NetworkManager.Instance.InstantiatePrjectile(ShootOrigin)
                .Initialize(viewDuraction, ThrowForce, this);
        }
    }

    protected override void TakeDamagePostprocess(CharacterBase attacker)
    {
        RoomSendClient.PlayerHealth(HealthManager);

        if (HealthManager.IsDie)
        {
            Controller.enabled = false;
            //transform.position = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];

            if (attacker == null)
            {
                //player suicide
            }
            else if (attacker.Id != Id)
            {
                if (attacker.CharacterKind == CharacterKind.player)
                {
                    RatingManager.KillAndDeath(attacker.Id, Id);
                    RoomSendClient.UpdateRatingTable(attacker.Id, Id);
                }
                else if (attacker.CharacterKind == CharacterKind.bot)
                {
                    RatingManager.AddDeath(Id);
                    RoomSendClient.UpdateRatingTableDeath(Id);
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
                RoomSendClient.UpdateRatingTableDeath(Id);
            }

            RoomSendClient.PlayerPosition(this);

            var firstLivePlayer = Room.Clients.Values.FirstOrDefault(x => x.player.HealthManager.IsALive);

            if (firstLivePlayer == null) //game over here
            {
                OnGameEnd();
            }

            var firstLivePlayerInOtherTeam = Room.Clients.Values.FirstOrDefault(x => x.player.HealthManager.IsALive && x.player.Team != firstLivePlayer.player.Team);

            if (firstLivePlayerInOtherTeam == null) //game over here. Team firstLivePlayer.player.Team WIN!
            {
                OnGameEnd();
            }
#warning you can on respawn here!
            //StartCoroutine(Respawn());
        }
    }

    private void OnGameEnd()
    {
        RoomSendServer.GameRoomEnd();

        StartCoroutine(Exit());
    }

    private IEnumerator Exit()
    {
        yield return new WaitForSeconds(RespawnTime);

#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(RespawnTime);

        HealthManager.SetPureHealth(HealthManager.MaxHealth);
        Controller.enabled = true;
        RoomSendClient.PlayerRespawn(this);
    }

    public void Suicide()
    {
        TakeDamage(HealthManager.CurrentHealth, null);
    }

    private bool GetShitftDown() => _inputs[5];
}
