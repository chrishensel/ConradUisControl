using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConradUisControl
{
    /// <summary>
    /// Represents a server, which listens on the localhost (configurable port) and interprets GET requests as commandos.
    /// </summary>
    class CucServer : IDisposable
    {
        #region Constants

        private const string ResponseSuccess = "200 OK";
        private const string ResponseError = "400 Bad Request";

        #endregion

        #region Events

        /// <summary>
        /// Raised when the user has navigated to an HTTP-method using HTTP.
        /// </summary>
        public event EventHandler<CommandEventArgs> CommandReceived;

        private void OnCommandReceived(CommandEventArgs args)
        {
            var copy = CommandReceived;
            if (copy != null)
            {
                copy(this, args);
            }
        }

        #endregion

        #region Fields

        private readonly Thread _thread;
        private readonly TcpListener _listener;

        #endregion

        #region Constructors

        public CucServer()
        {
            _listener = new TcpListener(IPAddress.Any, CucConfiguration.ListenPort);
            _thread = new Thread(StartThread);
        }

        #endregion

        #region Methods

        public void Start()
        {
            _thread.Start();
        }

        private void StartThread()
        {
            _listener.Start();
            while (true)
            {
                try
                {
                    TcpClient client = _listener.AcceptTcpClient();

                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);

                }
                catch (SocketException)
                {
                    // Ignored because it also happens when the server is closed.
                }
            }
        }

        private void HandleClientComm(object client)
        {
            using (TcpClient tcpClient = (TcpClient)client)
            {

                NetworkStream clientStream = tcpClient.GetStream();

                byte[] message = new byte[4096];
                int bytesRead;

                while (true)
                {
                    bytesRead = 0;

                    try
                    {
                        bytesRead = clientStream.Read(message, 0, 4096);
                    }
                    catch
                    {
                        //a socket error has occured
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        //the client has disconnected from the server
                        break;
                    }

                    //message has successfully been received
                    ASCIIEncoding encoder = new ASCIIEncoding();
                    string text = encoder.GetString(message, 0, bytesRead);
                    System.Diagnostics.Debug.WriteLine(text);

                    // TODO: First line is "GET / HTTP [...]"
                    string getRequestUri = null;
                    using (StringReader reader = new StringReader(text))
                    {
                        getRequestUri = reader.ReadLine();
                        getRequestUri = getRequestUri.Remove(0, getRequestUri.IndexOf('/') + 1);
                        getRequestUri = getRequestUri.Substring(0, getRequestUri.IndexOf(' '));
                    }

                    string command = null;
                    string[] parameters = null;
                    string response = ResponseSuccess;
                    CommandType commandType = CommandType.Invalid;

                    try
                    {
                        bool success = TryParseRequestUri(getRequestUri, out command, out parameters);
                        if (success)
                        {
                            // Handle special commands right here
                            switch (command)
                            {
                                case "":
                                    // Provide index page
                                    commandType = CommandType.Internal;
                                    response = Properties.Resources.IndexPage;
                                    break;
                                default:
                                    commandType = CommandType.Other;
                                    break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        switch (commandType)
                        {
                            case CommandType.Other:
                                {
                                    if (parameters == null)
                                    {
                                        parameters = new string[0];
                                    }

                                    CommandEventArgs args = new CommandEventArgs();
                                    args.Command = command;
                                    args.Parameters = parameters;

                                    OnCommandReceived(args);
                                } break;

                            case CommandType.Invalid:
                                response = ResponseError;
                                break;

                            default: break;
                        }

                        SendResponse(tcpClient, response);

                        tcpClient.Close();
                    }
                }
            }
        }

        private bool TryParseRequestUri(string requestUri, out string command, out string[] parameters)
        {
            command = requestUri;
            parameters = null;

            int parameterSign = requestUri.IndexOf('?');
            if (parameterSign != -1)
            {
                string parametersRaw = requestUri.Remove(0, parameterSign + 1);
                parameters = parametersRaw.Split('&');

                command = command.Substring(0, parameterSign);
            }
            return true;
        }

        private void SendResponse(TcpClient client, string returnCode)
        {
            NetworkStream clientStream = client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(returnCode);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        #endregion

        #region Nested types

        private enum CommandType
        {
            Invalid = 0,
            Internal,
            Other,
        }

        /// <summary>
        /// Provides event args for the case that the user has invoked via HTTP.
        /// </summary>
        public class CommandEventArgs : EventArgs
        {
            /// <summary>
            /// Gets/sets the command text. Example: http://localhost/[COMMAND]/.
            /// </summary>
            public string Command { get; set; }
            /// <summary>
            /// Gets/sets the command text. Example: http://localhost/[COMMAND]?[PARAMETER1]&[PARAMETER2].
            /// </summary>
            public string[] Parameters { get; set; }

            public CommandEventArgs()
            {
                Parameters = new string[0];
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            _listener.Stop();

            if (_thread != null)
            {
                _thread.Abort();
            }
        }

        #endregion
    }
}
