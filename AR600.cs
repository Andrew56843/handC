using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Thread;
using System.Threading;
using System.IO;
using System;

public class AR600
{
    byte[] WRBUFFER = new byte[111];
    byte[] SETBUFFER = new byte[111];
    byte[] BUF1 = new byte[1024];
    byte[] BUF2 = new byte[1024];

    UdpClient _UDPServer;
    IPEndPoint iep = null;
    UdpClient server = new UdpClient(10002);
    Thread Link;
    bool EN;

	public bool START()
	{
        try
        {
            _UDPServer = new UdpClient();
            _UDPServer.Connect("192.169.1.5", 10002);
            _UDPServer.DontFragment = true;

            Link = new Thread(EXCHANGE);
            Link.Start();
            return true;
        }
        catch
        {
            return false;
        }
	}

    public void CLOSE()
    {
        Link.Abort();
        _UDPServer = null;
    }



    private void EXCHANGE()
    {
        bool start_CMD = false;
        if (start_CMD)
        {
            try
            {
                _UDPServer.Send(SETBUFFER, SETBUFFER.Length);
                SETBUFFER[0] = 0;
                EN = true;
                BUF1 = server.Receive(iep);
                BUF2 = server.Receive(iep);
            }
            catch
            {
                sleep(10);
            }
            start_CMD = false;
        }
        else
        {
            try
            {
                _UDPServer.Send(WRBUFFER, WRBUFFER.Length);
                WRBUFFER[0] = 0;
                EN = true;
                BUF1 = server.Receive(iep);
                BUF2 = server.Receive(iep);
            }
            catch
            {
                sleep(10);
            }
        }
    
    }
}
