using System;
using StarX;
using System.Text;

public class NetworkManager
{
    static StarXClient client = new StarXClient();
    public static StarXClient StarXService
    {
        get
        {
            return client;
        }
    }

    public static void StartConnect(Action callback)
    {
        client.Init("127.0.0.1", 23456, () =>
        {
            client.Connect((data) =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
        });
    }

    public static void EnterWorld(Action<byte[]> callback)
    {
        //随机一个名字=--
        PlayerLoginInfo data = new PlayerLoginInfo();
        data.name = "怕你不是两三天" + GetTimeStamp();
        string jsonData = SimpleJson.SimpleJson.SerializeObject(data);
        client.Request("Room.Enter", Encoding.UTF8.GetBytes(jsonData), callback);
    }
    public static long GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }

}


public class PlayerLoginInfo
{
    public string name;
}
