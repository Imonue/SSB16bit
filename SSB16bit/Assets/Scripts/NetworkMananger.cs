using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Threading;


public class NetworkMananger : MonoBehaviour
{
    public static NetworkMananger instance;

    public static int BUFF_SIZE = 1024;

    public Socket sock;

    public Text textUI;
    public PostBox postBox;

    private Thread readThread;
    private Thread sendThread;

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

        sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var ep = new IPEndPoint(IPAddress.Parse("211.46.116.181"), 9190);
        //var ep = new IPEndPoint(IPAddress.Parse("192.168.171.130"), 9190);
        sock.Connect(ep);

        readThread = new Thread(ReciveToServer);
        readThread.Start();

        //sendThread = new Thread(SendToServer);
        //sendThread.Start();

        postBox = PostBox.GetInstance;
        StartCoroutine(CheckQueue());
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void SendMessage(string message)
    {
        byte[] buff = Encoding.UTF8.GetBytes(message);
        sock.Send(buff, SocketFlags.None);
    }

    public void SendLoginMessage(string id, string pw)
    {
        string message = "IDPW" + id + "/" + pw;
        byte[] buff = Encoding.UTF8.GetBytes(message);
        sock.Send(buff, SocketFlags.None);
        //StartCoroutine(ReciveToServer());
    }

    public void SendSelectCharacterMessage(string characterID)
    {
        string message = "SELECTCHARACTER:" + GameManager.instance.GetID() + '/' + characterID;
        SendMessage(message);
        //StartCoroutine(ReciveToServer());
    }

    public void SendMoveRightMessage()
    {
        string message = "CHARACTERMOVERIGHT" + GameManager.instance.GetID() + '&';
        SendMessage(message);
    }

    public void SendMoveLeftMessage()
    {
        string message = "CHARACTERMOVELEFT" + GameManager.instance.GetID() + '&';
        SendMessage(message);
    }

    public void SendMoveUpMessage()
    {
        string message = "CHARACTERMOVEUP" + GameManager.instance.GetID() + '&';
        SendMessage(message);
    }

    public void SendMoveDownMessage()
    {
        string message = "CHARACTERMOVEDOWN" + GameManager.instance.GetID() + '&';
        SendMessage(message);
    }

    public void SendMoveStopMessage()
    {
        string message = "CHARACTERMOVESTOP" + GameManager.instance.GetID() + '&';
        SendMessage(message);
    }

    public void SendJumpMessage()
    {
        string message = "CHARACTERJUMP" + GameManager.instance.GetID();
        SendMessage(message);
    }

    public void ResponseData(string data)
    {
        //textUI.text = data;
    }

    public void Response(string result)
    {
        PostBox.GetInstance.PushData(result);
    }

    public void ReciveToServer()
    {
        while (true)
        {
            string result;
            byte[] receiverBuff = new byte[8192];
            int n = NetworkMananger.instance.sock.Receive(receiverBuff);
            result = Encoding.UTF8.GetString(receiverBuff, 0, n);

            if (result.Contains("&"))
            {
                string[] results = result.Split(new string[] { "&" }, StringSplitOptions.None);

                for (int i = 0; i < results.Length; i++)
                    Response(results[i]);
            }
            else
            {
                Response(result);
            }
        }
    }

    public void SendToServer()
    {
        while (true)
        {
            string sendData = PostBox.GetInstance.GetSendData();
            if (sendData != string.Empty)
            {
                SendMessage(sendData);
            }
        }
    }

    //큐를 주기적으로 탐색
    private IEnumerator CheckQueue()
    {
        //1초 주기로 탐색
        WaitForSeconds waitSec = new WaitForSeconds(0);
        while (true)
        {
            //우편함에서 데이타 꺼내기
            string data = postBox.GetData();

            //우편함에 데이타가 있는 경우
            if (!data.Equals(string.Empty))
            {
                CheckMessage(data);
            }

            yield return waitSec;
        }
    }

    int count = 1;

    private void CheckMessage(string message)
    {
        if (message.Contains("LOGINSUCESS"))
        {
            message = message.Replace("LOGINSUCESS", ""); // LOGINSUCESS"ID" 부터 LOGINSUCESS를 지워서 ID만 남게 만듬
            Debug.Log("Login Sucess connect id is " + message); // 서버로부터 로그인 성공을 전달 받음
            GameManager.instance.SelectScene(message);
        }
        else if (message.Contains("CHARACTERMOVESTOP"))
        {
            message = message.Replace("CHARACTERMOVESTOP", "");
            GameManager.instance.ChracterMove(Vector3.zero, message);
        }
        else if (message.Contains("CHARACTERMOVERIGHT"))
        {
            message = message.Replace("CHARACTERMOVERIGHT", "");
            GameManager.instance.ChracterMove(Vector3.right, message);
        }
        else if (message.Contains("CHARACTERMOVELEFT"))
        {
            message = message.Replace("CHARACTERMOVELEFT", "");
            GameManager.instance.ChracterMove(Vector3.left, message);
        }
        else if (message.Contains("CHARACTERMOVEUP"))
        {
            message = message.Replace("CHARACTERMOVEUP", "");
            GameManager.instance.ChracterMove(Vector3.up, message);
        }
        else if (message.Contains("CHARACTERMOVEDOWN"))
        {
            message = message.Replace("CHARACTERMOVEDOWN", "");
            GameManager.instance.ChracterMove(Vector3.down, message);
        }
        else if (message.Contains("CHARACTERJUMP"))
        {
            string id = message.Replace("CHARACTERJUMP", "");
            GameManager.instance.CharacterJump(id);
        }
        else if (message.Contains("SELECTCHARACTER"))
        {
            Debug.Log(message);
            message = message.Replace("SELECTCHARACTER:", "");
            string[] result = message.Split(new char[] { '/' });
            string id = result[0];
            string characterID = result[1];
            Debug.Log("Select charter ID : " + characterID);
            GameManager.instance.LinkWorld(id, characterID);
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("프로그램 종료");
        readThread.Abort();
    }
}