using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.IO;

public class TwitchBotInput : MonoBehaviour
{
    private Queue<string> m_messageQueue;

    private TcpClient m_tcpClient;
    private StreamReader m_streamReader;
    private StreamWriter m_streamWriter;

    public string m_username;
    public string m_password;
    public string m_channelName;
    private string m_chatPrefix;
    private string m_chatCommandId;

    private DateTime m_lastMessage;

    private PacManMovement m_movementScript;

    private void Start()
    {
        if (GameManager.m_gameManager.GetGameType() == GameManager.GameType.TWITCH)
        {
            Destroy(gameObject.GetComponent<InputHandler>());
            m_movementScript = GetComponent<PacManMovement>();
            m_messageQueue = new Queue<string>();
            //this.m_username = "loucacolesgamedev".ToLower();
            //this.m_password = "oauth:idzb5toppv4xxihjonx823mno9uvbf";
            //this.m_channelName = "loucacolesgamedev".ToLower();

            this.m_username = GameManager.m_gameManager.GetUsername().ToLower();
            this.m_password = GameManager.m_gameManager.GetPassword();
            this.m_channelName = GameManager.m_gameManager.GetChatroom().ToLower();

            this.m_chatCommandId = "PRIVMSG";
            //this.m_chatPrefix = $":{m_username}!{m_username}@{m_username}.tmi.chat.twitch.tv {m_chatCommandId} #{m_channelName} : ";
            this.m_chatPrefix = ":" + m_username + "!" + m_username + "@" + m_username + ".tmi.chat.twitch.tv " + m_chatCommandId + " #" + m_channelName + " : ";

            Reconnect();
        }
    }

    private void Reconnect()
    {
        m_tcpClient = new TcpClient("irc.chat.twitch.tv", 6667);
        m_streamReader = new StreamReader(m_tcpClient.GetStream());
        m_streamWriter = new StreamWriter(m_tcpClient.GetStream());

        m_streamWriter.WriteLine("PASS " + m_password + Environment.NewLine
            + "NICK " + m_username + Environment.NewLine
            + "USER " + m_username + " 8 * :" + m_username);
        m_streamWriter.Flush();

        m_streamWriter.WriteLine("JOIN #" + m_channelName);
        m_streamWriter.Flush();

        m_lastMessage = DateTime.Now;
    }

    public void Update()
    {
        if (GameManager.m_gameManager.GetGameType() == GameManager.GameType.TWITCH && !m_movementScript.IsDead())
        {
            if (!m_tcpClient.Connected)
            {
                Reconnect();
            }

            TrySendMessage();

            TryReceiveMessage();
        }
    }

    private void TrySendMessage()
    {
        //if (DateTime.Now - m_lastMessage > TimeSpan.FromSeconds(0.5))
        //{
        //    if (m_messageQueue.Count > 0)
        //    {
        //        var message = m_messageQueue.Dequeue();
        //        m_streamWriter.WriteLine(m_chatPrefix + message);
        //        m_streamWriter.Flush();
        //        m_lastMessage = DateTime.Now;
        //    }
        //}
        if (m_messageQueue.Count > 0)
        {
            var message = m_messageQueue.Dequeue();
            m_streamWriter.WriteLine(m_chatPrefix + message);
            m_streamWriter.Flush();
            m_lastMessage = DateTime.Now;
        }
    }

    private void TryReceiveMessage()
    {
        if (m_tcpClient.Available > 0)
        {
            var message = m_streamReader.ReadLine();

            var iCollon = message.IndexOf(":", 1);
            if (iCollon > 0)
            {
                var command = message.Substring(1, iCollon);
                if (command.Contains(m_chatCommandId))
                {
                    var iBang = command.IndexOf("!");
                    if (iBang > 0)
                    {
                        var speaker = command.Substring(0, iBang);
                        var chatMessage = message.Substring(iCollon + 1);
                        ReceiveMessage(speaker, chatMessage);
                    }
                }
            }
        }
    }

    private void ReceiveMessage(string _speaker, string _chatMessage)
    {
        print(_speaker + ":" + _chatMessage);
        var message = _speaker + ": " + Environment.NewLine + _chatMessage;
        TwitchCanvas.m_instance.UpdateChatText(message);

        if (_chatMessage.ToLower() == "!up")
        {
            SendTwitchMessage("Moving Up, " + _speaker);
            m_movementScript.SetDirection(Vector3.up, Direction.Directions.UP);
        }
        else if (_chatMessage.ToLower() == "!down")
        {
            SendTwitchMessage("Moving Down, " + _speaker);
            m_movementScript.SetDirection(Vector3.down, Direction.Directions.DOWN);
        }
        else if (_chatMessage.ToLower() == "!right")
        {
            SendTwitchMessage("Moving Right, " + _speaker);
            m_movementScript.SetDirection(Vector3.right, Direction.Directions.RIGHT);
        }
        else if (_chatMessage.ToLower() == "!left")
        {
            SendTwitchMessage("Moving Left, " + _speaker);
            m_movementScript.SetDirection(Vector3.left, Direction.Directions.LEFT);
        }
        else if (_chatMessage.ToLower() == "!help" || _chatMessage.ToLower() == "help")
        {
            SendTwitchMessage(_speaker + ", enter ! followed by direction, e.g. !up");
        }
    }

    private void SendTwitchMessage(string _message)
    {
        m_messageQueue.Enqueue(_message);
    }
}