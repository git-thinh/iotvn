using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace iotvn
{
    class App
    {
        static App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];
                string resourceName = @"DLL\" + comName + ".dll";
                var assembly = Assembly.GetExecutingAssembly();
                resourceName = typeof(App).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        //Debug.WriteLine(resourceName);
                    }
                    else
                    {
                        byte[] buffer = new byte[stream.Length];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                ms.Write(buffer, 0, read);
                            buffer = ms.ToArray();
                        }
                        asm = Assembly.Load(buffer);
                    }
                }
                return asm;
            };
        }


        const int _BUFFER_SIZE = 1024 * 1024 * 50; // 50 KB

        static void Main(string[] args)
        {
            var server = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 12345);
            server.Start();
            try
            {
                while (true)
                {
                    TcpClient clientAccept = server.AcceptTcpClient();
                    Thread t = new Thread(new ParameterizedThreadStart((obj)=> {
                        TcpClient client = (TcpClient)obj;
                        var stream = client.GetStream();
                        try
                        {
                            byte[] bufs = new byte[_BUFFER_SIZE];
                            int i = stream.Read(bufs, 0, bufs.Length);
                            //if (i > 36)
                            //{
                            //    MSG_TYPE type = (MSG_TYPE)bufs[0];
                            //    string data = Encoding.UTF8.GetString(bufs, 1, i - 1);
                            //    if (data.Length > 36) data = data.Substring(36);
                            //    Console.WriteLine("{0}\t\t\t={1}", type, data);
                            //    ___process_message(stream, type, data);
                            //}
                            client.Close();
                        }
                        catch
                        {
                            client.Close();
                        }
                    }));
                    t.Start(clientAccept);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }

        }
    }
}
