using UnityEngine;


public class AStarTest {
  public static void Test() {
    int[] TestTable =
    {
          0,2,0,0,0,
          0,1,0,0,0,
          0,0,0,0,0,
          0,1,1,1,1,
          0,0,0,0,3,
        };

    var astar = new AStar();
    astar.Calculate(TestTable, 5, 5, new Vector2Int(1, 0), new Vector2Int(4, 4));


    Debug.Log("=== ワークの状態");
    foreach (var stat in astar.GetWorkStatus()) {
      Debug.Log(stat);
    }

    Debug.Log("=== 経路");
    foreach (var result in astar.GetResult()) {
      Debug.LogFormat("{0} {1}", result.x, result.y);
    }
  }

}
