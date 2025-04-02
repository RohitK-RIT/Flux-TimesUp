using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

public class PlayerDebuffHandler : MonoBehaviour
{
    public static PlayerDebuffHandler Instance;
    private float originalSpeed;
    private PlayerController player;

    void Awake() { Instance = this; }
    void Start() { player = FindObjectOfType<PlayerController>(); originalSpeed = player.Stats.movementSpeed; }

    public void ApplySlow(float duration) {
        Debug.Log("boss is slowing player movement");
        //StartCoroutine(SlowPlayer(duration));
    }

    IEnumerator SlowPlayer(float duration) {
        player.Stats.movementSpeed *= 0.5f;
        yield return new WaitForSeconds(duration);
        player.Stats.movementSpeed = originalSpeed;
    }
}
