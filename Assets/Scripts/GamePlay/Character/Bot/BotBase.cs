using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBase : CharacterBase
{
    private float _chaseRange = 10f;
    private bool isPatrolRoutineRunning;
    private float yVelocity = 0;
    private BotState _state;
    private Player _target;

    public float patrolSpeed = 1f;
    public float chaseSpeed = 1f;
    public float detectionRange = 15f;
    public float patrolDuration = 3f;
    public float idleDuration = 3f;
    
    public override CharacterKind CharacterKind { get; } = CharacterKind.bot;

    public override int Team { get; protected set; } = -1;

    private void Start()
    {
        Gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        MoveSpeed *= Time.fixedDeltaTime;
        JumpSpeed *= Time.fixedDeltaTime;

        patrolSpeed *= Time.fixedDeltaTime;
        chaseSpeed *= Time.fixedDeltaTime;


        Id = BotManager.GetNextId();
        BotManager.AddBot(Id, this);
        HealthManager = new HealthManager(false);
        HealthManager.OwnerId = Id;

        WeaponController = new WeaponController(new List<WeaponBase> { new GunWeapon() });
        BoosterContainer = new BoosterContainer();

        RoomSendClient.SpawnBot(this);

        _state = BotState.patrol;
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case BotState.idle:
                LookForPlayer();
                break;
            case BotState.patrol:
                if (!LookForPlayer())
                {
                    Patrol();
                }
                break;
            case BotState.chase:
                Chase();
                break;
            case BotState.attack:
                Attack();
                break;
            default:
                break;
        }
    }

    private bool LookForPlayer()
    {
        foreach (Client client in Room.Clients.Values)
        {
            if (client.player != null && client.player.CharacterKind == CharacterKind.player)
            {
                Vector3 enemyToPlayer = client.player.transform.position - transform.position;
                if (enemyToPlayer.magnitude <= detectionRange)
                {
                    if (Physics.Raycast(ShootOrigin.position, enemyToPlayer, out RaycastHit hit, detectionRange))
                    {
                        if (hit.collider.TryGetComponent(out Player player))
                        {
                            _target = player;

                            if (isPatrolRoutineRunning)
                            {
                                isPatrolRoutineRunning = false;
                                StopCoroutine(StartPatrol());
                            }

                            _state = BotState.chase;
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private void Patrol()
    {
        if (!isPatrolRoutineRunning)
        {
            StartCoroutine(StartPatrol());
        }

        Move(transform.forward, patrolSpeed);
    }

    private IEnumerator StartPatrol()
    {
        isPatrolRoutineRunning = true;
        Vector2 _randomPatrolDirection = Random.insideUnitCircle.normalized;
        transform.forward = new Vector3(_randomPatrolDirection.x, 0f, _randomPatrolDirection.y);

        yield return new WaitForSeconds(patrolDuration);

        _state = BotState.idle;

        yield return new WaitForSeconds(idleDuration);

        _state = BotState.patrol;
        isPatrolRoutineRunning = false;
    }

    private void Chase()
    {
        if (CanSeeTarget())
        {
            Vector3 botToPlayer = _target.transform.position - transform.position;

            if (botToPlayer.magnitude <= _chaseRange)
            {
                _state = BotState.attack;
            }
            else
            {
                Move(botToPlayer, chaseSpeed);
            }
        }
        else
        {
            _target = null;
            _state = BotState.patrol;
        }
    }

    private void Attack()
    {
        if (CanSeeTarget())
        {
            Vector3 botToPlayer = _target.transform.position - transform.position;
            transform.forward = new Vector3(botToPlayer.x, 0f, botToPlayer.z);

            if (botToPlayer.magnitude <= _chaseRange)
            {
                Shoot(botToPlayer);
            }
            else
            {
                Move(botToPlayer, chaseSpeed);
            }
        }
        else
        {
            _target = null;
            _state = BotState.patrol;
        }
    }

    private void Move(Vector3 direction, float speed)
    {
        direction.y = 0f;
        transform.forward = direction;
        Vector3 movement = transform.forward * speed;

        if (Controller.isGrounded)
        {
            yVelocity = 0f;
        }
        yVelocity += Gravity;

        movement.y = yVelocity;
        Controller.Move(movement);

        RoomSendClient.BotPosition(this);
        RoomSendClient.BotRotation(this);
    }

    protected override void TakeDamagePostprocess(CharacterBase attacker)
    {
        RoomSendClient.BotHealth(HealthManager);

        if (HealthManager.IsDie)
        {
            if (attacker != null)
            {
                RatingManager.AddKillBot(attacker.Id);
                RoomSendClient.UpdateRatingTableBotsKills(RatingManager.GetPlayerEntity(attacker.Id).KilledBots, attacker.Id);
            }

            BotManager.RemoveBot(Id);
            Destroy(gameObject);
        }
    }

    private bool CanSeeTarget()
    {
        if (_target == null)
        {
            return false;
        }

        if (Physics.Raycast(ShootOrigin.position, _target.transform.position - transform.position, out RaycastHit hit, detectionRange))
        {
            if (hit.collider.TryGetComponent<Player>(out var _))
            {
                return true;
            }
        }

        return false;
    }
}
