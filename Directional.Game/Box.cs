using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Directional.Game
{
    public class Box
    {
        public enum Direction
        {
            Top,
            Left,
            Right,
            Down
        }
        
        private readonly int _fallSpeed = 3;
        public readonly Direction MovingDirection;

        public Box(Color color, int size, int fallSpeed, Direction movingDirection = Direction.Top)
        {
            MovingDirection = movingDirection;
            Button = new Button
            {
                Visible = false,
                Width = size,
                Height = size,
                BackColor = color,
                TabStop = false
            };
        }

        public Button Button { get; }
        public int Bottom => Button.Bottom;
        public int Top => Button.Top;

        public void Show(Form form, int left)
        {
            Button.Visible = true;
            if (MovingDirection == Direction.Top)
            {
                Button.Top = 0;
                Button.Left = left;
            }
            else if (MovingDirection == Direction.Down)
            {
                Button.Top = form.Height;
                Button.Left = left;
            }
            else if(MovingDirection == Direction.Right)
            {
                Button.Left = form.Width;
                Button.Top = left;
            }
            else
            {
                Button.Top = left;
                Button.Left = 0;
            }

            Button.Show();
            Button.BringToFront();
        }

        public void Fall()
        {
            if(MovingDirection == Direction.Top)
                Button.Top = Button.Top + _fallSpeed;
            else if(MovingDirection == Direction.Down)
                Button.Top = Button.Top - _fallSpeed;
            else if(MovingDirection == Direction.Right)
                Button.Left = Button.Left - _fallSpeed;
            else
                Button.Left = Button.Left + _fallSpeed;
        }

        public void Dispose()
        {
            Button.Hide();
            Button.Dispose();
        }
    }
}