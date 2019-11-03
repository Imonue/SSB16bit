using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofle;

public class Mario : MonoBehaviour
{
    public enum MarioState
    {
        Idle,
        Run,
        Jump
    }

    [Header("Mario Sprite")]
    [SerializeField]
    private SpriteRenderer spriteIdle = null;
    [SerializeField]
    private SpriteRenderer spriteRun = null;
    [SerializeField]
    private SpriteRenderer spriteJump = null;

    private StateMachine<Mario> stateMachine = null;

    [Header("Component")]
    [SerializeField]
    private Rigidbody2D rigidBody2D = null;

    [Header("Stat")]
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float jump = 200.0f;
    [SerializeField]
    private bool isJump
    {
        get
        {
            return 0 != rigidBody2D.velocity.y;

        }
    }

    private class IdleState : State<Mario>
    {
        protected override void Begin()
        {
            base.Begin();
            Owner.ShowSprite(MarioState.Idle);
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) // 방향키를 눌렀을 때 Idle에서 Run으로 변경
            {
                // Run로 변경 처리 코드
                Invoke<RunState>();
            }
            if (Input.GetKey(KeyCode.Space)) // 스페이스바를 눌렀을 때 Idle에서 Jump로 변경
            {
                // Jump로 변경 처리 코드
                Invoke<JumpState>();
            }
        }

        protected override void End()
        {
            base.End();
        }
    }

    private class RunState : State<Mario>
    {
        protected override void Begin()
        {
            base.Begin();
            Owner.ShowSprite(MarioState.Run);
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Owner.Move(true);
                Owner.LookAt(true);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Owner.Move(false);
                Owner.LookAt(false);
            }
            else
            {
                // Idle로 변경 처리 코드
                Invoke<IdleState>();
            }

            if (Input.GetKey(KeyCode.Space)) // 스페이스바를 눌렀을 때 Run에서 Jump로 변경
            {
                // Jump로 변경 처리 코드
                Invoke<JumpState>();
            }
        }

        protected override void End()
        {
            base.End();
        }
    }

    private class JumpState : State<Mario>
    {
        protected override void Begin()
        {
            base.Begin();
            Owner.Jump();
            Owner.ShowSprite(MarioState.Jump);
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Owner.Move(true);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Owner.Move(false);
            }

            if (!Owner.isJump)
            {
                Invoke<IdleState>();
            }
        }

        protected override void End()
        {
            base.End();
        }
    }

    void Start()
    {
        //Component Initialization
        rigidBody2D = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine<Mario>(this);
        StartCoroutine(stateMachine.Coroutine<IdleState>());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Move(bool bLeft)
    {
        rigidBody2D.AddForce(new Vector2((bLeft ? -speed : speed), 0));
        if (bLeft)
        {
            NetworkMananger.instance.SendMessage("Mario Left Move");
            //StartCoroutine(NetworkMananger.instance.ReciveToServer());
        }
        else
        {
            NetworkMananger.instance.SendMessage("Mario Right Move");
           // StartCoroutine(NetworkMananger.instance.ReciveToServer());
        }
    }

    private void LookAt(bool bLeft)
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, bLeft ? 180 : 0, transform.rotation.z));
    }

    private void Jump()
    {
        if (!isJump)
        {
            rigidBody2D.AddForce(new Vector2(0, jump));
            NetworkMananger.instance.SendMessage("Mario Left Jump");
            //StartCoroutine(NetworkMananger.instance.ReciveToServer());
        }
    }

    private void ShowSprite(MarioState state)
    {
        HideAllSprite();
        switch (state)
        {
            case MarioState.Idle:
                spriteIdle.enabled = true;
                break;
            case MarioState.Run:
                spriteRun.enabled = true;
                break;
            case MarioState.Jump:
                spriteJump.enabled = true;
                break;
        }
    }

    private void HideAllSprite()
    {
        spriteIdle.enabled = false;
        spriteRun.enabled = false;
        spriteJump.enabled = false;
    }

}
