using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverLines : MonoBehaviour
{
    int index = -1;
    int width = -1;

    public void OnPointerEnter() {
        if (!PuzzleGenerator.Instance.isSolved && SolutionChecker.Instance.hearts != 0 && !PuzzleGenerator.Instance.disableLines.isOn) {
            index = transform.GetSiblingIndex();
            width = (int)Mathf.Sqrt(transform.parent.childCount);

            for (int left = index - 1; left >= index - (index % width); left--) {
                if (transform.parent.GetChild(left).gameObject.GetComponent<SlotSwitcher>() == null || !transform.parent.GetChild(left).gameObject.GetComponent<SlotSwitcher>().isOn)
                    transform.parent.GetChild(left).GetChild(0).gameObject.SetActive(true);
            }

            for (int top = index - width; top > 0; top -= width) {
                if (transform.parent.GetChild(top).gameObject.GetComponent<SlotSwitcher>() == null || !transform.parent.GetChild(top).gameObject.GetComponent<SlotSwitcher>().isOn)
                    transform.parent.GetChild(top).GetChild(0).gameObject.SetActive(true);
            }

            for (int right = index + 1; right < index - (index % width) + width; right++) {
                if (transform.parent.GetChild(right).gameObject.GetComponent<SlotSwitcher>() == null || !transform.parent.GetChild(right).gameObject.GetComponent<SlotSwitcher>().isOn)
                    transform.parent.GetChild(right).GetChild(0).gameObject.SetActive(true);
            }

            for (int down = index + width; down < transform.parent.childCount; down += width) {
                if (transform.parent.GetChild(down).gameObject.GetComponent<SlotSwitcher>() == null || !transform.parent.GetChild(down).gameObject.GetComponent<SlotSwitcher>().isOn)
                    transform.parent.GetChild(down).GetChild(0).gameObject.SetActive(true);
            }
        } else {
            index = -1;
        }
    }

    public void OnPointerExit() {
        if (index != -1) {
            for (int left = index - 1; left >= index - (index % width); left--) {
                transform.parent.GetChild(left).GetChild(0).gameObject.SetActive(false);
            }

            for (int top = index - width; top > 0; top -= width) {
                transform.parent.GetChild(top).GetChild(0).gameObject.SetActive(false);
            }

            for (int right = index + 1; right < index - (index % width) + width; right++) {
                transform.parent.GetChild(right).GetChild(0).gameObject.SetActive(false);
            }

            for (int down = index + width; down < transform.parent.childCount; down += width) {
                transform.parent.GetChild(down).GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
