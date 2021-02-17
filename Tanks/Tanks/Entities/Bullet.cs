using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    public class Bullet : Entity
    {
        public int Speed;
        public Direction direction;

        public Bullet(Point pos, Direction dir, int speed) : base(pos)
        {
            Image = new Bitmap(@"Images\Bullet.png");
            Speed = 2 * speed;
            direction = dir;
			Dimension = new Size(6, 6);
			Position = new Point(pos.X + 17, pos.Y + 17);
			Name = "Bullet";
        }

		public void Move()
		{
			if (direction == Direction.LEFT)
			{
				Position.X -= Speed;
			}
			if (direction == Direction.RIGHT)
			{
				Position.X += Speed;
			}
			if (direction == Direction.UP)
			{
				Position.Y -= Speed;
			}
			if (direction == Direction.DOWN)
			{
				Position.Y += Speed;
			}
		}

		public void Render(Graphics g)
		{
			g.DrawImage(Image, new Point(Position.X, Position.Y));
		}
	}
}
