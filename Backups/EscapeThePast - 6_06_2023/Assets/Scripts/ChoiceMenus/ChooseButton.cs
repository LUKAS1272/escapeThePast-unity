using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseButton : MonoBehaviour
{
    public bool isAvailable;

    public void ChoiceMade() {
        Debug.Log($"You chose index {gameObject.transform.GetSiblingIndex()}, which availability is {isAvailable}");
        
        if (!isAvailable) {
            StartCoroutine(writeError(gameObject.transform.parent.transform.parent.GetChild(1).GetChild(gameObject.transform.GetSiblingIndex()).transform.GetComponent<Text>().text)); // Gets the label of pressed button
        }
    }

    int ongoingErrorMessages = 0;
    IEnumerator writeError(string inputValue) {
        ChooseMenu.Instance.errorText.text = $"\"{inputValue}\" option is not available at this moment...";

        ongoingErrorMessages++;
        yield return new WaitForSeconds(5);
        ongoingErrorMessages--;

        if (ongoingErrorMessages == 0) {
            ChooseMenu.Instance.errorText.text = "";
        }
    }
}
