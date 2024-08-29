using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClassDemoTCP
{
    public class Server
    {
        private const int PORT = 727; 


        public void Start()
        {
            List<Task> tasks = new List<Task>();
            List<TcpClient> sockets = new List<TcpClient>();
            //definerer server
            TcpListener server = new TcpListener(System.Net.IPAddress.Loopback, PORT); 
            server.Start();
            Console.WriteLine("Server startet");
            //Venter på klinet
            while (true)
            {
                TcpClient socket = server.AcceptTcpClient();
                if (socket != null)
                {
                    sockets.Add(socket);
                    tasks.Add(new Task(() => {
                        NumberOfWordsForOneClient(socket); 

                    }
                    
                    )); 
                    
                }
                foreach (Task t in tasks)
                {
                    t.Start(); 
                }

                Task.WaitAny(Task.CompletedTask); 
            }
            
            

            
            



        }

        public int NumberOfWordsForOneClient(TcpClient socket)
        {
            //Definerer kommunkation til strings
            StreamReader sr = new StreamReader(socket.GetStream());
            StreamWriter sw = new StreamWriter(socket.GetStream());
            // Ekko-kammer starter her
            try
            {
                string l = sr.ReadLine();
                if (l != null)
                {
                    int numberofwords = l.Split(' ').Length;
                    Console.WriteLine($"Modtaget: {l}, {numberofwords} ord");
                    sw.WriteLine($"{l}");
                    sw.WriteLine($"Modtaget: {l}, {numberofwords} antal ord");
                    sw.Flush();
                    return numberofwords;
                    }
            }
            catch (Exception ex) {
                Console.WriteLine("Client disconnected");
            }
            return 0; 
        }
    }
}
