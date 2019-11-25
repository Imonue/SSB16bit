using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("User HP Bar")]
    [SerializeField]
    private GameObject userHP;
    [SerializeField]
    private Image userChaImg;
    [SerializeField]
    private Slider uhs;

    [Header("Another User HP Bar")]
    [SerializeField]
    private GameObject auserHP;
    [SerializeField]
    private Image auserChaImg;
    [SerializeField]
    private Slider auhs;

    public Text aupx;
    public Text aupy;

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

        userHP.SetActive(false);
        userHP.SetActive(false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(GameManager.instance.character != null)
        {
            aupx.text = GameManager.instance.character.transform.position.x.ToString();
            aupy.text = GameManager.instance.character.transform.position.y.ToString();
        }
    }

    public void SetUser(Sprite characterImage)
    {
        this.userHP.SetActive(true);
        this.userChaImg.sprite = characterImage;
    }

    public void SetAUser(Sprite characterImage)
    {
        this.auserHP.SetActive(true);
        this.auserChaImg.sprite = characterImage;
    }

    public void SetUHS(Slider slider)
    {
        this.uhs = slider;
    }

    public Slider GetUHS()
    {
        return this.uhs;
    }

    public void SetAUHS(Slider slider)
    {
        this.auhs = slider;
    }

    public Slider GetAUHS()
    {
        return this.auhs;
    }
}
