using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
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
    private bool isJump
    {
        get
        {
            return 0 != rigidbody.velocity.y;

        }
    }
    [SerializeField]
    private bool isRun
    {
        get
        {
            return 0 != rigidbody.velocity.x;
        }
    }

    protected virtual void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rigidbody = GetComponent<Rigidbody2D>();
        Debug.Log("Character create active scene is " + SceneManager.GetActiveScene().name);
    }


    protected virtual void Update()
    {
        //AnimationMovement();
        if (userType)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                NetworkMananger.instance.SendMoveRightMessage();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                NetworkMananger.instance.SendMoveLeftMessage();
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                NetworkMananger.instance.SendMoveUpMessage();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                NetworkMananger.instance.SendMoveDownMessage();
            }
            else
            {
                if (direction != Vector3.zero)
                {
                    NetworkMananger.instance.SendMoveStopMessage();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        //rigidbody.AddForce(new Vector2((bLeft ? -speed : speed), 0));
        rigidbody.velocity = this.direction.normalized * speed * Time.fixedDeltaTime;
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
        if (isJump)
        {
            animator.SetBool("Jump", true);
        }
        else if (isRun)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
            animator.SetBool("Jump", false);
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
}
