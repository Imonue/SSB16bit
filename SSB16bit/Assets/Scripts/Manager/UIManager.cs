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

    [Header("Another User HP Bar")]
    [SerializeField]
    private GameObject auserHP;
    [SerializeField]
    private Image auserChaImg;

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
}
