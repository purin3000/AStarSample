using UnityEngine;
using UnityEngine.UI;
using System;

public class UIRoot : MonoBehaviour {
  [SerializeField]
  private DropdownXY texSize;

  [SerializeField]
  private DropdownX seed;

  [SerializeField]
  private DropdownX wallCount;

  [SerializeField] 
  private DropdownXY startPos;

  [SerializeField]
  private DropdownXY goalPos;

  [SerializeField]
  private Button createNew;

  public void SetupTexSize(int minX, int maxX, int minY, int maxY, int xIndex, int yIndex, Action<int, int> listner) {
    texSize.Setup(minX, maxX, minY, maxY, xIndex, yIndex, listner);
  }

  public void SetupSeed(int[] list, int index, Action<int> listner) {
    seed.Setup(list, index, listner);
  }

  public void SetupWallCount(int[] list, int index, Action<int> listner) {
    wallCount.Setup(list, index, listner);
  }

  public void SetupStartPos(int minX, int maxX, int minY, int maxY, int xIndex, int yIndex, Action<int, int> listner) {
    startPos.Setup(minX, maxX, minY, maxY, xIndex, yIndex, listner);
  }

  public void SetupGoalPos(int minX, int maxX, int minY, int maxY, int xIndex, int yIndex, Action<int, int> listner) {
    goalPos.Setup(minX, maxX, minY, maxY, xIndex, yIndex, listner);
  }

  public void SetupCreateNew(Action listner) {
    SetupButton(createNew, listner);
  }

  private static void SetupButton(Button button, Action listner) {
    button.onClick.AddListener(() => listner.Invoke());
  }
}

