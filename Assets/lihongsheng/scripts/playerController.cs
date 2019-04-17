using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.IO;

public class playerController : MonoBehaviour
{
    Animator animator;
    public GameObject VideoPlayback;
    MediaPlayerCtrl mediaPlayerCtrl;
    Vector3 ts;
    public skeletonController sc;
    public BloodBox bloodBox;
    private float allHp = 2000f;
    public float currentHp;
    public float playerPower = 1000f;
    private GameController gameController;
    public Transform playerMoveTrans;
    public float playerMoveSpeed;
    public GameObject daoEffect;
    public BarbarianController barbarianController;
    public float playerAttackRange;
    public Transform daoEffectPosition;
    public CameraMove cm;
    public Transform FallDownPosition;
    public float FallDownWalkForwardTime;
    private bool setWalk = false;
    public float FallDownUpRotation;
    public float FallDownUpTime;
    public float FallDownRotation;
    public float FallDownTime;
    public GameObject ShakeCamera;
    public float FallDownShakeTime;
    public float DelayFallDownTime;
    public float DelayPlayFurionTime;
    public bool isPlayerAttack;
    private int attackTimes;
    private int count;
    public int counter;
    private Vector3 currentRotationEuler;
    private Quaternion currentRotation;
    private Vector3 rotationDeltaEuler;
    private Quaternion rotationDelta;

    public int fID = 0;
    GstUnityBridgeTexture gubt;
    int fID_max = 25921;
    double x_max = -60f;
    double x_min = -65f;
    double z_max = 11f;
    double z_min = 6f;
    CallPrefetch cpp;
    public int pos_change = 0; //this will indicate whether player changes its position or not
    public int dir_change = 0;

    float step = 0.03125f;

    

    void Awake()
    {
        
        attackTimes = 0;
        currentHp = allHp;
        ts = this.transform.position;
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Idling", true);
        //mediaPlayerCtrl = VideoPlayback.GetComponent<MediaPlayerCtrl>();
        gubt = GameObject.Find("playerMove/ALplayer/All/Used/SphereMovie").GetComponent<GstUnityBridgeTexture>();
        cpp = GameObject.Find("Callprefetch").GetComponent<CallPrefetch>();
        
    }
   

    void Start()
    {
        count = 0;
        counter = 0;
        gameController = GameController.GetInstance();
        print("\n I am at start");
        


        if (!gameController.isPlayerBloodVisible)
        {
            bloodBox.gameObject.SetActive(false);
        }
        if (gameController.isRecordMode)
        {
            StartCoroutine("DelayPlayFallDown");
        }
    }
    /*
    void Update()
    {
        //remember the On Computer Debug in inspector (GameController)
        print("\n I am entering update");
        
   
    }
    */


    int frameID = 0;

    IEnumerator Play_Video(int direction)
    {
        
        if ((fID + direction < fID_max) && (fID + direction >= 0))
        {
            gubt.Stop();
            fID = fID + direction;
            String m_URI = "file:///C:/Users/spauldsnl/Documents/decoding_videos/viking_texas_Ionly/" + fID.ToString() + ".mp4"; //D:/decoding_videos/of_Ionly/"
            //String m_URI = "file:///C:/Users/spauldsnl/Documents/decoding_videos/server_fetch/" + fID.ToString() + ".mp4";
            gubt.Setup(m_URI, 0, 0);
            gubt.Play();
            StartCoroutine(cpp.prefetch());
            pos_change = 1;
            dir_change = 1;
        }
        yield return null;

    }


