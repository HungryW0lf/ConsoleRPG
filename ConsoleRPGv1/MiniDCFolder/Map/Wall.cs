﻿namespace ConsoleRPG
{
    public class Wall : BaseObject
    {
        Renderer renderer;
        Position position;
        Collider collider;
        public Wall(Map map, int x, int y) : base(map)
        {
            collider = new Collider(this, OnCollision);
            position = new Position(this, x, y);
            renderer = new Renderer(this, '#', 5, new Color(Color.Colors.White), false);
        }

        bool OnCollision(BaseObject baseObject)
        {
            return true;
        }
    }
}
