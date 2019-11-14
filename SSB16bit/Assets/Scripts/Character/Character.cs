using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public enum CharacterState
    {
        idle = 0,
        move = 1,
        dash = 2,
        attack = 3,
        interact = 4,
        stagger = 5
    }

    [Header("Component")]
    private Animator animator;
    private Rigidbody2D rigidbody;

    [Header("Infomation")]
    [SerializeField]
    private string userID;

    [Header("Stat")]
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private float jump = 100.0f;

    [Header("State")]
    [SerializeField]
    private bool userType; // true = i'm, false = another user
    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private Vector3 moveDirection;
    [SerializeField]
    private bool isJump
    {
        get
        {
            return 0 != rigidbody.velocity.y;

        }
    }
    [SerializeField]
    private bool isMove
    {
        get
        {
            return 0 != rigidbody.velocity.x && 0 != rigidbody.velocity.y;
        }
    }
    [SerializeField]
    private CharacterState currentState;

    public Coroutine attackRoutine;

    protected virtual void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.currentState = CharacterState.idle;
        Debug.Log("Character create active scene is " + SceneManager.GetActiveScene().name);
    }


    protected virtual void Update()
    {
        if (currentState == CharacterState.interact) return;
        AnimationMovement();
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        //rigidbody.AddForce(new Vector2((bLeft ? -speed : speed), 0));
        //rigidbody.velocity = this.direction.normalized * speed * Time.fixedDeltaTime;
    }

    public void Jump()
    {
        if (!isJump)
        {
            rigidbody.AddForce(new Vector2(0, jump));
        }
    }

    public void LookAt(bool bLeft)
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, bLeft ? 180 : 0, transform.rotation.z));
    }

    public void AnimationMovement()
    {
        GetMoveDirection().Normalize();
        SetMoveDirection(Mathf.Round(GetMoveDirection().x), Mathf.Round(GetMoveDirection().y), GetMoveDirection().z);
        GetAnimator().SetFloat("DirY", GetMoveDirection().y);
        GetAnimator().SetFloat("DirX", GetMoveDirection().x);
    }

    public void GetInput()
    {
        if (userType)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                NetworkMananger.instance.SendMoveRightMessage();
                this.transform.Translate(new Vector3(0.01f, 0, 0));
                this.SetMoveDirection(Vector3.right);
                this.GetAnimator().SetBool("Move", true);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                NetworkMananger.instance.SendMoveStopMessage();
                //this.SetDirection(Vector3.zero);
                this.GetAnimator().SetBool("Move", false);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                NetworkMananger.instance.SendMoveLeftMessage();
                this.transform.Translate(new Vector3(-0.01f, 0, 0));
                this.SetMoveDirection(Vector3.left);
                this.GetAnimator().SetBool("Move", true);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                NetworkMananger.instance.SendMoveStopMessage();
                this.SetDirection(Vector3.zero);
                this.GetAnimator().SetBool("Move", false);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                NetworkMananger.instance.SendMoveUpMessage();
                this.transform.Translate(new Vector3(0, 0.01f, 0));
                this.SetMoveDirection(Vector3.up);
                this.GetAnimator().SetBool("Move", true);
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                NetworkMananger.instance.SendMoveStopMessage();
                this.SetDirection(Vector3.zero);
                this.GetAnimator().SetBool("Move", false);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                NetworkMananger.instance.SendMoveDownMessage();
                this.transform.Translate(new Vector3(0, -0.01f, 0));
                this.SetMoveDirection(Vector3.down);
                this.GetAnimator().SetBool("Move", true);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                NetworkMananger.instance.SendMoveStopMessage();
                this.SetDirection(Vector3.zero);
                this.GetAnimator().SetBool("Move", false);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                CharacterAttack();
            }
        }
    }


    public void CharacterAttack()
    {
        attackRoutine = StartCoroutine(Attack());
    }


    private IEnumerator Attack()
    {
        currentState = CharacterState.attack;
        GetAnimator().SetBool("Attack", true);
        SetDirection(Vector3.zero);
        rigidbody.velocity = GetDirection().normalized * speed * Time.deltaTime;
        yield return new WaitForSeconds(0.05f);
        StopAttack();
    }

    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            currentState = CharacterState.idle;
            animator.SetBool("Attack", false);
        }
    }

    ////////////////////////////// Getter/Setter //////////////////////////////

    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public Animator GetAnimator()
    {
        return this.animator;
    }

    public void SetRigidbody(Rigidbody2D rigidbody)
    {
        this.rigidbody = rigidbody;
    }

    public Rigidbody2D GetRigidbody()
    {
        return this.rigidbody;
    }

    public void SetUserID(string userID)
    {
        this.userID = userID;
    }

    public string GetUserID()
    {
        return this.userID;
    }

    public void SetUserType(bool userType)
    {
        this.userType = userType;
    }

    public bool GetUserType()
    {
        return this.userType;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public Vector3 GetDirection()
    {
        return this.direction;
    }

    public void SetMoveDirection(float x, float y, float z)
    {
        this.moveDirection = new Vector3(x, y, z);
    }

    public void SetMoveDirection(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
    }

    public Vector3 GetMoveDirection()
    {
        return this.moveDirection;
    }

    public void SetCurrentState(CharacterState currentState)
    {
        this.currentState = currentState;
    }

    public CharacterState GetCurrentState()
    {
        return this.currentState;
    }
}
