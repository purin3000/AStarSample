using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Cost {
  public class Data {
    public Data(int cost, Color color) {
      Cost = cost;
      Color = color;
    }
    public readonly int Cost;
    public readonly Color Color;
  }

  private readonly static Data[] COST_TABLE = {
    new Data(10, Color.white),
    new Data(11, Color.white*0.9f),
    new Data(12, Color.white*0.8f),
    new Data(13, Color.white*0.7f),
    new Data(14, Color.white*0.6f),
    new Data(15, Color.white*0.5f),
    new Data(WALL, Color.black),
  };

  private static Dictionary<int, Data> dicData;
  private static IReadOnlyDictionary<int, Data> DIC_COST => dicData ?? (dicData = COST_TABLE.ToDictionary(data => data.Cost));

  public const int WALL = 99;

  public static int GetCostLength() => COST_TABLE.Length;
  public static int GetCost(int index) => COST_TABLE[index].Cost;
  public static Color CostToColor(int cost) => DIC_COST[cost].Color;
}
