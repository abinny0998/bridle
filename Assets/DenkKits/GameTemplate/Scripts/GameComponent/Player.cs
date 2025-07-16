using DenkKits.GameServices.Audio.Scripts;
using DenkKits.UIManager.Scripts.UIAnimation;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum NameAnimationState
    {
        Root_Idle,
        Root_Run,
        Root_Walk,
        Root_Death,
        Root_Attack
    }

    [SerializeField] private Animator _animatorCharracter;
    [SerializeField] private Transform _tfCharacter;
    [SerializeField] private Stack<GameObject> stackBricks = new();
    private StatePlayer statePlayer;
    private NameAnimationState _currentNameAnimationState;
    private float speed = 2.0f;
    private float posYSpawn = 0.0f;
    private float heightBrick = 0.0f;
    private Vector3 current = Vector3.zero;
    private Vector3 target = Vector3.zero;
    [SerializeField] private Vector3 checkBrick = Vector3.zero;

    private bool _isEndGame = false;

    private Action onGameover;
    private void Start()
    {
        posYSpawn = this.transform.position.y;
        //heightBrick = prefBrick.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.z;
    }

    public void Reset()
    {
        if (stackBricks.Count > 0)
        {
            for (int i = 0; i < stackBricks.Count - 1; i++)
            {
                //RemoveBrick();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isEndGame) return;
        CheckPrivote(checkBrick);
        Move(statePlayer);
        //CheckBrick();
    }

    public void Init(float speed, Action onGameover)
    {
        this.speed = speed;
        this.onGameover = onGameover;
    }

    public void GameEnd(bool isWin)
    {
        _isEndGame = true;
        if (isWin)
            RunAnimation(NameAnimationState.Root_Attack);
        else
            RunAnimation(NameAnimationState.Root_Death);

    }

    private void CheckDirection()
    {
        int countDirection = 4;
        if (CheckPrivote(GetDirection(StatePlayer.MoveForward)))
            countDirection--;
        if (CheckPrivote(GetDirection(StatePlayer.MoveBackward)))
            countDirection--;
        if (CheckPrivote(GetDirection(StatePlayer.MoveLeft)))
            countDirection--;
        if (CheckPrivote(GetDirection(StatePlayer.MoveRight)))
            countDirection--;

        if (countDirection <= 0)
        {
            if (CheckBridge(GetDirection(StatePlayer.MoveForward)) ||
                CheckBridge(GetDirection(StatePlayer.MoveBackward)) ||
                CheckBridge(GetDirection(StatePlayer.MoveLeft)) ||
                    CheckBridge(GetDirection(StatePlayer.MoveRight)))
                return;

            GameController.Instance.GameEnd(false);
        }
    }

    private void RunAnimation(NameAnimationState nameAnimationState)
    {
        if (_isEndGame && nameAnimationState != NameAnimationState.Root_Death) return;
        if (_currentNameAnimationState == nameAnimationState) { return; }

        _currentNameAnimationState = nameAnimationState;
        _animatorCharracter.Play(nameAnimationState.ToString());

        if (nameAnimationState == NameAnimationState.Root_Idle)
        {
            AudioManager.Instance.PlaySfx(AudioName.Gameplay_Dash);
            if (!_isEndGame)
                CheckDirection();
        }
    }

    public void ChangeStatePlayer(StatePlayer statePlayer)
    {
        this.statePlayer = statePlayer;
        checkBrick = GetDirection(statePlayer);
    }

    private Vector3 GetDirection(StatePlayer statePlayer)
    {
        switch (statePlayer)
        {
            case StatePlayer.MoveForward:
                return new Vector3(0, 5, 2);
            case StatePlayer.MoveBackward:
                return new Vector3(0, 5, -2);
            case StatePlayer.MoveLeft:
                return new Vector3(-2, 5, 0);
            case StatePlayer.MoveRight:
                return new Vector3(2, 5, 0);
        }
        return Vector3.zero;
    }

    private void Move(StatePlayer statePlayer)
    {
        Vector3 posMoving = Vector3.zero;
        switch (statePlayer)
        {
            case StatePlayer.Idle:
                RunAnimation(NameAnimationState.Root_Idle);
                posMoving = Vector3.zero;
                break;
            case StatePlayer.MoveForward:
                RunAnimation(NameAnimationState.Root_Run);
                posMoving = Vector3.forward;
                break;
            case StatePlayer.MoveBackward:
                RunAnimation(NameAnimationState.Root_Run);
                posMoving = Vector3.back;
                break;
            case StatePlayer.MoveLeft:
                RunAnimation(NameAnimationState.Root_Run);
                posMoving = Vector3.left;
                break;
            case StatePlayer.MoveRight:
                RunAnimation(NameAnimationState.Root_Run);
                posMoving = Vector3.right;
                break;
        }
        RotateCharacter(statePlayer);
        Moving(posMoving);
    }

    public void Moving(Vector3 posMoving)
    {
        if (_isEndGame) return;
        current = this.transform.position;
        target = this.transform.position + posMoving;
        this.transform.position = Vector3.MoveTowards(current, target, speed);
    }

    public bool CheckPrivote(Vector3 directtion)
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position + directtion, transform.TransformDirection(Vector3.down * 4), out hit, Mathf.Infinity);
        Debug.DrawRay(this.transform.position + directtion, transform.TransformDirection(Vector3.down * 4), Color.green, Mathf.Infinity);

        if (hit.collider == null)
        {
            statePlayer = StatePlayer.Idle;
            RunAnimation(NameAnimationState.Root_Idle);
            return true;
        }
        else
        {
            //if(!hit.collider.tag.Contains("Brick") && !hit.collider.tag.Contains("Bridge"))
            if (hit.collider.CompareTag("Water"))
            {
                statePlayer = StatePlayer.Idle;
                RunAnimation(NameAnimationState.Root_Idle);
                return true;
            }
            else if (hit.collider.CompareTag("Brigde") && statePlayer != StatePlayer.MoveForward)
            {
                statePlayer = StatePlayer.Idle;
                RunAnimation(NameAnimationState.Root_Idle);
                return true;
            }
        }
        return false;
    }

    public bool CheckBridge(Vector3 directtion)
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position + directtion, transform.TransformDirection(Vector3.down * 4), out hit, Mathf.Infinity);
        Debug.DrawRay(this.transform.position + directtion, transform.TransformDirection(Vector3.down * 4), Color.green, Mathf.Infinity);

        if (hit.collider != null && hit.collider.CompareTag("Brigde"))
        {
            Debug.Log("AAAAAAAAAAAA 1");
            return true;
        }
        Debug.Log("AAAAAAAAAAAA 2");
        return false;
    }

    private void RotateCharacter(StatePlayer statePlayer)
    {
        switch (statePlayer)
        {
            case StatePlayer.Idle: break;
            case StatePlayer.MoveForward:
                _tfCharacter.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case StatePlayer.MoveBackward:
                _tfCharacter.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case StatePlayer.MoveLeft:
                _tfCharacter.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case StatePlayer.MoveRight:
                _tfCharacter.rotation = Quaternion.Euler(0, 90, 0);
                break;
        }
    }
}
