using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public static LoginManager instance;

    [Header("ID")]
    [SerializeField]
    private InputField idInputField;
    [SerializeField]
    private string id;

    [Header("Password")]
    [SerializeField]
    private InputField pwInputField;
    [SerializeField]
    private string pw;

    [Header("MessageBox")]
    [SerializeField]
    private GameObject messageBoxObj;
    [SerializeField]
    private Text messageText;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickLogin()
    {
        if (idInputField.text == "" || idInputField.text == "")
        {
            messageBoxObj.SetActive(true);
            messageText.text = "아이디와 비밀번호를 다시한번 확인해주세요";

        }
        else
        {
            this.id = idInputField.text;
            this.pw = pwInputField.text;

            NetworkMananger.instance.SendLoginMessage(id, pw);

        }
    }
}
