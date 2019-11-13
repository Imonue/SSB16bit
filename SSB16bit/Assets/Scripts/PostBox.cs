using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostBox
{
    //싱글턴 인스턴스
    private static PostBox instance;
    //싱글턴 인스턴스 반환
    public static PostBox GetInstance
    {
        get
        {
            if (instance == null)
                instance = new PostBox();

            return instance;
        }
    }

    // 데이타를 담을 큐
    public Queue<string> readQueue;

    // 서버에게 보낼 메세지를 갖고 있는 큐
    public Queue<string> sendQueue;

    private PostBox()
    {   //큐 초기화
        readQueue = new Queue<string>();
        sendQueue = new Queue<string>();
    }

    //큐에 데이타 삽입
    public void PushData(string data)
    {
        readQueue.Enqueue(data);
    }

    public void PushSendData(string data)
    {
        sendQueue.Enqueue(data);
    }

   
    //큐에있는 데이타 꺼내서 반환
    public string GetData()
    {
        //데이타가 1개라도 있을 경우 꺼내서 반환
        if (readQueue.Count > 0)
            return readQueue.Dequeue();
        else
            return string.Empty;    //없으면 빈값을 반환
    }

    //큐에있는 데이타 꺼내서 반환
    public string GetSendData()
    {
        //데이타가 1개라도 있을 경우 꺼내서 반환
        if (sendQueue.Count > 0)
            return sendQueue.Dequeue();
        else
            return string.Empty;    //없으면 빈값을 반환
    }
}
