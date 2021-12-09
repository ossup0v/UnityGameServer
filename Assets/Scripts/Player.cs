using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Id;
    public string Username;
    public CharacterController Controller;
    public Transform ShootOrigin;
    public float Gravity = -9.81f;
    public float MoveSpeed = 5f;
    public float JumpSpeed = 5f;
    public float CurrentHealth;
    public float MaxHealth = 100;
    public float GunDistance = 25f;
    public float GunDamage = 50f;
    public float ThrowForce = 600f;

    public bool IsALife => CurrentHealth > 0;
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

        _inputs = new bool[5];
    }

    public void FixedUpdate()
    {
        if (CurrentHealth <= 0)
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
            inputDirection.x += 1;
        }

        Move(inputDirection);
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
        if (!IsALife)
            return;

        if (Physics.Raycast(ShootOrigin.position, viewDuraction, out var hit, GunDistance))
        {
            if (hit.collider.TryGetComponent<HitRegistration>(out var hitRegistration))
            {
                hitRegistration.RegisterHit(GunDamage);
            }
        }
    }

    public void ThrowItem(Vector3 viewDuraction)
    {
        if (!IsALife)
            return;

        if (itemAmount > 0)
        {
            itemAmount--;
            NetworkManager.Instance.InstantiatePrjectile(ShootOrigin)
                .Initialize(viewDuraction, ThrowForce, Id);
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Player taking damage!!");

        if (!IsALife)
            return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Controller.enabled = false;
            transform.position = new Vector3(0f, 25f, 0);

            ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }

        ServerSend.PlayeHealth(this);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        CurrentHealth = MaxHealth;
        Controller.enabled = true;
        ServerSend.PlayeRespawn(this);
    }
}