    void Update()
    {
                ts = this.transform.position;
                //Debug.Log("\n current position is= " + this.transform.position.x + "," + this.transform.position.y + "," + this.transform.position.z);
                if (Input.GetKey(KeyCode.W))
                {
                    //if ((ts.z >= z_min) && (ts.z < z_max))
                    {
                        StartCoroutine(Play_Video(1));
                        Debug.Log("Moving Forward");
                        animator.SetBool("Idling", false);
                        ts.z += step;
                        this.transform.position = ts;
                        //this.transform.position = new Vector3(ts.x, ts.y, ts.z += (Time.deltaTime * playerMoveSpeed)); 
                        //mediaPlayerCtrl.setDirection(1);
                    }
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    //if ((ts.x >= x_min) && (ts.x < x_max))
                    {
                        StartCoroutine(Play_Video(161));
                        Debug.Log("Moving Right");
                        animator.SetBool("Idling", false);
                        ts.x += step;
                        this.transform.position = ts;
                        //this.transform.position += Vector3.right * Time.deltaTime * playerMoveSpeed;
                    }
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    //if ((ts.z > z_min) && (ts.z <= z_max))
                    {
                        StartCoroutine(Play_Video(-1));
                        Debug.Log("Moving BackWard");
                        animator.SetBool("Idling", false);
                        ts.z -= step;
                        this.transform.position = ts;
                        //this.transform.position = new Vector3(ts.x, ts.y, ts.z -= (Time.deltaTime * playerMoveSpeed));
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {

                    //if ((ts.x > x_min) && (ts.x <= x_max))
                    {
                        StartCoroutine(Play_Video(-161));
                        Debug.Log("Moving Left");
                        animator.SetBool("Idling", false);
                        ts.x -= step;
                        this.transform.position = ts;
                        //this.transform.position += Vector3.left * Time.deltaTime * playerMoveSpeed;
                    }
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    animator.SetTrigger("Use");
                    animator.SetBool("Idling", false);
                    Debug.Log("Shooting \n");
                }

                else
                {
                    animator.SetBool("Idling", true);
                    //Debug.Log("\n I am pausing the mediaplayerctrl");
                    
                }
                //Debug.Log("\n app button is touching and the modified position is= " + this.transform.position);

                if (isPlayerAttack)
                {
                    Debug.Log("set trigger\n");
                    animator.SetTrigger("Use");
                }
    }
 

    public void ScBeHitted()
    {
        if(sc!= null)
        {
           sc.BeHitted(); 
        }
    }

    public void BarnarianBeHitted()
    {
        setWalk = false;
        attackTimes++;
        //????
		if (attackTimes  > 0)
		{
			isPlayerAttack = false;
		}
        if(barbarianController != null)
        {
            float distance = Vector3.Distance(this.transform.position, barbarianController.transform.position);
            //Debug.Log(distance);
            if(distance <playerAttackRange)
            {
                //Debug.Log("BarnarianBeHitted");
                Instantiate(daoEffect,daoEffectPosition);
                barbarianController.BeHitted();
                cm.ShakeCamera(0.1f);
            }
        }
    }


    public void SetBoold(float changeBlood)
    {
        currentHp = currentHp - changeBlood;
        if (currentHp <= 0.0f){
            //??
            currentHp = 0.0f;
            animator.SetInteger("Death", 2);
			if (sc != null)
			{
				sc.isPlayerDie = true;
			}
        }
        if(gameController.isPlayerBloodVisible)
        {
            bloodBox.OnBloodChange(allHp, currentHp);
        }
    }

    /// <summary>
    /// ??
    /// </summary>
    private void FallDownAction_1()
    {
        setWalk = true;
		
		Hashtable args = new Hashtable();
		
        args.Add("easeType", iTween.EaseType.linear);
		// x y z ????????
        args.Add("x", FallDownPosition.position.x);
		args.Add("y", FallDownPosition.position.y);
		args.Add("z", FallDownPosition.position.z);

		//?????
		args.Add("time", FallDownWalkForwardTime);
        if(gameController.isFallDownMode)
        {
            args.Add("oncomplete", "FallDownAction_2");
        }else{
            //args.Add("oncomplete", "LookRight");
        }
        iTween.MoveTo(this.gameObject, args);

    }

    private void FallDownAction_2()
	{
        Debug.Log("FallDownAction_2");
        setWalk = false;
		//?????????iTween??????  
		Hashtable args = new Hashtable();
		//args.Add("delay", FallDownShakeTime);
		// x y z ?????  
		args.Add("x", FallDownUpRotation);
		args.Add("y", 0f);
		args.Add("z", 0f);
        //??
        args.Add("time", FallDownUpTime);
		args.Add("oncompleteparams", FallDownTime);
		if (gameController.isFallDownMode)
		{
            args.Add("oncomplete", "FallDownAction_3");
			cm.ShakeCamera(FallDownShakeTime);
        }else
        {
            args.Add("oncomplete", "LookRight");
        }
		iTween.RotateTo(ShakeCamera, args);
	}



	private void LookRight()
	{
        //setWalk = false;
        //Debug.Log("LookRight");
		//?????????iTween??????
          
		Hashtable args = new Hashtable();
        args.Add("easeType", iTween.EaseType.easeOutBack);
		args.Add("from", 0f);
		args.Add("to", 80f);
		//??????ValueTo?????  
		args.Add("onupdate", "AnimationUpdata");
		args.Add("onupdatetarget", gameObject);
		args.Add("oncomplete", "LookLeft");
        args.Add("time", 0.7f);
		iTween.ValueTo(ShakeCamera, args);
	}

	public void AnimationUpdata(object obj)
	{
		float per = (float)obj;
        cm.transform.rotation = Quaternion.Euler(new Vector3(0f, per, 0f));
	}

    public IEnumerator DelayPlayFallDown()
    {
        yield return new WaitForSeconds(1f);
        FallDownAction_1();
        yield return new WaitForSeconds(1f);
        LookRight();
        yield return null;
    }

}
