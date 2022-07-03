using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DropdownXY : MonoBehaviour {
  [SerializeField]
  Text label;

  [SerializeField]
  Dropdown dropdownX;

  [SerializeField]
  Dropdown dropdownY;

  private static void SetDropdownOptions(Dropdown dropdown, string[] list) {
    dropdown.ClearOptions();
    dropdown.AddOptions(list.Select(s => new Dropdown.OptionData(s)).ToList());
  }

  public void Setup(string[] xList, string[] yList, int xIndex, int yIndex, Action<int, int> listner) {
    SetDropdownOptions(dropdownX, xList);
    SetDropdownOptions(dropdownY, yList);
    dropdownX.onValueChanged.AddListener((_) => {
      if (listner != null) {
        listner(dropdownX.value, dropdownY.value);
      }
    });
    dropdownY.onValueChanged.AddListener((_) => {
      if (listner != null) {
        listner(dropdownX.value, dropdownY.value);
      }
    });
    dropdownX.value = xIndex;
    dropdownY.value = yIndex;
  }

  public void Setup(int minX, int maxX, int minY, int maxY, int xIndex, int yIndex, Action<int, int> listner) {
    var xList = Enumerable.Range(minX, maxX).Select(val => val.ToString()).ToArray();
    var yList = Enumerable.Range(minY, maxY).Select(val => val.ToString()).ToArray();
    Setup(xList, yList, xIndex, yIndex, listner);
  }
}
