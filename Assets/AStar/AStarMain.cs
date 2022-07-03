using astar.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace astar {
  public class AStarMain {
    public const int COST_WALL = 99;

    private AStarCostTable costTable;

    private AStarWorkTable workTable;
    private List<AStarWork> openList;
    private int openCount;
    private AStarWork goalWork;
    
    private List<Vector2Int> result;

    public List<Vector2Int> GetResult() { return result; }

    public void Calculate(AStarCostTable costTable, Vector2Int startPosition, Vector2Int goalPosition) {
      this.costTable = costTable;

      workTable = new AStarWorkTable(costTable.Width, costTable.Height);
      openList = new List<AStarWork>();
      openCount = 0;
      goalWork = workTable.SafeGetWork(goalPosition.x, goalPosition.y);
      if (goalWork is null) {
        Error($"error goalPosition:{goalPosition}");
        return;
      }
      
      AStarWork work = workTable.SafeGetWork(startPosition.x, startPosition.y);
      if (work is null) {
        Error($"error startPosition:{startPosition}");
        return;
      }

      // 壁をクローズしておく
      costTable.ForEach((x, y, cost) => {
        if (cost == COST_WALL) {
          workTable.GetWork(x, y).CloseNode();
        }
      });

      OpenNode(work.Position.x, work.Position.y, null);

      bool error = false;
      int maxCount = workTable.Width * workTable.Height;
      while (openCount < maxCount) {
        if (work.Position == goalPosition) {
          break;
        }

        CloseNode(work);
        OpenAround(work);
        work = openList.OrderBy(w => w.Score).FirstOrDefault();
        if (work == null) {
          error = true;
          break;
        }
      }

      if (error) {
        Debug.LogError("目的地に到達できません");
        AStarWork nearestWork = workTable.NearestWork(goalPosition);
        if (nearestWork is null) {
          Error("最寄りの目的地検索にも失敗");
          return;
        }
        result = nearestWork.GetPath();
      } else {
        result = goalWork.GetPath();
      }
    }

    private void Error(string message) {
      result = new List<Vector2Int>();
      Debug.LogError(message);
    }

    private void OpenAround(AStarWork work) {
      var (sx, sy) = (work.Position.x, work.Position.y);
      //right
      if (sx < workTable.Width - 1) {
        OpenNode(sx + 1, sy, work);
      }
      //left
      if (0 < sx) {
        OpenNode(sx - 1, sy, work);
      }
      //up
      if (0 < sy) {
        OpenNode(sx, sy - 1, work);
      }
      //down
      if (sy < workTable.Height - 1) {
        OpenNode(sx, sy + 1, work);
      }
    }

    private void OpenNode(int x, int y, AStarWork parent) {
      var nextWork = workTable.GetWork(x, y);
      if (nextWork == null || !nextWork.IsOpen()) {
        return;
      }

      // このあたりのコストの計算はシステムによる
      var cost = costTable.GetCost(x, y, parent);
      var hCost = GetHeuristicCost(nextWork, goalWork);
      var score = cost + hCost;

      if (nextWork.ParentAreValid()) {
        int nextScore = nextWork.Score;
        if (score < nextScore) {
          openList.Remove(nextWork);

          nextWork.OpenNode(cost, score, parent);
          openList.Add(nextWork);
        }
      } else {
        nextWork.OpenNode(cost, score, parent);
        openList.Add(nextWork);
        ++openCount;
      }
    }

    private void CloseNode(AStarWork work) {
      work.CloseNode();
      openList.Remove(work);
    }

    /// <summary>
    /// 予測コスト
    /// 予測はあくまでも予測なので、実コストより値を低くしておくこと。
    /// そうしないと実コストより予測が優先され、結果がおかしくなる可能性が出てくる。
    /// </summary>
    private static int GetHeuristicCost(AStarWork current, AStarWork target) {
      // ４方向
      var dx = Mathf.Abs(current.Position.x - target.Position.x);
      var dy = Mathf.Abs(current.Position.y - target.Position.y);
      return dx + dy;
    }
  }
}
