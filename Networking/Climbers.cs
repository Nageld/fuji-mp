using System.Numerics;
using Celeste64;

namespace InfiniteJumpMod;

public class Climbers : NPC
{
    public const string TALK_FLAG = "GRANNY";

    public Climbers() : base(Assets.Models["granny"])
    {
        Model.Transform = Matrix4x4.CreateScale(3) * Matrix4x4.CreateTranslation(0, 0, -1.5f);

    }

    public override void Interact(Player player)
    {
    }

}