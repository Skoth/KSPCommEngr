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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KSPCommEngr;
using System.Collections.Generic;
using System.Diagnostics;

namespace CommEngrTest
{
    [TestClass]
    public class SearchTests
    {
        public Graph graph;
        public Search search;

        [TestInitialize]
        public void Setup()
        {
            int[,] nodes = new int[,] {
                { 1, 1, 0, 1, 0 },
                { 0, 1, 0, 1, 1 },
                { 1, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 1, 1, 1, 0, 0 }
            };
            graph = new Graph(nodes);
            search = new Search(graph);
        }

        [TestMethod]
        public void ChooseNode()
        {
            Node goal = graph.nodes[graph.nodes.Length - 1];
            search.goalNode = goal;
            List<Node> reachable = new List<Node>();
            reachable.Add(graph.nodes[15]);
            reachable.Add(graph.nodes[21]);
            search.reachable = reachable;
            Node optimalNode = graph.nodes[21];
            Assert.AreSame(search.ChooseNode(), optimalNode);

            search.goalNode = graph.nodes[0];
            optimalNode = graph.nodes[15];
            Assert.AreSame(search.ChooseNode(), optimalNode);
        }

        [TestMethod]
        public void LineOverlapAllowed()
        {
            Node start = graph.nodes[5];
            Node goal = graph.nodes[7];

            search.Start(start, goal);
            while (!search.finished)
            {
                search.Step();
            }
            int expectedPathLength = 3;

            Assert.AreEqual(expectedPathLength, search.path.Count, "Path length not expected size: check the search path: ", search.path);
        }

        [TestMethod]
        public void BoundaryCrossingsAllowed()
        {
            Node start = graph.nodes[5];
            Node goal = graph.nodes[15];
            int expectedPathCount = 3;
            List<Node> expectedPath = new List<Node>
            {
                graph.nodes[5],
                graph.nodes[10],
                graph.nodes[15]
            };

            search.Start(start, goal);
            while(!search.finished)
            {
                search.Step();
            }

            Assert.AreEqual(expectedPathCount, search.path.Count, 
                String.Format("Expected path count: {0}; Search result path count: {1}.", 
                    expectedPathCount, search.path.Count));
            foreach(var node in search.path)
            {
                CollectionAssert.Contains(expectedPath, node);
            }
        }

        [TestMethod]
        public void MinimumCorners()
        {
            Node start = graph.nodes[5];
            Node goal = graph.nodes[18];
            List<Node> expectedPath = new List<Node> {
                graph.nodes[5],
                graph.nodes[10],
                graph.nodes[15],
                graph.nodes[16],
                graph.nodes[17],
                graph.nodes[18]
            };

            search.Start(start, goal);
            while (!search.finished)
            {
                search.Step();
            }

            foreach(var node in search.path)
            {
                CollectionAssert.Contains(expectedPath, node, 
                    "Node not in expected path (node, search path):", search.path);
            }
        }
    }
}
