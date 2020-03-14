using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarCore
{
    Block[,] map;
    SortedSet<ValueTuple<int,int>> openList;
    HashSet<ValueTuple<int,int>> closeList;
    Dictionary<ValueTuple<int,int>, int> costs;
    Dictionary<ValueTuple<int, int>, ValueTuple<int, int>> parents;
    BlockCMP cmp;

    public AStarCore(Block[,] map)
    {
        this.map = map;
        costs = new Dictionary<ValueTuple<int, int>, int>();
        parents = new Dictionary<(int, int), (int, int)>();
        cmp = new BlockCMP();
        cmp.costs = costs;
        openList = new SortedSet<ValueTuple<int, int>>(cmp);
        closeList = new HashSet<(int, int)>();
    }

    private void Reset()
    {
        costs.Clear();
        closeList.Clear();
        openList.Clear();
        parents.Clear();
    }

    public bool FindPath(ValueTuple<int,int> star, ValueTuple<int, int> target,List<ValueTuple<int, int>> ans)
    {
        Reset();
        costs[star] = 0;
        openList.Add(star);
        while(openList.Count != 0)
        {
            var node = openList.Max;
            openList.Remove(node);
            closeList.Add(node);
            AddAround(node);
            if(node == target)
            {
                GetAns(ans,target);
                return true;
            }
        }
        return false;
    }

    private void GetAns(List<(int,int)> ans,(int,int) target)
    {
        while (parents.ContainsKey(target))
        {
            ans.Add(target);
            target = parents[target];
        }
        ans.Reverse();
    }

    private void AddAround(ValueTuple<int,int> node)
    {
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;
                if (node.Item1 + i < 0 || node.Item1 + i >= map.GetLength(0)) continue;
                if (node.Item2 + j < 0 || node.Item2 + j >= map.GetLength(1)) continue;
                var now = (node.Item1 + i, node.Item2 + j);
                if (closeList.Contains(now) || map[now.Item1,now.Item2].IsBarrier) continue;


                var cost = costs[node] + ((i == 0 || j == 0) ? 10 : 14);
                if (costs.ContainsKey(now))
                {
                    if (costs[now] > cost)
                    {
                        openList.Remove(now);
                        costs[now] = cost;
                        parents[now] = node;
                        openList.Add(now);
                    }
                }
                else
                {
                    costs[now] = cost;
                    parents[now] = node;
                    openList.Add(now);
                }
            }
        }
    }

    public  class BlockCMP : IComparer<ValueTuple<int, int>>
    {
        public ValueTuple<int, int> target;
        public Dictionary<ValueTuple<int, int>, int> costs;

        public int Compare(ValueTuple<int, int> x, ValueTuple<int, int> y)
        {
            int ans = GetCost(y) - GetCost(x);
            if (ans == 0) ans = x.Item1 - y.Item1;
            return ans;
        }

        public int GetCost(ValueTuple<int,int> pos)
        {
            return costs[pos] + Math.Abs(pos.Item1 - target.Item1) *10+ Math.Abs(pos.Item2 - target.Item2)*10;
        }
    }
}
