using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DazCamUI.Controller
{
    class SocketConnection
    {
        public static string SendCommand(string command,string address, int timeoutSeconds = 0)
        {
            command += "<EOT>";

            string responseText = "";
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(address, 80);
                    socket.Send(Encoding.UTF8.GetBytes(command), command.Length, SocketFlags.None);

                    // Default timeout is 5 days (beyond a practical time), because a job can run for hours. Otherwise specified number of seconds for shorter operations
                    // that expect an immediate response (like ping).
                    var timeoutAfter = DateTime.Now.AddSeconds(timeoutSeconds);
                    if (timeoutSeconds == 0) timeoutAfter = DateTime.Now.AddDays(5);

                    while (true)
                    {
                        int bytesReceived = socket.Available;
                        if (bytesReceived > 0)
                        {
                            // Get response
                            byte[] buffer = new byte[bytesReceived];
                            int byteCount = socket.Receive(buffer, bytesReceived, SocketFlags.None);
                            responseText += new string(Encoding.UTF8.GetChars(buffer));

                            if (responseText.Contains("<EOT>"))
                            {
                                responseText = responseText.Remove(responseText.Length - 5);
                                
                                return responseText;
                            }
                        }

                        if (DateTime.Now > timeoutAfter)
                        {
                            return "Timeout!";
                        }
                    }
                }
            }

            catch (SocketException ex)
            {
                return "SE!" + ex.Message;
            }
        }
    }
}
