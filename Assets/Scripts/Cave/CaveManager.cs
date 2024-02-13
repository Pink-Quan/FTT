using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManager : MonoBehaviour
{
    public CharacterController killer;
    public CharacterController Nam;
    public PlayerController player;

    private void Start()
    {
        player.DisableMoveAndUI();
    }
}
