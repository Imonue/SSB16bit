using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Text;


public class SocketLib : MonoBehaviour
{
    private Thread thread;

    public SocketLib()
    {
        thread = new Thread(new ThreadStart(Run));
    }

    public void Request()
    {
        //thread.Start();
    }

    void Run()
    {
        string result;
        byte[] receiverBuff = new byte[8192];
        int n = NetworkMananger.instance.sock.Receive(receiverBuff);
        result = Encoding.UTF8.GetString(receiverBuff, 0, n);

        Thread.Sleep(1000);

        Debug.Log("Server message : " + result);
        Response(result);

    }

    

    public IEnumerator ReciveToServer()
    {
        string result;
        byte[] receiverBuff = new byte[8192];
        int n = NetworkMananger.instance.sock.Receive(receiverBuff);
        result = Encoding.UTF8.GetString(receiverBuff, 0, n);
        //Debug.Log("Server message : " + result);
        Response(result);
        yield return new WaitForSeconds(0.0f);
    }

    public void Response(string result)
    {
        PostBox.GetInstance.PushData(result);
    }
}