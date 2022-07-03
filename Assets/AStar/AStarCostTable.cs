using astar.impl;
using System;
using UnityEngine;

namespace astar {
  public class AStarCostTable {
    private readonly int[] costTable;
    public readonly int Width;
    public readonly int Height;

    public AStarCostTable(int[] costTable, int width, int height) {
      this.costTable = costTable;
      this.Width = width;
      this.Height = height;
    }

    public int GetCost(Vector2Int pos, AStarWork parent) => GetCost(pos.x, pos.y, parent);
    
    public int GetCost(int x, int y, AStarWork parent) {
      var cost = costTable[Width * y + x];
      if (parent == null) {
        return cost;
      }
      return cost + parent.Cost;
    }

    public void ForEach(Action<int, int, int> action) {
      for (int x = 0; x < Width; x++) {
        for (int y = 0; y < Height; y++) {
          var cost = costTable[Width * y + x];
          action(x, y, cost);
        }
      }
    }
  }
}
