using System;
using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
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

        public override void OnWorldLoaded(World world)
        {
            foreach (var player in Players.Values)
            {
                world.Add(player);

            }
            base.OnWorldLoaded(world);
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
            
            
            // TODO
            // Rotation
            if (Changes.Count > 0)
            {
                Message? message = (Message)Changes.Pop()!;
                if (message.UserID != Id)
                {
                    Console.WriteLine(message.ToString());

                    if (Players.ContainsKey(message.UserID))
                    {
                        // Update existing Climber
                        long longValue = long.Parse(message.PacketNum);
                        if (Player2Packet[message.UserID] <= longValue)
                        {
                            if (message.CurrentMap == currentMap)
                            {
                                var climber = Players[message.UserID];
                                climber.Position = message.PosVec;
                                Players[message.UserID] = climber;
                                Player2Packet[message.UserID] = longValue;
                            }
                        }
                    }
                    else
                    {
                        // Add new Climber
                        if (message.CurrentMap == currentMap)
                        {
                            var newClimber = new Climbers
                            {
                                Position = message.PosVec
                            };
                            var addedClimber = World?.Add(newClimber);
                            if (addedClimber != null)
                            {
                                Players[message.UserID] = addedClimber;
                                long longValue = long.Parse(message.PacketNum);
                                Player2Packet[message.UserID] = longValue;
                            }
                        }
                    }
                }
            }
            
		}
    }
}
