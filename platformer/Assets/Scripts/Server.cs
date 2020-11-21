﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

public class Server : MonoBehaviour
{
    IPHostEntry ipHost;
    IPAddress ipAddr;
    IPEndPoint ipEndPoint;
    private Socket sListener;
    public static bool connectClient = false;
    public static bool disconnectServer=false;

    public  Character characterServer;
    public CharacterClient characterClient;
    
    // Start is called before the first frame update
    void Start()
    {
        characterServer = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        characterClient = characterClient = DataScenes.characterClient.GetComponent<CharacterClient>();
        try
        {
            ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipHost.AddressList[1];
            Debug.Log(ipAddr);
            ipEndPoint = new IPEndPoint(ipAddr, 11000);

            sListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            sListener.Bind(ipEndPoint);
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
            sListener.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            //Отключение 
            if(disconnectServer)
            {
                sListener.Close();
                disconnectServer = false;
                connectClient = false;
                return;
            }
            if (sListener.Available > 0)
            {
                int size;
                IPEndPoint IPEndPointClient = new IPEndPoint(IPAddress.Any, 11000);
                EndPoint EndPointClient = (EndPoint)IPEndPointClient;

                //Получение
                int sizeInBytes = Marshal.SizeOf(characterClient.chrctrInfomation);
                IntPtr ptr = Marshal.AllocHGlobal(sizeInBytes);
                byte[] buffer = new byte[sizeInBytes];

                size = sListener.ReceiveFrom(buffer, ref EndPointClient);
                if(size==1)
                {
                    bool tempBool = Convert.ToBoolean(buffer[0]);
                    if(tempBool)
                    {
                        connectClient = true;
                        characterClient.gameObject.SetActive(true);
                        Debug.Log("CONNECT");

                        byte[] msgConnect = new byte[1];
                        msgConnect[0] = Convert.ToByte(true);
                        sListener.SendTo(msgConnect,EndPointClient);

                    }
                    else
                    {
                        connectClient = false;                           
                        characterClient.gameObject.SetActive(false);
                        Debug.Log("DISCONNECT");
                    }                        
                    return;
                }

                Marshal.Copy(buffer, 0, ptr, sizeInBytes);
                characterClient.chrctrInfomation = (DataScenes.CharacterInfomation)Marshal.PtrToStructure(ptr, typeof(DataScenes.CharacterInfomation));
                Marshal.FreeHGlobal(ptr);

                //Отправление
                sizeInBytes = Marshal.SizeOf(characterServer.chrctrInfomation);
                byte[] temp = new byte[sizeInBytes];
                ptr = Marshal.AllocHGlobal(sizeInBytes);
                Marshal.StructureToPtr(characterServer.chrctrInfomation, ptr, true);
                Marshal.Copy(ptr, temp, 0, sizeInBytes);
                Marshal.FreeHGlobal(ptr);
                sListener.SendTo(temp, EndPointClient);
            }           

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            characterClient.gameObject.SetActive(false);
            connectClient = false;
            disconnectServer = false;
            sListener.Close();
        }
    }
}