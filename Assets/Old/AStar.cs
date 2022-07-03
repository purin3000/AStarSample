using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar {
  int m_width;
  int m_height;
  int[] m_costTable;

  ASWork[] m_work;

  ASWork m_goal;
  List<Vector2Int> m_result;

  // List<ASWork> m_openList;
  PriorityQueue<int, ASWork> m_openList2;

  const int CalcMax = 5000;

  public List<Vector2Int> GetResult() { return m_result; }

  public List<string> GetWorkStatus() { return m_work.Select(_work => _work.GetDumpStr()).ToList(); }

  public void Calculate(int[] costTable, int width, int height, Vector2Int startPosition, Vector2Int goalPosition) {
    m_width = width;
    m_height = height;
    m_costTable = costTable;

    m_work = new ASWork[costTable.Length];

    for (int i = 0; i < m_work.Length; ++i) {
      int x = i % m_width;
      int y = i / m_width;
      m_work[i] = new ASWork(new Vector2Int(x, y), Status.Open);
    }

    var startIndex = _GetIndex(startPosition);
    var goalIndex = _GetIndex(goalPosition);

    m_goal = m_work[goalIndex];
    // m_openList = new List<ASWork>();
    m_openList2 = new PriorityQueue<int, ASWork>();

    var startWork = m_work[startIndex];

    _OpenNode(startWork.m_position, null);

    for (int cnt = 0; true; ++cnt) {
      if (CalcMax <= cnt) {
        Debug.Log("回数制限により計算停止");
        break;
      }

      _CloseNode(startWork);
      _OpenAround(startWork);
      startWork = _GetMinNode();
      if (startWork == null) {
        break;
      }
      // if (work.m_position.x == m_goal.m_position.x && work.m_position.y == m_goal.m_position.y)
      // {
      //     _CloseNode(work);
      //     break;
      // }
    }

    m_result = m_goal.GetPath();
  }

  void _OpenAround(ASWork work) {
    int sx = work.m_position.x;
    int sy = work.m_position.y;
    //right
    if (sx < m_width - 1) {
      _OpenNode(new Vector2Int(sx + 1, sy), work);
    }
    //left
    if (0 < sx) {
      _OpenNode(new Vector2Int(sx - 1, sy), work);
    }
    //up
    if (0 < sy) {
      _OpenNode(new Vector2Int(sx, sy - 1), work);
    }
    //down
    if (sy < m_height - 1) {
      _OpenNode(new Vector2Int(sx, sy + 1), work);
    }
  }

  void _OpenNode(Vector2Int pos, ASWork parent) {
    var work = _GetWork(pos);
    if (work.m_status != Status.Open) {
      return;
    }
     
    int gCost = m_costTable[_GetIndex(pos)];
    //gCost *= gCost * gCost;
    switch (gCost) {
      case 0: gCost = 1; break;
      case 1: gCost = 5; break;
      case 2: gCost = 1000; break;
    }
    if (parent != null) {
      gCost += parent.m_gCost;
    }

    var hCost = Mathf.Abs(m_goal.m_position.x - work.m_position.x) + Mathf.Abs(m_goal.m_position.y - work.m_position.y);

    if (work.ParentAreValid()) {
      int cost = gCost + hCost;
      int workCost = work.m_gCost + work.m_hCost;
      if (cost < workCost) {
        m_openList2.Remove(workCost);

        work.OpenNode(gCost, hCost, parent);
        m_openList2.Push(gCost, work);

      }
    } else {
      work.OpenNode(gCost, hCost, parent);
      m_openList2.Push(gCost, work);
    }
  }

  void _CloseNode(ASWork work) {
    work.m_status = Status.Close;

    //最小ノードを処理しているのでPopでもRemoveでも結果は変わらないはず
    m_openList2.Pop();
    // m_openList2.Remove(work.m_gCost);
  }

  ASWork _GetMinNode() {
#if true
    if (m_openList2.Count == 0) {
      return null;
    }

    return m_openList2.Peek();

#else
        if (m_openList.Count == 0)
        {
            return null;
        }

        ASWork minWork = m_openList[0];
        for (int i = 1; i < m_openList.Count; ++i)
        {
            var work = m_openList[i];
            if (work.m_score < minWork.m_score ||
             (work.m_score == minWork.m_score && work.m_hCost < minWork.m_hCost))
            {
                minWork = work;
            }
        }
        return minWork;
#endif
  }

  int _GetIndex(Vector2Int pos) { return m_width * pos.y + pos.x; }


  ASWork _GetWork(Vector2Int pos) {
    var index = _GetIndex(pos);
    if (0 <= index && index < m_work.Length) {
      return m_work[index];
    }
    return null;
  }

  enum Status {
    Open,
    Close,
  }

  private class ASWork : System.IComparable {
    public Vector2Int m_position = Vector2Int.zero;
    public Status m_status = Status.Open;

    public int m_gCost = 0;
    public int m_hCost = 0;
    public int m_score = 0;

    private ASWork m_parent = null;
    public bool ParentAreValid() => m_parent != null;

    public int CompareTo(object obj) {
      if (obj == null) {
        return 1;
      }

      var otherWork = obj as ASWork;

      return m_score - otherWork.m_score;
    }

    public ASWork(Vector2Int pos, Status status) { m_position = pos; m_status = status; }

    public void OpenNode(int gCost, int hCost, ASWork parent) {
      m_status = Status.Open;
      m_gCost = gCost;
      m_hCost = hCost;
      m_score = gCost + hCost;
      m_parent = parent;
    }

    public List<Vector2Int> GetPath() {
      var ret = new List<Vector2Int>();

      if (m_parent != null) {
        var work = this;
        while (work != null) {
          ret.Add(work.m_position);
          work = work.m_parent;
        }
        ret.Reverse();
      }
      return ret;
    }

    public string GetDumpStr() {
      if (m_parent != null) {
        return string.Format("X:{0} Y:{1} STATUS:{2} BAS:{3} HEU:{4} SCO:{5} px:{6} py:{7}", m_position.x, m_position.y, m_status, m_gCost, m_hCost, m_score, m_parent.m_position.x, m_parent.m_position.y);
      } else {
        return string.Format("X:{0} Y:{1} STATUS:{2} BAS:{3} HEU:{4} SCO:{5}", m_position.x, m_position.y, m_status, m_gCost, m_hCost, m_score);
      }
    }
  }
}
