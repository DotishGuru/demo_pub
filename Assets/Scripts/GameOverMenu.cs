using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public void Restart()
    {
        GameManager.Instance.RestartLevel();
    }
}
