using astar;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestMain : MonoBehaviour {
  private const int MAX_TX = 16;
  private const int MAX_TY = 16;

  private readonly static int[] SEED_LIST = new[] { 0, 100, 200, 300, -1 };
  private readonly static int[] WALL_COUNT_LIST = new[] { 0, 10, 30, 50, 100 };

  private readonly static Vector2Int DEFAULT_START_POS = new Vector2Int(0, 15);
  private readonly static Vector2Int DEFAULT_GOAL_POS = new Vector2Int(15, 0);
  private const int DEFAULT_SEED_INDEX = 0;
  private const int DEFAULT_WALL_COUNT_INDEX = 2;

  [SerializeField]
  private UIRoot ui;

  [SerializeField]
  private Canvas canvas;

  [SerializeField]
  private Transform scaleRoot;

  [Range(1, 100)]
  [SerializeField]
  private float texScale = 20;

  private Vector2Int texSize;
  private int seed;
  private int wallCount = 300;
  private Vector2Int startPosition = new Vector2Int(2, 2);
  private Vector2Int goalPosition = new Vector2Int(13, 13);

  private int[] costTable;
  private Vector2Int costTableSize;

  private List<Texture2D> removeTextureList = new List<Texture2D>();
  private List<GameObject> removeGoList = new List<GameObject>();

  private void Start() {
    ui.SetupTexSize(1, MAX_TX, 1, MAX_TX, 15, 15, (x, y) => {
      var width = x + 1;
      var height = y + 1;
      texSize = new Vector2Int(width, height);
    });

    ui.SetupSeed(SEED_LIST, DEFAULT_SEED_INDEX, (index) => {
      seed = SEED_LIST[index];
    });

    ui.SetupWallCount(WALL_COUNT_LIST, DEFAULT_WALL_COUNT_INDEX, (index) => {
      wallCount = WALL_COUNT_LIST[index];
    });

    ui.SetupStartPos(0, MAX_TX, 0, MAX_TY, DEFAULT_START_POS.x, DEFAULT_START_POS.y, (x, y) => {
      startPosition = new Vector2Int(x, y);
    });

    ui.SetupGoalPos(0, MAX_TX, 0, MAX_TY, DEFAULT_GOAL_POS.x, DEFAULT_GOAL_POS.y, (x, y) => {
      goalPosition = new Vector2Int(x, y);
    });

    ui.SetupCreateNew(() => CalcAStar(AStartTest1New));
  }

  private void CalcAStar(Func<List<Vector2Int>> func) {
    UpdateCostTable();

    removeTextureList.ForEach(tex => Destroy(tex));
    removeTextureList.Clear();
    removeGoList.ForEach(go => Destroy(go));
    removeGoList.Clear();

    var (width, height) = (costTableSize.x, costTableSize.y);
    var tex = TextureUtil.CreateTextureWithTable(width, height, costTable);
    removeTextureList.Add(tex);
    removeGoList.Add(TextureUtil.CreateImage(scaleRoot, tex, texScale, "MapImage").gameObject);

    var result = func();

    var tex2 = TextureUtil.CreateResultTexture(width, height, result);
    removeTextureList.Add(tex2);
    removeGoList.Add(TextureUtil.CreateImage(scaleRoot, tex2, texScale, "ResultImage").gameObject);
  }

  private void UpdateCostTable() {
    System.Random rand = 0 <= seed ? new System.Random(seed) : new System.Random();

    var (width, height) = (texSize.x, texSize.y);
    costTable = new int[width * height];
    costTableSize = texSize;
    for (int x = 0; x < width; ++x) {
      for (int y = 0; y < height; ++y) {
        var index = rand.Next(Cost.GetCostLength() - 1);
        costTable[x + y * width] = Cost.GetCost(index);
      }
    }
    for (int i = 0; i < wallCount; ++i) {
      var index = rand.Next(costTable.Length - 1);
      costTable[index] = AStarMain.COST_WALL;
    }
  }

  private List<Vector2Int> AStartTest1New() {
    var (width, height) = (costTableSize.x, costTableSize.y);
    var asCostTable = new AStarCostTable(costTable, width, height);
    var astar = new AStarMain();
    astar.Calculate(asCostTable, startPosition, goalPosition);
    return astar.GetResult();
  }
}
