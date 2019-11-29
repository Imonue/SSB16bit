using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum PlayerType
    {
        none = 0,
        player1 = 1,
        player2 = 2
    };

    public static GameManager instance;

    [Header("User infomation")]
    [SerializeField]
    private string userID;
    [SerializeField]
    private string auserID;
    public Character auc;
    public Character character;

    [Header("Game Manager Value")]
    [SerializeField]
    private bool gameStart = false;
    [SerializeField]
    private PlayerType playerType = PlayerType.none;
    private WaitForSeconds characterCreateTime = new WaitForSeconds(1.0f);


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SelectScene(string ID)
    {
        this.userID = ID;
        SceneManager.LoadScene("SelectScene");
    }

    public void LinkWorld(string ID, string characterID)
    {

        if (this.userID.Equals(ID))
        {
            SceneManager.LoadScene("WorldScene");
            StartCoroutine(CharacterCreate(characterID));
        }
        else
        {
            StartCoroutine(AnotherCharcterCreate(ID, characterID));
        }
    }

    public void ChracterMove(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            this.auc.transform.Translate(direction);
            this.auc.SetMoveDirection(direction);
            this.auc.GetAnimator().SetBool("Move", true);
        }
        else
        {
            this.auc.GetAnimator().SetBool("Move", false);
        }
    }

    public void CharacterAttack(string userID)
    {
        auc.CharacterAttack();
    }

    IEnumerator CharacterCreate(string characterID)
    {
        yield return characterCreateTime;
        Debug.Log("character : " + characterID);
        int characID = 0;
        if (characterID.Equals("mario"))
        {
            characID = 0;
        }
        else if (characterID.Equals("link"))
        {
            characID = 1;
        }
        else
        {
            Debug.Log("EEROR!! Check for character Id!!!");
        }
        if (this.playerType == PlayerType.player1)
        {
            this.character = Instantiate(CharacterManager.instance.characters[characID].gameObj, new Vector2(-1.0f, 0), Quaternion.identity).GetComponent<Character>();
        }
        else if(this.playerType == PlayerType.player2)
        {
            this.character = Instantiate(CharacterManager.instance.characters[characID].gameObj, new Vector2(1.0f, 0), Quaternion.identity).GetComponent<Character>();
        }
            this.character.SetUserID(this.userID);
        this.character.SetUserType(true);
        this.character.SetTag("Player");
        this.character.SetHS(UIManager.instance.GetUHS());
        UIManager.instance.SetUser(CharacterManager.instance.characters[characID].hp);
    }

    IEnumerator AnotherCharcterCreate(string ID, string characterID)
    {
        yield return characterCreateTime;
        this.gameStart = true;
        int characID = 0;
        if (characterID.Equals("mario"))
        {
            characID = 0;
        }
        else if (characterID.Equals("link"))
        {
            characID = 1;
        }
        else
        {
            Debug.Log("EEROR!! Check for character Id!!!");
        }
        this.auserID = ID;
        if (this.playerType == PlayerType.player1)
        {
            this.auc = Instantiate(CharacterManager.instance.characters[characID].gameObj, new Vector2(1.0f, 0), Quaternion.identity).GetComponent<Character>();
        }
        else if (this.playerType == PlayerType.player2)
        {
            this.auc = Instantiate(CharacterManager.instance.characters[characID].gameObj, new Vector2(-1.0f, 0), Quaternion.identity).GetComponent<Character>();
        }
        this.auc.SetUserID(ID);
        this.auc.SetUserType(false);
        this.auc.SetTag("AnotherPlayer");
        this.auc.SetHS(UIManager.instance.GetAUHS());
        UIManager.instance.SetAUser(CharacterManager.instance.characters[characID].hp);
    }

    public void SetPlayer1()
    {
        this.playerType = PlayerType.player1;
    }

    public void SetPlayer2()
    {
        this.playerType = PlayerType.player2;
    }

    ////////////////////////////// Getter/Setter //////////////////////////////

    public void SetID(string ID)
    {
        this.userID = ID;
    }

    public string GetID()
    {
        return this.userID;
    }
}
