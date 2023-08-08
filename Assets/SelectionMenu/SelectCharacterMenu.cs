using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectCharacterMenu : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI name;
    private GameManager gameManager;
    private int index;

    private void Start() {
        gameManager = GameManager.instance;

        index = PlayerPrefs.GetInt("PlayerIndex");

        if (index > gameManager.characters.Count) {
            index = 0;
        }

        ChangeScreen();
    }

    private void ChangeScreen() {
        PlayerPrefs.SetInt("PlayerIndex", index);
        image.sprite = gameManager.characters[index].image;
        name.text = gameManager.characters[index].name;
    }

    public void NextCharacter() {
        if (index == gameManager.characters.Count - 1) {
            index = 0;
        } else {
            index++;
        }
        ChangeScreen();
    }

    public void PrevCharacter() {
        if (index == 0) {
            index = gameManager.characters.Count - 1;
        } else {
            index--;
        }
        ChangeScreen();
    }
}
