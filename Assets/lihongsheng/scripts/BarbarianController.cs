﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianController : MonoBehaviour {
    private Animator barbarianAnimator;
    public float startFightDistance;
    private float distanceBarbarianPlayer;
    public GameObject player;
    public float walkSpeed;
    public CameraMove cm;
    public bool freezeXY;
    private float startX;
    private float startY;
    private Animator Player;

	void Start () {
        barbarianAnimator = this.GetComponent<Animator>();
        if(freezeXY)
        {
            startX = this.transform.position.x;
            startY = this.transform.position.y;
        }
	}
	
	void LateUpdate () {
        distanceBarbarianPlayer = Vector3.Distance(player.transform.position,this.transform.position);
		if(startFightDistance>distanceBarbarianPlayer) 
        {
            barbarianAnimator.SetBool("Fight",true);
            return;
        }
        this.transform.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
        Debug.Log("\n the location of the monster is =" + this.transform.position + "orientation of the monster is = " + this.transform.rotation);
        if(freezeXY)
        {
            this.transform.position = new Vector3(startX,startY,this.transform.position.z);
        }
	}
    public void BeHitted()
    {
		barbarianAnimator.Play("Cry");
        this.transform.Translate(Vector3.back * Time.deltaTime * 0.4f);
        //Player.Play("jump");
        //barbarianAnimator.Play("Frown");
    }
    public void SakeCamera()
    {
        cm.ShakeCamera(0.1f);
    }
}
