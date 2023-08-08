using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    private GameObject playerObject;

    void Start() {
        playerObject = GameObject.FindGameObjectWithTag("Player");

        int indexPlayer = PlayerPrefs.GetInt("PlayerIndex");
        GameObject player = Instantiate(GameManager.instance.characters[indexPlayer].playableCharacter, transform.position, Quaternion.identity);
        player.transform.SetParent(playerObject.transform);
    }
}
