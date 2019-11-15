using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("User infomation")]
    [SerializeField]
    private string ID;
    [SerializeField]
    private List<Character> characters = new List<Character>();

    [Header("Game Manager Value")]
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
        this.ID = ID;
        SceneManager.LoadScene("SelectScene");
    }

    public void LinkWorld(string ID, string characterID)
    {

        if (this.ID.Equals(ID))
        {
            SceneManager.LoadScene("WorldScene");
            StartCoroutine(CharacterCreate(characterID));
        }
        else
        {
            StartCoroutine(AnotherCharcterCreate(ID, characterID));
        }
    }

    public void ChracterMove(Vector3 direction, string userID)
    {
        if (!userID.Equals(this.ID))
        {
            if (direction != Vector3.zero)
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    if (characters[i].GetUserID().Equals(userID))
                    {
                        this.characters[i].transform.Translate(direction/100);
                        this.characters[i].SetMoveDirection(direction);
                        this.characters[i].GetAnimator().SetBool("Move", true);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    if (characters[i].GetUserID().Equals(userID))
                    {
                        Debug.Log(userID);
                        this.characters[i].GetAnimator().SetBool("Move", false);
                        break;
                    }
                }

            }
        }
    }

    public void CharacterJump(string userID)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].GetUserID().Equals(userID))
            {
                this.characters[i].Jump();
            }
        }
    }

    public void CharacterAttack(string userID)
    {
        if (!userID.Equals(this.ID))
        {
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].GetUserID().Equals(userID))
                {
                    this.characters[i].CharacterAttack();
                }
            }
        }
    }

    IEnumerator CharacterCreate(string characterID)
    {
        yield return characterCreateTime;
        Debug.Log("character : " + characterID);
        int characID = 0 ;
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
        Character character = Instantiate(CharacterManager.instance.characterObjs[characID], new Vector2(0, 0), Quaternion.identity).GetComponent<Character>();
        character.SetUserID(this.ID);
        character.SetUserType(true);
        character.SetTag("Player");
        this.characters.Add(character);
    }

    IEnumerator AnotherCharcterCreate(string ID, string characterID)
    {
        yield return characterCreateTime;
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
        Character character = Instantiate(CharacterManager.instance.characterObjs[characID], new Vector2(0, 0), Quaternion.identity).GetComponent<Character>();
        character.SetUserID(ID);
        character.SetUserType(false);
        character.SetTag("AnotherPlayer");
        this.characters.Add(character);
    }

    ////////////////////////////// Getter/Setter //////////////////////////////

    public void SetID(string ID)
    {
        this.ID = ID;
    }

    public string GetID()
    {
        return this.ID;
    }
}
