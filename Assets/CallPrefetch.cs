using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.IO;


public class CallPrefetch : MonoBehaviour
{

    public TCPTestClient ttc;
    public int fid;
    int count = 0;
    public playerController pc_1;
    int fid_max = 25921;
    string video_string;
    List<int> fid_list;


    // Use this for initialization
    void Start()
    {

        ttc = GameObject.Find("udpclient").GetComponent<TCPTestClient>();
        pc_1 = GameObject.Find("playerMove/ALplayer").GetComponent<playerController>();
        ttc.ConnectToTcpServer();


    }

    void check_neighbours(int direction)
    {
        if ((fid + direction <= fid_max) && (fid + direction > 0))
        {
            video_string = @"C:/Users/spauldsnl/Documents/decoding_videos/server_fetch/" + (fid + direction).ToString() + ".mp4";
            if (!(File.Exists(video_string)))
            {
                fid_list.Add(fid + direction);
            }
        }
    }

    // Update is called once per frame
    //void LateUpdate()
    public IEnumerator prefetch()
    {
        //StartCoroutine("Send");
        if (pc_1.pos_change == 1)
        {
            fid = pc_1.fID;
            fid_list = new List<int>();
            //checking what neighbour fids are already there or not
            check_neighbours(1);
            check_neighbours(161);
            check_neighbours(-1);
            check_neighbours(-161);
            Send(fid_list);
            StartCoroutine(ttc.ListenForData());
            pc_1.pos_change = 0;
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {
            List<int> fid_list = new List<int>();
            fid_list.Add(fid);
            fid_list.Add(fid + 161);
            fid_list.Add(fid + 1);
            //before sending the request for fids we need to check whether those fids already existed or not then pass to network thread
            Send(fid_list);
            StartCoroutine(ttc.ListenForData());
            count++;
        }
        */
        yield return null;
    }

    //IEnumerator Send()
    void Send(List<int> fid_list)
    {
        //else fid = 0;
        //fid++;
        ttc.Sendfid(fid_list);
        //if (fid < 2)

        //yield return new WaitForSeconds(2f);
    }
}










































/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.IO;

public class CallPrefetch : MonoBehaviour
{

    public TCPTestClient ttc;
    public playerController pc_1;
    
    int count = 0;
    int fid = 0;
    int fid_max = 25291;
    string path = "C:/Users/spauldsnl/Documents/decoding_videos/server_fetch";
    string video_path = "";
    List<int> fid_list;

    // Use this for initialization
    void Start()
    {

        ttc = GameObject.Find("Callprefetch").GetComponent<TCPTestClient>();
        pc_1 = GameObject.Find("playerMove/ALplayer").GetComponent <playerController> ();
        ttc.ConnectToTcpServer();


    }

    void check_neighbours(int direction)
    {
        if ((fid + direction <= fid_max) && (fid + direction >0))
        {
            video_path = @"C:/Users/spauldsnl/Documents/decoding_videos/server_fetch/" + (fid + direction).ToString() + ".mp4";
            if (!(File.Exists(video_path)))
            {
                fid_list.Add(fid + direction);
            }
        }
    }
    // Update is called once per frame
    //void LateUpdate()
    public IEnumerator prefetch()
    {
        //StartCoroutine("Send");
        //if (pc_1.pos_change == 1)
        {
            fid = pc_1.fID;
            fid_list = new List<int>();
            //checking what neighbour fids are already there or not
            check_neighbours(1);
            check_neighbours(161);
            check_neighbours(-1);
            check_neighbours(-161);
            Send(fid_list);
            StartCoroutine(ttc.ListenForData());
            pc_1.pos_change = 0;
        }
            //count++;
            yield return null;
    }

    //IEnumerator Send()
    void Send(List<int> fid_list)
    {
        ttc.Sendfid(fid_list);
    }
}
*/
