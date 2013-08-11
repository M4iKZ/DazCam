using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.SPOT;

namespace DazCAM
{
    public class SocketServer : IDisposable
    {
        #region Declarations

        public delegate void RequestHandler(object sender, RequestHandlerEventArgs e);

        public class RequestHandlerEventArgs
        {
            public readonly string RequestCommand;
            public string ResponseMessage { get; set; }
            public bool ResponseError { get; set; }

            public RequestHandlerEventArgs(string requestCommand)
            {
                RequestCommand = requestCommand;
            }
        }

        private Socket _socket = null;

        #endregion

        #region Properties

        public event RequestHandler OnRequest;

        #endregion

        #region Constructors

        public SocketServer(int portToListenOn = 80)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, portToListenOn));

            // Show our IP address when debugging
            Debug.Print(Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress);

            //Start listen for web requests
            _socket.Listen(10);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Enters an endless loop to listen for requests on the specified Port, raising an OnRequest() event each time a request is received.
        /// </summary>
        public void ListenForRequest()
        {
            string requestText;
            int bytesReceived;
            int byteCount;
            string responseText;

            while (true)
            {
                using (Socket clientSocket = _socket.Accept())
                {
                    requestText = "";
                    DateTime timeoutAfter = DateTime.Now.AddSeconds(2);
                    while (DateTime.Now <= timeoutAfter)
                    {
                        bytesReceived = clientSocket.Available;

                        if (bytesReceived > 0)
                        {
                            // Get request
                            byte[] buffer = new byte[bytesReceived];
                            byteCount = clientSocket.Receive(buffer, bytesReceived, SocketFlags.None);
                            requestText += new string(Encoding.UTF8.GetChars(buffer));

                            if (requestText.Substring(requestText.Length - 5) == "<EOT>")
                            {
                                // Handle Request
                                requestText = requestText.Substring(0, requestText.Length - 5);
                                responseText = HandleRequest(requestText);
                                responseText += "<EOT>";

                                // Return Response to caller
                                clientSocket.Send(Encoding.UTF8.GetBytes(responseText), responseText.Length, SocketFlags.None);
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private string HandleRequest(string requestText)
        {
            RequestHandlerEventArgs e = new RequestHandlerEventArgs(requestText);
            if (OnRequest != null) OnRequest(this, e);

            return (e.ResponseError ? "ERROR:" : "OK:") + e.ResponseMessage;
        }

        #endregion

        #region IDisposable Members

        ~SocketServer()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (_socket != null)
                _socket.Close();
        }

        #endregion
    }
}
