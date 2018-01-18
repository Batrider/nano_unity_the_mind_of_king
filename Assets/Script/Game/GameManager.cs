using SimpleJson;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int mainPlayerUid;
    public Text text;
    public Queue<byte[]> quesitionQueue = new Queue<byte[]>();
    public Queue<byte[]> answerQueue = new Queue<byte[]>();

	public List<Player> players = new List<Player>();
    /// <summary>
    /// net connect
    /// </summary>
    void Start()
    {
        NetworkManager.StartConnect(() =>
        {
            NetworkManager.EnterWorld((msg) =>
            {
                Debug.Log("enter");
                string jsonData = Encoding.UTF8.GetString(msg);
                EnterWorldResponse jsonObj = SimpleJson.SimpleJson.DeserializeObject<EnterWorldResponse>(jsonData);
                mainPlayerUid = jsonObj.id;
                NetworkManager.StarXService.On("questionNotify", QuesitionNotifyHandler);
                NetworkManager.StarXService.On("onMessage", onMessageHandler);
                NetworkManager.StarXService.On("leave", onLeaveHandler);
                NetworkManager.StarXService.On("AnswerNotify", AnswerNotify);
				NetworkManager.StarXService.On("endCompetition", EndCompetitionNotify);


            });
        });
    }

    void onMessageHandler(byte[] msg)
    {
        Debug.Log("OnMessage");
    }
    void onLeaveHandler(byte[] msg)
    {
        Debug.Log("OnLeave");
    }
	void EndCompetitionNotify(byte[] msg)
	{
		string jsonData = Encoding.UTF8.GetString (msg);
		Debug.LogWarning (jsonData);
		EndCompetition pData = SimpleJson.SimpleJson.DeserializeObject<EndCompetition> (jsonData);
		Debug.Log ("the win id:" + pData.winId);
	}

    /// <summary>
    /// on player data update
    /// </summary>
    /// <param name="msg"></param>
    void AnswerNotify(byte[] msg)
    {
		answerQueue.Enqueue (msg);
    }
    /// <summary>
    /// player clone queue 
    /// </summary>
    public void Update()
    {
        if (quesitionQueue.Count > 0)
        {
            ShowQuestion(quesitionQueue.Dequeue());
        }
        if(answerQueue.Count >0)
        {
			ShowAnswer (answerQueue.Dequeue ());
        }
    }
    /// <summary>
    /// create self
    /// </summary>
    /// <param name="msg"></param>
    void QuesitionNotifyHandler(byte[] msg)
    {
        Debug.Log("recieve question");
        quesitionQueue.Enqueue(msg);
    }
    /// <summary>
    /// update player position
    /// if player is nil,create
    /// if player != nil,update pos
    /// </summary>
    /// <param name="data"></param>
    void ShowQuestion(byte[] msg)
    {
        string jsonData = Encoding.UTF8.GetString(msg);
        Debug.Log(jsonData);
        Quesition jsonObj = SimpleJson.SimpleJson.DeserializeObject<Quesition>(jsonData);
        text.text = jsonObj.sDesc;
    }
    void ShowAnswer(byte[] msg)
    {
        string jsonData = Encoding.UTF8.GetString(msg);
		Debug.Log (jsonData);
        AnswerNotify pData = SimpleJson.SimpleJson.DeserializeObject<AnswerNotify>(jsonData);
		if (players.Exists (x => x.id == pData.id)) {
		} else {
			Player p = new Player ();
			p.id = pData.id;
			p.score = pData.score;
			players.Add (p);
		}
    }

    public void OnDestroy()
    {
        NetworkManager.StarXService.Notify("Room.LeaveRoom", new byte[] { });
    }
}

public class Quesition
{
    public int id;
    public string sDesc;
    public string sDesc1;
    public string sDesc2;
    public string sDesc3;
    public string sDesc4;
    public int iRightIdx;
}

public class AnswerNotify
{
    public int id;
    public bool isRight;
    public int score;
}

public class EndCompetition
{
	public int winId;
}

public class Player
{
    public int id;
    public string name;
    public int score;
}

public class EnterWorldResponse
{
    public int id;
}

public class LeaveWorldResponse
{
    public int id;
}

public class StartRequest
{
    public int id;
}