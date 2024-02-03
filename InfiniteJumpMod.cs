using System;
using System.Collections;
using System.Numerics;
using Celeste64;
namespace InfiniteJumpMod
{
    public class InfiniteJumpMod : GameMod
    {
        private Player? _player;
        public Vector3 lastPos = Vector3.Zero;
        public Int64 packetNum = 0;
        public Stack Changes = new Stack();
        public String Id = Guid.NewGuid().ToString();
        public string? currentMap = "";
        public Dictionary<String, Actor> Players = new Dictionary<string, Actor>();
        public Dictionary<String,long> Player2Packet = new Dictionary<string, Int64>();
        
        
        public override void OnActorAdded(Actor actor)
        {
            base.OnActorAdded(actor);
            if(actor is Player player)
            {
                _player = player;
            }
        }

        public override void OnModLoaded()
        {
            Console.WriteLine("STARTED");
            ClientHandler.ClientStart("141.148.63.115", 25566,ref Changes);
        }

        public override void Update(float deltaTime)
        {
            currentMap = Map?.Name;
            base.Update(deltaTime);
            if (_player?.Position != lastPos)
            {
                if (currentMap != null)
                {
                    Message message = new Message(Id, currentMap, packetNum.ToString(), _player?.Position.X.ToString(),_player?.Position.Y.ToString(),_player?.Position.Z.ToString());
                    ClientHandler.SendToServer(message);
                }

                if (_player != null) lastPos = _player.Position;
                packetNum += 1;
            }
            
            
            
            if (Changes.Count > 0)
            {
                Message? message = (Message)Changes.Pop()!;
                var player = _player;
                var validMove = true;
                if (player != null && message.UserID != Id)
                {
                    Console.WriteLine(message.ToString());
                    var climber = new Climbers();
                    if (Players.ContainsKey(message.UserID))
                    {
                        if (Player2Packet.ContainsKey(message.UserID))
                        {
                            long longValue = long.Parse(message.PacketNum);

                            if (Player2Packet[message.UserID] > longValue)
                            {
                                validMove = false;
                            }
                            else
                            {
                                World?.Destroy(Players[message.UserID]);
                                Players.Remove(message.UserID);
                            }
                        }
                    }

                    if (message.CurrentMap == currentMap && validMove)
                    {
                        climber.Position = message.PosVec;
                        Players[message.UserID] = climber;
                        World?.Add(climber);
                        long longValue = long.Parse(message.PacketNum);

                        Player2Packet[message.UserID] = longValue;
                    }
                }
            }
            
            
            
		}
    }
}
