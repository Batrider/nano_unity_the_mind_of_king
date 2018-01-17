using SimpleJson;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static ulong mainPlayerUid;
    public Text text;
    public Queue<Quesition> quesitionQueue = new Queue<Quesition>();
    /// <summary>
    /// net connect
    /// </summary>
    void Start()
    {
        NetworkManager.StartConnect(() =>
        {
            NetworkManager.EnterWorld((msg) =>
            {
                NetworkManager.StarXService.On("AnswerNotify", AnswerNotify);
                NetworkManager.StarXService.On("QuestionNotify", QuesitionNotify);
            });
        });
    }
    /// <summary>
    /// on player data update
    /// </summary>
    /// <param name="msg"></param>
    void AnswerNotify(byte[] msg)
    {
        string jsonData = Encoding.UTF8.GetString(msg);
        PlayerAnswer pData = SimpleJson.SimpleJson.DeserializeObject<PlayerAnswer>(jsonData);
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
    }
    /// <summary>
    /// create self
    /// </summary>
    /// <param name="msg"></param>
    void QuesitionNotify(byte[] msg)
    {
        string jsonData = Encoding.UTF8.GetString(msg);
        Quesition jsonObj = (Quesition)SimpleJson.SimpleJson.DeserializeObject(jsonData);
        quesitionQueue.Enqueue(jsonObj);
    }
    /// <summary>
    /// update player position
    /// if player is nil,create
    /// if player != nil,update pos
    /// </summary>
    /// <param name="data"></param>
    void ShowQuestion(Quesition data)
    {
        text.text = data.SDesc;
    }
}

public class Quesition
{
    public string STitle;
    public string SDesc;
    public string SDesc1;
    public string SDesc2;
    public string Desc3;
    public string SDesc4;
    public int IRightIdx;
}
