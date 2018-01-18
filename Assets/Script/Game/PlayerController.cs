using System.Text;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendServerNotify(1);
        }
		else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SendServerNotify(2);
        }
		else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SendServerNotify(3);
        }
		else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SendServerNotify(4);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCompetition();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveRoom();
        }
    }
    void StartCompetition()
    {
        StartRequest rq = new StartRequest();
        rq.id = GameManager.mainPlayerUid;
        string jsonData = SimpleJson.SimpleJson.SerializeObject(rq);
        NetworkManager.StarXService.Notify("Room.StartCompetition", Encoding.UTF8.GetBytes(jsonData));
    }

    void LeaveRoom()
    {
        NetworkManager.StarXService.Notify("Room.LeaveRoom", new byte[] { });
    }

    /// <summary>
    /// send the change of player's pos to server
    /// </summary>
    /// <param name="step"></param>
    void SendServerNotify(int index)
    {
        Debug.Log("Commit:" + index);
        PlayerAnswer data = new PlayerAnswer();
        data.answerIndex = index;
        string jsonData = SimpleJson.SimpleJson.SerializeObject(data);
        NetworkManager.StarXService.Notify("Room.CommitAnswer", Encoding.UTF8.GetBytes(jsonData));
    }
}

/// <summary>
/// the data use exchange data with server
/// </summary>
public class PlayerData
{
    public ulong id;
    public float posX;
    public float posY;
}

public class PlayerAnswer
{
	public int answerIndex;
}



