using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

public class Client : MonoBehaviour
{
    IPAddress ipAddr;
    IPEndPoint ipEndPoint;
    Socket sender;

    public static bool connect = false;
    public static bool disconnect = false;

    public CharacterClient characterClient;
    public Character characterServer;
    // Start is called before the first frame update
    void Start()
    {
        characterServer = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        characterClient = DataScenes.characterClient.GetComponent<CharacterClient>();
        try
        {
            ipAddr = IPAddress.Parse(DataScenes.IPAddress);
            ipEndPoint = new IPEndPoint(ipAddr, 11000);        
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            Destroy(gameObject.GetComponent<Client>());
        }
        try
        {
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sender.ReceiveTimeout = 100;
            sender.Connect(ipEndPoint);
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
            sender.Close();
            Destroy(gameObject.GetComponent<Client>());
        }

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            int size;
            //подключение 
            if (!connect)
            {                
                byte[] msgConnect = new byte[1];
                msgConnect[0] = Convert.ToByte(true);
                sender.Send(msgConnect);

                sender.Receive(msgConnect);         
                if(Convert.ToBoolean(msgConnect[0]))
                {
                    connect = true;
                    characterClient.gameObject.SetActive(true);
                }
                else
                {
                    connect = false;
                }
                disconnect = false;
                return;
            }
            // отключение 
            if (disconnect)
            {
                byte[] msgConnect = new byte[1];
                msgConnect[0] = Convert.ToByte(false);
                sender.Send(msgConnect);
                sender.Close();
                disconnect = false;
                connect = false;
                return;
            }


            //отправление 
            int sizeInBytes = Marshal.SizeOf(characterClient.chrctrInfomation);
            IntPtr ptr = Marshal.AllocHGlobal(sizeInBytes);
            byte[] buffer = new byte[sizeInBytes];
            Marshal.StructureToPtr(characterServer.chrctrInfomation, ptr, true);
            Marshal.Copy(ptr, buffer, 0, sizeInBytes);
            Marshal.FreeHGlobal(ptr);
            sender.Send(buffer);

            //Получение
            sizeInBytes = Marshal.SizeOf(characterClient.chrctrInfomation);
            ptr = Marshal.AllocHGlobal(sizeInBytes);
            byte[] bytes = new byte[sizeInBytes];
            size = sender.Receive(bytes);
            Marshal.Copy(bytes, 0, ptr, sizeInBytes);
            characterClient.chrctrInfomation = (DataScenes.CharacterInfomation)Marshal.PtrToStructure(ptr, typeof(DataScenes.CharacterInfomation));
            Marshal.FreeHGlobal(ptr);
            

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            characterClient.gameObject.SetActive(false);
            sender.Close();
            disconnect = false;
            connect = false;
            Destroy(gameObject.GetComponent<Client>());
        }
    }
}
