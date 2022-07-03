using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace astar.impl {
  public class AStarWork : IComparable {
    private enum Status {
      Open,
      Close,
    }

    public readonly Vector2Int Position;

    public int Cost { get; private set; }
    public int Score { get; private set; }

    private Status status;
    private AStarWork parent;

    public bool IsOpen() => status == Status.Open;
    public bool ParentAreValid() => parent != null;

    public int CompareTo(object obj) {
      if (obj is not AStarWork otherWork) {
        return 1;
      }
      return Score - otherWork.Score;
    }

    public AStarWork(Vector2Int pos) { Position = pos; status = Status.Open; }

    public void OpenNode(int cost, int score, [CanBeNull] AStarWork parent) {
      this.status = Status.Open;
      this.Cost = cost;
      this.Score = score;
      this.parent = parent;
    }

    public void CloseNode() => status = Status.Close;

    public List<Vector2Int> GetPath() {
      var ret = new List<Vector2Int>();

      var work = this;
      while (work != null) {
        ret.Add(work.Position);
        work = work.parent;
      }
      ret.Reverse();

      return ret;
    }

  }
}
