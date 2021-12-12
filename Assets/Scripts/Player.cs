using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public WeaponController WeaponController;
    public BoosterContainer BoosterContainer;

    public int Id;
    public string Username;
    public CharacterController Controller;
    public Transform ShootOrigin;
    public float Gravity = -9.81f * 2;
    public float MoveSpeed = 5f;
    public float JumpSpeed = 5f;
    public float CurrentHealth;
    public float MaxHealth = 100;
    public float ThrowForce = 600f * 2 * 4 * 10;

    public bool IsALife => CurrentHealth > 0;
    public bool IsDie => CurrentHealth <= 0;
    public int itemAmount = 0;
    public int maxItemAmount = 3;

    private bool[] _inputs;
    private float yVelocity = 0;

    private void Start()
    {
        Gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        MoveSpeed *= Time.fixedDeltaTime;
        JumpSpeed *= Time.fixedDeltaTime;
    }

    internal bool AttemptPickupItem()
    {
        if (itemAmount >= maxItemAmount)
            return false;

        itemAmount++;
        return true;
    }

    public void Initialize(int id, string username)
    {
        Id = id;
        Username = username;
        CurrentHealth = MaxHealth;
        WeaponController = new WeaponController(new List<WeaponBase> { new GunWeapon(), new RocketLaucnherWeapon() });
        BoosterContainer = new BoosterContainer();

        _inputs = new bool[5];
    }

    public void FixedUpdate()
    {
        if (IsDie)
            return;

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
            //inputDirection.x += 1;
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

    public void Shoot(Vector3 viewDuraction)
    {
        if (IsDie)
            return;

        WeaponController.GetCurrentWeapon().Shoot(this, viewDuraction, ShootOrigin.position);
    }

    public void ThrowItem(Vector3 viewDuraction)
    {
        if (IsDie)
            return;

        if (itemAmount > 0)
        {
            itemAmount--;
            NetworkManager.Instance.InstantiatePrjectile(ShootOrigin)
                .Initialize(viewDuraction, ThrowForce, Id);
        }
    }

    public void TakeDamage(float damage, int attackerPlayerId)
    {
        Debug.Log("Player taking damage!!");

        if (IsDie )
            return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Controller.enabled = false;
            transform.position = new Vector3(0f, 25f, 0);
            RatingManager.KillAndDeath(attackerPlayerId, Id);
            ServerSend.UpdateRatingTable(attackerPlayerId, Id);
            ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }

        ServerSend.PlayerHealth(this);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        CurrentHealth = MaxHealth;
        Controller.enabled = true;
        ServerSend.PlayerRespawn(this);
    }
}
