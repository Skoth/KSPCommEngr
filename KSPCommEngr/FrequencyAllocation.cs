﻿#region license

/* The MIT License (MIT)

 * Copyright (c) 2015 Skoth

 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion

using UnityEngine;
using System.Collections;
using KSPCommEngr.Extensions;
using KSP.IO;
using System.Linq;
using System;

namespace KSPCommEngr
{
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class FrequencyAllocation : MonoBehaviour
    {
        private static Rect windowPosition = new Rect();
        private static Rect windowPos2 = new Rect();
        private GUIStyle windowStyle, labelStyle;
        private bool hasInitStyles = false;
        public Texture2D image = new Texture2D(400, 100); //400 pixels wide by 100 pixels tall
        private Texture2D iconImg = new Texture2D(30, 30);
        private byte[] iconImgData;
        private LineRenderer line = null;

        public void Awake()
        {
            CommEngrLog.Log("Frequency Allocation MonoBehaviour Activated");
        }

        public void Start()
        {

            line = new GameObject("Line").AddComponent<LineRenderer>();
            line.transform.parent = transform;
            line.useWorldSpace = false;
            line.transform.localPosition = Vector3.zero;
            line.transform.localEulerAngles = Vector3.zero;
            line.material = new Material(Shader.Find("Particles/Additive"));
            line.SetColors(Color.red, Color.yellow);
            line.SetWidth(3.0f, 0.0f);
            line.SetVertexCount(2);
            line.SetPosition(0, Vector3.zero); 
            //camera.
            line.SetPosition(1, new Vector3(1.0f, 1.0f, 0.0f) * 2);

            try {
                if (File.Exists<File>("smiley.png"))
                {
                    CommEngrLog.Log("smiley.png found!");
                    iconImg.LoadImage(File.ReadAllBytes<File>("smiley.png"));
                }
                else
                {
                    CommEngrLog.Log("smiley.png not found!");
                }
            } catch(Exception ex) {
                CommEngrLog.Log(ex.Message, LogType.Exception);
            }

            // Signal tests with sinusoid
            Signal q = new Signal(x => Mathf.Sin(x));
            CommEngrLog.Log("Frequency Allocation - Sine Signal generated.");
            //q.DisplaySignal();

            // Test channel noise distortion
            q = Channel.AdditiveNoise(q);
            CommEngrLog.Log("Frequency Allocation - Channel Noise added to Signal.");
            //q.DisplaySignal();

            if (!hasInitStyles) InitStyles();

            drawImage();
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
        }

        public void Update()
        {

        }

        private void OnDraw()
        {

            windowPosition = GUILayout.Window(10, windowPosition, OnWindow, "Frequency Allocation Chart", GUIStyle.none);

            if (windowPosition.x == 0f && windowPosition.y == 0f)
                windowPosition = windowPosition.CenterScreen();
        }

        // warning: do NOT test/observe input in OnGUI (reserve that for Update()); OnGUI is called multiple times per frame
        public void OnGUI() 
        {
            // Test examples for Legacy GUI Scripting Guide: http://docs.unity3d.com/Manual/gui-Basics.html
            // Note: only windows are draggable; they act as containers of controls--hence
            // the following elements are un-draggable controls absolutely positioned
            GUI.Box(new Rect(400, 400, 100, 135), "Box", windowStyle);

            if (GUI.Button(new Rect(410, 430, 80, 20), "Allocate new frequency band channel"))
            {
                CommEngrLog.Log("Allocating a new frequency band for channel...");
            }

            if (GUI.Button(new Rect(420, 460, 80, 20), "Display existing channels"))
            {
                CommEngrLog.Log("Displaying existing channels...");
            }

            if (Time.time % 2 < 1)
            {
                if (GUI.Button(new Rect(430, 490, 200, 20), "Meet the flashing button"))
                {
                    CommEngrLog.Log("You clicked me!");
                }
            }
            if (GUI.changed)
            {
                CommEngrLog.Log("Change!");
            }
        }

        private void OnWindow(int windowId)
        {
            GUILayout.BeginVertical();
                // windowPos2 = GUILayout.Window(11, windowPos2, OnWin2, "Inner Window Test", windowStyle);
                GUI.DrawTexture(new Rect(10f, 10f, 400f, 100f), image, ScaleMode.StretchToFill);
                GUILayout.Label("Frequency in Hz", labelStyle);
            GUILayout.EndVertical();

            GUI.DragWindow();
        }
        private void OnWin2(int wId)
        {
            GUILayout.BeginHorizontal();
                GUILayout.Button("Button!");
            GUILayout.EndHorizontal();
        }

        private void InitStyles()
        {
            windowStyle = new GUIStyle(HighLogic.Skin.window);
            windowStyle.fixedWidth = 400f;
            windowStyle.fixedHeight = 400f;
                
            labelStyle = new GUIStyle(HighLogic.Skin.label);
            labelStyle.stretchWidth = true;
            //labelStyle.DrawCursor

            hasInitStyles = true;
        }

        // Code provided from KSP Wiki @: http://wiki.kerbalspaceprogram.com/wiki/Module_code_examples
        // Special thanks to the author of the "Drawing and displaying a 2D image" snippet
        void drawImage()
        {
            // Set all the pixels to black
            int size = image.height * image.width;

            Color[] colors = Enumerable.Repeat(Color.black, size).ToArray();
            image.SetPixels(colors);

            int lineWidth = 3; //draw a curve 3 pixels wide
            for (int x = 0; x < image.width; x++)
            {
                int fx = f(x);
                for (int y = fx; y < fx + lineWidth; y++)
                {
                    image.SetPixel(x, y, Color.red);
                    //CommEngrLog.Log("drawImage() - (" + x.ToString() + ", " + y.ToString() + ")");
                }
            }

            image.Apply();
        }

        //the function to plot:
        int f(int x)
        {
            return 2 * x;
        }
    }
}
