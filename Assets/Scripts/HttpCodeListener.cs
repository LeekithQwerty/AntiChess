using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
public class HttpCodeListener
{
    private HttpListener listener;
    private Thread listenerThread;
    private Action<string> onCodeFetched;

    private const string responseHtml = "Success, you can return to the app now!"; // TODO: Change this to a successful html response

    public HttpCodeListener(int port)
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}/");
        listener.Prefixes.Add($"http://127.0.0.1:{port}/");
        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
    }

    public void StartListening(Action<string> callback)
    {
        onCodeFetched = callback;

        listener.Start();

        listenerThread = new Thread(ListeningThread);
        listenerThread.Start();
        Debug.Log("Start Listening");
    }

    public void StopListening()
    {
        listener.Stop();
        onCodeFetched = null;
    }

    private void ListeningThread()
    {
        while (listener.IsListening)
        {
            var result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();
            Debug.Log(result);
        }
    }

    private void ListenerCallback(IAsyncResult result)
    {
        var context = listener.EndGetContext(result);
        Debug.Log("Listening Call back 1 ");
        if (!context.Request.QueryString.AllKeys.Contains("code"))
        {
            Debug.Log("Return");

            return;
        }

        Debug.Log("Listening Call back 2 ");
        UnityMainThreadDispatcher.Instance().Enqueue(() => onCodeFetched?.Invoke(context.Request.QueryString.Get("code")));

        var buffer = Encoding.UTF8.GetBytes(responseHtml);

        context.Response.ContentLength64 = buffer.Length;
        var st = context.Response.OutputStream;
        st.Write(buffer, 0, buffer.Length);
        context.Response.Close();
    }
}
