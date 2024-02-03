using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Celeste64;

namespace InfiniteJumpMod;


[JsonSerializable(typeof(Message))]
internal partial class MessageContext : JsonSerializerContext { }

public class Message
{
    public Vector3 PosVec { get; set; }
    public string UserID { get; set; }
    [JsonPropertyName("Position")]
    public string Position { get; set; }
    public string CurrentMap { get; set; }
    public string PacketNum { get; set; }
    public string PosX { get; set; }
    public string PosY { get; set; }
    public string PosZ { get; set; }

    public Message(string userID, string currentMap, string packetNum, string posX, string posY, string posZ)
    {
        UserID = userID;
        CurrentMap = currentMap;
        PacketNum = packetNum;
        PosX = posX;
        PosY = posY;
        PosZ = posZ;
    }

    public override string ToString()
    {
        // return $@"{{ ""userID"": ""{UserID}"",""position"": ""{Position}"",""currentMap"": ""{CurrentMap}"",""packetNum"": ""{PacketNum}"" }}";
        return  JsonSerializer.Serialize(this, MessageContext.Default.Message);
    }


    public void PositionVec()
    {
        Console.WriteLine(this.ToString());
        // var s = Position.Substring(1, Position.Length - 2);
        // string[] parts = s.Split(new string[] { "," }, StringSplitOptions.None);
        // var temp = new Vector3(
        //     float.Parse(parts[0]),
        //     float.Parse(parts[1]),
        //     float.Parse(parts[2]));
        var temp = new Vector3(float.Parse(PosX, CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(PosY, CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(PosZ, CultureInfo.InvariantCulture.NumberFormat));

        this.PosVec = temp;
    }
}