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
    public SocketLib socket;
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
        var ep = new IPEndPoint(IPAddress.Parse("192.168.171.128"), 9190);
        sock.Connect(ep);

        socket = new SocketLib();
        postBox = PostBox.GetInstance;
        StartCoroutine(CheckQueue());
        RequestData();
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

    private void RequestData()
    {
        socket.Request();
    }

    public void ResponseData(string data)
    {
        textUI.text = data;
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