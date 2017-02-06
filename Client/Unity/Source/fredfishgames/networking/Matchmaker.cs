using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace fredfishgames.networking
{
    [System.Serializable]
    class MatchmakingThread {
        public MatchmakingThread(Thread t, ThreadType ty, Action<string> callback) {
            thread = t;
            type = ty;
            Callback = callback;
        }
        public enum ThreadType {
            Get,
            Status,
            Delete,
            Send
        }
        public ThreadType type;
        public bool done = false;
        public string args;
        public Thread thread;
        public Action<string> Callback;
    }
    [System.Serializable]
    public class Matchmaker
    {
        public string Address;
        public int Port;
        List<MatchmakingThread> threads;
        List<int> threadsToWatch;
        public Matchmaker(string address, int port) {
            Address = address;
            Port = port;
            threads = new List<MatchmakingThread>();
            threadsToWatch = new List<int>();
        }

        public void Update()
        {
            foreach (int i in threadsToWatch)
            {
                if (threads[i].done)
                {
                    threadsToWatch.Remove(i);
                    threads[i].Callback(threads[i].args);
                    break;
                }
            }
        }


        /// <summary>
        /// This gets a server to play on from the matchmaking server.
        /// Returns true if successful.
        /// Returns false if unsuccessful.
        /// </summary>
        /// <param name="callback">A method with a string parameter, where the match's info is passed once found</param>
        public Thread GetServer(Action<string> callback) {
            Thread thread = new Thread(() => getServer(threads.Count - 1));
            MatchmakingThread mmthread = new MatchmakingThread(thread, MatchmakingThread.ThreadType.Get, callback);
            threads.Add(mmthread);
            threadsToWatch.Add(threads.Count - 1);
            thread.Start();
            return thread;
        }

        public Thread SendServer(Action<string> callback) {
            Thread thread = new Thread(() => sendServer(threads.Count - 1,new Byte[5] {4, 0, 0, 0, 0}));
            MatchmakingThread mmthread = new MatchmakingThread(thread, MatchmakingThread.ThreadType.Send, callback);
            threads.Add(mmthread);
            threadsToWatch.Add(threads.Count - 1);
            thread.Start();
            return thread;
        }

        public Thread GetStatus(Action<string> callback)
        {
            Thread thread = new Thread(() => sendServer(threads.Count - 1, new Byte[5] {2, 0, 0, 0, 0 }));
            MatchmakingThread mmthread = new MatchmakingThread(thread, MatchmakingThread.ThreadType.Status, callback);
            threads.Add(mmthread);
            threadsToWatch.Add(threads.Count - 1);
            thread.Start();
            return thread;
        }

        public Thread ClearServer(Action<string> callback)
        {
            Thread thread = new Thread(() => sendServer(threads.Count - 1, new Byte[5] { 5, 0, 0, 0, 0 }));
            MatchmakingThread mmthread = new MatchmakingThread(thread, MatchmakingThread.ThreadType.Delete, callback);
            threads.Add(mmthread);
            threadsToWatch.Add(threads.Count - 1);
            thread.Start();
            return thread;
        }

        private void sendServer(int threadId, byte[] command) {
            string args;
            try
            {
                
                byte[] r = sendCommand(Address, Port, command);
                if (r[0] == 0)
                {
                    args = "0";
                }
                else {
                    args = "1";
                }
            }
            catch (SocketException ex) {
                args = "0";
            }
            Debug.Log("Thread id " + threadId + " finished");
            threads[threadId].args = args;
            threads[threadId].done = true;
        }

        private void getServer(int threadId) {
            string ip;
            try
            {
                Debug.Log("Finding a server");
                byte[] adrs = sendCommand(Address, Port, new Byte[5] { 3, 0, 0, 0, 0 });
                
                if (adrs[0] == 1)
                {
                    ip = adrs[1] + "." + adrs[2] + "." + adrs[3] + "." + adrs[4];
                }
                else
                {
                    ip = "0";
                }
            }
            catch (SocketException ex) {
                ip = "0";
            }
            Debug.Log("Thread id "+threadId+" finished");
            threads[threadId].args = ip;
            threads[threadId].done = true;
         }

        private byte[] sendCommand(string address, int prt, byte[] command)
        {
            try
            {
                TcpClient client = new TcpClient(address, prt);
                NetworkStream stream = client.GetStream();
                stream.Write(command, 0, command.Length);
                byte[] r = new byte[5];
                stream.Read(r, 0, 5);
                stream.Close();
                client.Close();
                return r;

                //textBox4.Text += "\r\n" + reader.ReadLine();
            }

            catch (Exception ex)
            {
                Debug.LogError(ex);
                return new byte[5] { 0, 0, 0, 0, 0 };
            }
        }
    }
}
