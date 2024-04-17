using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelect : MonoBehaviour
{
    public PlayerInputManager inputManager;
    public GameObject bandit;
    public GameObject sheriff;

    private void Update()
    {
        if(GameObject.FindGameObjectWithTag("Sheriff"))
        {
            if(inputManager.enabled)
            {
                PlayerInputManager.instance.playerPrefab = bandit;
            }
            
        }
        else
        {
            if (inputManager.enabled)
            {
                PlayerInputManager.instance.playerPrefab = sheriff;
            }
        }
    }
}
