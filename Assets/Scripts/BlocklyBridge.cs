
using UnityEngine;

public class BlocklyBridge : MonoBehaviour
{
    public string url;
    public int port;
    WebsocketServer websocket;


    // Start is called before the first frame update
    private void Start()
    {
        websocket = WebsocketServer.Start(url, port);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
    }
}
