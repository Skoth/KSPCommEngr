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
    // Inspired by Unity 5 2D: Pathfinding lynda.com course by Jesse Freeman
    public class Search
    {
        //private List<Node> _path;

        public Graph graph;
        public List<Node> reachable;
        public List<Node> explored;
        public List<Node> path;
        public Node goalNode;
        public int iterations;
        public bool finished;
        public IEnumerable<int> tempVals;

        public Search(Graph g)
        {
            graph = g;
        }

        public void Start(Node start, Node goal)
        {
            // Clear and reset all values
            reachable = new List<Node>();
            reachable.Add(start);

            goalNode = goal;

            explored = new List<Node>();
            path = new List<Node>();
            iterations = 0;

            for (int i = 0; i < graph.nodes.Length; ++i)
            {
                graph.nodes[i].Clear();
            }
        }

        public void Step()
        {
            if (path.Count > 0) return;

            if (reachable.Count == 0)
            {
                finished = true;
                return;
            }

            iterations++;

            // Get node
            var node = ChooseNode();

            // Check if node is goal, if it is, fill out path with previous nodes
            if (node == goalNode)
            {
                while (node != null)
                {
                    path.Insert(0, node);
                    node = node.previous;
                }
                finished = true;
                return;
            }

            reachable.Remove(node);
            explored.Add(node);

            for (var i = 0; i < node.adjacencyList.Count; ++i)
            {
                AddAdjacent(node, node.adjacencyList[i]);
            }
        }

        // Critical Section for implementing 3-Node Corner detection
        public void AddAdjacent(Node node, Node adjacent)
        {
            if (explored.Contains(adjacent) || reachable.Contains(adjacent))
                return;

            adjacent.previous = node;
            reachable.Add(adjacent);
        }

        public Node ChooseNode()
        {
            tempVals = reachable.Select(node => CornerHeuristic(node) + graph.Distance(node, goalNode));
            return reachable.OrderBy(node => CornerHeuristic(node) + graph.Distance(node, goalNode)).First();
        }

        private int CornerHeuristic(Node node)
        {
            if (node.previous != null)
            {
                if (node.previous.previous != null)
                {
                    if ((node.previous.Id == node.Id + graph.columns && node.previous.previous.Id == node.Id + graph.columns - 1) || // 1
                        (node.previous.Id == node.Id + graph.columns && node.previous.previous.Id == node.Id + graph.columns + 1) || // 2
                        (node.previous.Id == node.Id - graph.columns && node.previous.previous.Id == node.Id - graph.columns - 1) || // 3
                        (node.previous.Id == node.Id - graph.columns && node.previous.previous.Id == node.Id - graph.columns + 1) || // 4
                        (node.previous.Id == node.Id + 1 && node.previous.previous.Id == node.Id + 1 - graph.columns) || // 5
                        (node.previous.Id == node.Id - 1 && node.previous.previous.Id == node.Id - 1 - graph.columns) || // 6
                        (node.previous.Id == node.Id + 1 && node.previous.previous.Id == node.Id + 1 + graph.columns) || // 7
                        (node.previous.Id == node.Id - 1 && node.previous.previous.Id == node.Id - 1 + graph.columns))   // 8
                    {
                        return 50;
                    }
                }
            }
            return 0;
        }
    }
}
