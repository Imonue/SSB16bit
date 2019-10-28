using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

public class NetworkMananger : MonoBehaviour
{
    public static NetworkMananger instance;

    public Socket sock;

    public Text textUI;
    public PostBox postBox;

    private int count = 0;

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

    public void ResponseData(string data)
    {
        textUI.text = data;
    }

    public void Response(string result)
    {
        PostBox.GetInstance.PushData(result);
    }

    public IEnumerator ReciveToServer()
    {
        string result;
        byte[] receiverBuff = new byte[8192];
        int n = NetworkMananger.instance.sock.Receive(receiverBuff);
        result = Encoding.UTF8.GetString(receiverBuff, 0, n);
        Debug.Log("Server message : " + result);
        Response(result);
        yield return new WaitForSeconds(0.0f);
    }

    //큐를 주기적으로 탐색
    private IEnumerator CheckQueue()
    {
        //1초 주기로 탐색
        WaitForSeconds waitSec = new WaitForSeconds(0);
        count++;
        Debug.Log("Count : " + count);
        while (true)
        {
            //우편함에서 데이타 꺼내기
            string data = postBox.GetData();
            Debug.Log("Post Message : " + data);
            //우편함에 데이타가 있는 경우
            if (!data.Equals(string.Empty))
            {
                //데이타로 UI 갱신
                ResponseData(data);
                //yield break;
            }

            yield return waitSec;
        }
    }
}