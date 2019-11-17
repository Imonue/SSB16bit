using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public struct CharacterInfo
    {
        public GameObject gameObj;
        public Sprite hp;
    }

    enum CharacterID
    {
        mario = 1
    }

    public static CharacterManager instance;

    [SerializeField]
    private List<GameObject> objs = new List<GameObject>();
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();

    public List<CharacterInfo> characters = new List<CharacterInfo>();

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

        CharacterInfo characterInfo1;
        characterInfo1.gameObj = objs[0];
        characterInfo1.hp = sprites[0];
        characters.Add(characterInfo1);

        CharacterInfo characterInfo2;
        characterInfo2.gameObj = objs[1];
        characterInfo2.hp = sprites[1];
        characters.Add(characterInfo2);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
