using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class PlayerData
{
    public string PlayerName = "Aaron";
    public int PlayerHealth = 100;
    public int Score;
    public Vector3 PlayerPosition;
}

public class SimpleGame : MonoBehaviour
{
    public PlayerData playerInfo = new PlayerData();

    void Start()
    {
        playerInfo.PlayerName = "Aaron";
        playerInfo.PlayerHealth = 100;
        playerInfo.Score = 0;
        playerInfo.PlayerPosition = transform.position;
        PrintPlayerData();
    }

    void Update()
    {
        // Move player
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 5f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 5f;
        transform.Translate(moveX, 0, moveZ);
        playerInfo.PlayerPosition = transform.position;

        // Take damage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerInfo.PlayerHealth -= 10;
            PrintPlayerData();
        }

        // Gain score
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerInfo.Score += 100;
            PrintPlayerData();
        }
    }

    public void PrintPlayerData()
    {
        string jsonData = JsonUtility.ToJson(playerInfo, true);
        Debug.Log("Current Player Data (JSON):\n" + jsonData);
    }
}
