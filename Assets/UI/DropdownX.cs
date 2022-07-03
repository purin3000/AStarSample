using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DropdownX : MonoBehaviour {
  [SerializeField]
  Text label;

  [SerializeField]
  Dropdown dropdownX;

  private static void SetDropdownOptions(Dropdown dropdown, string[] list) {
    dropdown.ClearOptions();
    dropdown.AddOptions(list.Select(s => new Dropdown.OptionData(s)).ToList());
  }

  public void Setup(string[] xList, int index, Action<int> listner) {
    SetDropdownOptions(dropdownX, xList);
    dropdownX.onValueChanged.AddListener((_) => {
      if (listner != null) {
        listner(dropdownX.value);
      }
    });
    dropdownX.value = index;
  }

  public void Setup(int[] list, int index, Action<int> listner) {
    Setup(list.Select(val => val.ToString()).ToArray(), index, listner);
  }
}

