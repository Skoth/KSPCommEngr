﻿#region license

/* The MIT License (MIT)

 * Copyright (c) 2016 Skoth

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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KSPCommEngr
{
    public static class Grid
    {
        private static int width = 100;
        private static int height = 100;
        private static int[,] grid = new int[width,height];
        public static readonly float UnitLength = 20f;

        public static int[,] Instance { get { return grid; } }

        public static void ModifyGrid(bool set, params Rect[] positions)
        {
            int indices = 0;
            foreach(var position in positions)
            {
                indices = RectToGridCoords(position);
                
            }
        }

        public static float SnapToGrid(float point)
        {
            return ((int)point / (int)UnitLength) * UnitLength;
        }

        public static float[][] Coordinates = new float[][]
            {
                Enumerable.Range(0, 100).Select(x => (float)x).ToArray()
            };

        private static int RectToGridCoords(Rect pos)
        {
            return 0;
        }
    }
}
