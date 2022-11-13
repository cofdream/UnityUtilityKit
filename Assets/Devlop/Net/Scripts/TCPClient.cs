using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Cofdream.Net
{
    [System.Serializable]
    public class TCPClient
    {
        public Socket socket;
        public void Init(string address = "127.0.0.1",int port = 9999)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(new IPEndPoint(IPAddress.Parse(address), port), (asyncResult) =>
            {
                socket.EndConnect(asyncResult);

                AsyncSend(socket, "服务器你好，我是客户端 Cofdream.");

                AsyncReveive(socket);


            }, null);
        }

        public void Quit()
        {
            socket.Close();
        }

        public void AsyncReveive()
        {
            AsyncReveive(socket);
        }


        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="client"></param>  
        private void AsyncReveive(Socket socket)
        {
            byte[] data = new byte[2048];
            try
            {
                //开始接收消息  
                socket.BeginReceive(data, 0, data.Length, SocketFlags.None,
                asyncResult =>
                {
                    int length = socket.EndReceive(asyncResult);
                    Debug.Log(string.Format("服务器发送消息:{0}", Encoding.UTF8.GetString(data)));
                }, null);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        /// <summary>  
        /// 发送消息  
        /// </summary>  
        /// <param name="client"></param>  
        /// <param name="p"></param>  
        private void AsyncSend(Socket client, string p)
        {
            if (client == null || p == string.Empty) return;
            //数据转码  
            byte[] data = new byte[2048];
            data = Encoding.UTF8.GetBytes(p);
            try
            {
                //开始发送消息  
                client.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
                {
                    //完成消息发送  
                    int length = client.EndSend(asyncResult);
                    //输出消息  
                    Debug.Log(string.Format("客户端发出消息:{0}", p));
                }, null);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}