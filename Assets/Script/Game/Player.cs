using System.Text;
using UnityEngine;

public class Player : MonoBehaviour
{
    void OnPlayerInput()
    {
        if (Input.GetKey(KeyCode.Keypad1))
        {
            SendServerNotify(1);
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            SendServerNotify(2);
        }
        else if (Input.GetKey(KeyCode.Keypad3))
        {
            SendServerNotify(3);
        }
        else if (Input.GetKey(KeyCode.Keypad4))
        {
            SendServerNotify(4);
        }
    }
    /// <summary>
    /// send the change of player's pos to server
    /// </summary>
    /// <param name="step"></param>
    void SendServerNotify(int index)
    {
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

