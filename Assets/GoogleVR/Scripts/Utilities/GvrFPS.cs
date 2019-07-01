// Copyright 2015 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.IO;
using System.Linq;


[RequireComponent(typeof(Text))]
public class GvrFPS : MonoBehaviour {
  private const string DISPLAY_TEXT_FORMAT = "{0} FPS";//"{0} msf\n({1} FPS)";
  private const string MSF_FORMAT = "#.#";
  private const float MS_PER_SEC = 1000f;
  List<double> fps_list = new List<double>();
    int deviation = 0;  

  private Text textField;
  private float fps = 120;

  public Camera cam;

  void Awake() {
    textField = GetComponent<Text>();
  }

  void Start() {
    if (cam == null) {
       cam = Camera.main;
    }

    if (cam != null) {
      // Tie this to the camera, and do not keep the local orientation.
      transform.SetParent(cam.GetComponent<Transform>(), true);
    }
  }

  void LateUpdate()
  {
    float deltaTime = Time.unscaledDeltaTime;
    float interp = deltaTime / (0.5f + deltaTime); //previously it was 0.5f
    float currentFPS = 1.0f / deltaTime;
    fps = Mathf.Lerp(fps, currentFPS, interp);
    float msf = MS_PER_SEC / fps;
    //Debug.Log("\n FPS = " + fps);
    
        //textField.text = string.Format(DISPLAY_TEXT_FORMAT,
        //    msf.ToString(MSF_FORMAT), Mathf.RoundToInt(fps));
    //fps = (int)(1.0f / Time.smoothDeltaTime);
    //Debug.Log("\n Time.deltatime = " + Time.deltaTime);
    textField.text = string.Format(DISPLAY_TEXT_FORMAT, Mathf.RoundToInt(fps));
    //fps_list.Add(fps);
    //if (fps < 60) deviation += 1;
    //Debug.Log("\n fps recorded for = " + fps_list.Count + "times, its below 60 for = " + deviation + "times, and the avg fps = " + fps_list.Average());

  }
}
