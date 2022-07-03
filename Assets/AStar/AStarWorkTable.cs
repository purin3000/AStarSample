using JetBrains.Annotations;
using System.Linq;
using UnityEngine;

namespace astar.impl {
  public class AStarWorkTable {
    private readonly AStarWork[] works;
    public readonly int Width;
    public readonly int Height;

    public AStarWorkTable(int width, int height) {
      works = new AStarWork[width * height];
      Width = width;
      Height = height;
    
      for (int x = 0; x < width; x++) {
        for (int y = 0; y < height; y++) {
          works[width * y + x] = new AStarWork(new Vector2Int(x, y));
        }
      }
    }

    public AStarWork GetWork(int x, int y) => works[Width * y + x];

    [CanBeNull]
    public AStarWork SafeGetWork(int x, int y) {
      if (x < 0 || x >= Width) {
        return null;
      }
      if (y < 0 || y >= Height) {
        return null;
      }
      return GetWork(x, y);
    }

    public AStarWork NearestWork(Vector2Int pos) {
      var result = works
        .Where(work => work.ParentAreValid())
        .OrderBy(work => Vector2Int.Distance(work.Position, pos))
        .FirstOrDefault();
      return result;
    }
  }
}
