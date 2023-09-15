using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private float deathZoneCountdown = 5f;
    public GameObject countdownText;
    private TMP_Text _countdownTextMeshPro;

    void Awake() 
    {
        countdownText = GameObject.FindGameObjectWithTag("Timer");
        _countdownTextMeshPro = countdownText.GetComponent<TMP_Text>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(/*other.gameObject.CompareTag("DeathZone")
            || */other.gameObject.CompareTag("LevelEnd"))
        {
            GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("DeathZone"))
        {
            deathZoneCountdown -= Time.deltaTime;
            _countdownTextMeshPro.text = $"Countdown: {deathZoneCountdown.ToString("F2")}";
            if(deathZoneCountdown <= 0)
            {
                deathZoneCountdown = 0;
                _countdownTextMeshPro.text = $"Countdown: {deathZoneCountdown.ToString("F2")}";
                GameManager.Instance.GameOver();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("DeathZone"))
        {
            deathZoneCountdown = 5f;
            _countdownTextMeshPro.text = $"Countdown: {deathZoneCountdown.ToString("F2")}";
        }
    }
}
