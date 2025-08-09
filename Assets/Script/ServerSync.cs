using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ServerSync : MonoBehaviour
{
    public SimpleGame playerScript;
    private string serverUrl = "https://localhost:7047/api/data";

    void Start()
    {
        StartCoroutine(SendPlayerDataToServer());

        StartCoroutine(GetPlayerDataFromServer());
    }

    void Update()
    {
        if (Time.frameCount % (3 * 60) == 0)
        {
            StartCoroutine(SendPlayerDataToServer());
        }
    }

    IEnumerator SendPlayerDataToServer()
    {
        string jsonData = JsonUtility.ToJson(playerScript.playerInfo, true);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
        request.certificateHandler = new AcceptAllCertificates();
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Data sent to server.");
        else
            Debug.LogError("Error sending data: " + request.error);
    }

    IEnumerator GetPlayerDataFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);
        request.certificateHandler = new AcceptAllCertificates();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PlayerData serverData = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);


            playerScript.playerInfo = serverData;

            Debug.Log("Data received from server: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error getting data: " + request.error);
        }
    }
}

public class AcceptAllCertificates : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData) => true;
}
