using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelect : MonoBehaviour
{
    public PlayerInputManager inputManager;
    public GameObject bandit;

    private void Update()
    {
        if(GameObject.FindGameObjectWithTag("Sheriff"))
        {
            PlayerInputManager.instance.playerPrefab = bandit;
        }
    }
}
