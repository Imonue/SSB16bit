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

        sock.Connect(ep);

        readThread = new Thread(ReciveToServer);
        readThread.Start();

        sendThread = new Thread(SendToServer);
        sendThread.Start();

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
        string message = "IDPW" + id + "/" + pw + '\0';
        byte[] buff = Encoding.UTF8.GetBytes(message);
        sock.Send(buff, SocketFlags.None);
    }

    public void SendSelectCharacterMessage(string characterID)
    {
        string message = "SELECTCHARACTER:" + GameManager.instance.GetID() + '/' + characterID + '\0';
        SendMessage(message);
    }

    public void SendMoveRightMessage()
    {
        string message = "CHARACTERMOVERIGHT" + GameManager.instance.GetID() + '\0';
        //SendMessage(message);
        PostBox.GetInstance.PushSendData(message);
    }

    int ncount = 0;

    public void SendMoveLeftMessage()
    {
        ncount++;
        Debug.Log("Left count(Send) = " + ncount);
        string message = "CHARACTERMOVELEFT" + GameManager.instance.GetID() + '\0';
        //SendMessage(message);
        PostBox.GetInstance.PushSendData(message);
    }

    public void SendMoveUpMessage()
    {
        string message = "CHARACTERMOVEUP" + GameManager.instance.GetID() + '\0';
        //SendMessage(message);
        PostBox.GetInstance.PushSendData(message);
    }

    public void SendMoveDownMessage()
    {
        string message = "CHARACTERMOVEDOWN" + GameManager.instance.GetID() + '\0';
        //SendMessage(message);
        PostBox.GetInstance.PushSendData(message);
    }

    public void SendMoveStopMessage()
    {
        string message = "CHARACTERMOVESTOP" + GameManager.instance.GetID() + '\0';
        //SendMessage(message);
        PostBox.GetInstance.PushSendData(message);
    }

    public void SendJumpMessage()
    {
        string message = "CHARACTERJUMP" + GameManager.instance.GetID() + '\0';
        //SendMessage(message);
        PostBox.GetInstance.PushSendData(message);
    }

    public void SendAttackMessage()
    {
        string message = "CHARACTERATTACK" + GameManager.instance.GetID() + '\0';
        //SendMessage(message);
        PostBox.GetInstance.PushSendData(message);
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
            byte[] receiverBuff = new byte[1024];
            int n = NetworkMananger.instance.sock.Receive(receiverBuff);
            result = Encoding.UTF8.GetString(receiverBuff, 0, n);

            Debug.Log(result);

            Response(result);

        }
    }

    public void SendToServer()
    {
        while (true)
        {
            string sendData = PostBox.GetInstance.GetSendData();
            if (sendData != string.Empty)
            {
                Thread.Sleep(50);
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

    int count = 0;

    private void CheckMessage(string message)
    {
        if (message.Contains("LOGINSUCESS"))
        {
            message = message.Replace("LOGINSUCESS", ""); // LOGINSUCESS"ID" 부터 LOGINSUCESS를 지워서 ID만 남게 만듬
            Debug.Log("Login Sucess connect id is " + message); // 서버로부터 로그인 성공을 전달 받음
            GameManager.instance.SelectScene(message);
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
        else
        {
            if (message.Contains("MOVESTOP"))
            {
                GameManager.instance.ChracterMove(Vector3.zero);
            }
            else if (message.Contains("MOVERIGHT"))
            {
                GameManager.instance.ChracterMove(new Vector3(0.01f, 0, 0));
            }
            else if (message.Contains("MOVELEFT"))
            {
                count++;
                Debug.Log("Left count(Recevie) = " + count);
                GameManager.instance.ChracterMove(new Vector3(-0.01f, 0, 0));
            }
            else if (message.Contains("MOVEUP"))
            {
                GameManager.instance.ChracterMove(new Vector3(0, 0.01f, 0));
            }
            else if (message.Contains("MOVEDOWN"))
            {
                GameManager.instance.ChracterMove(new Vector3(0, -0.01f, 0));
            }
            else if (message.Contains("ATTACK"))
            {
                string id = message.Replace("CHARACTERATTACK", "");
                GameManager.instance.CharacterAttack(id);
            }
        }

    }

    void OnApplicationQuit()
    {
        Debug.Log("프로그램 종료");
        readThread.Abort();
        sendThread.Abort();
    }
}