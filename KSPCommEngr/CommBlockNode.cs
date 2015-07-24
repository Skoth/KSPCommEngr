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

using System;
using UnityEngine;
using KSP;
using KSPCommEngr.Extensions;

namespace KSPCommEngr
{
    /*
     * 
     *      +------O------+               +------O------+
     *      |             |               |             |
     *      |             |               |             |
     *      O      X      O - - - > - - - O      X      O
     *      |             |               |             |
     *      |             |               |             |
     *      +------O------+               +------O------+
     * 
     */

    public enum nodeFaces
    {
        BandpassFilter,
        DataSource,
        HighpassFilter,
        LocalOscillator,
        LowpassFilter,
        Multiplier
    }

    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class CommBlockNode : MonoBehaviour 
    {
        public CommLinkNode topLink { get; set; }
        public CommLinkNode rightLink { get; set; }
        public CommLinkNode bottomLink { get; set; }
        public CommLinkNode leftLink { get; set; }
        public Rect nodePosition;
        public Texture2D nodeFace; // icon

        public CommBlockNode(string nodeFace = "")
        {
            
            if (nodeFace.Equals(string.Empty))
            {

            }
            nodePosition = new Rect(0, 0, 300, 300);
            nodePosition = nodePosition.CenterScreen();
            
        }
        
        public void SetVisibility()
        {

        }
    }

    public class CommLinkNode
    {

    }
}