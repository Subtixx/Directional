using System.Drawing;
using System.Windows.Forms;

namespace Directional.Game
{
    public class Box
    {
        public enum Direction
        {
            Top,
            Left,
            Right,
            Down,
            Max // Last element.
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
            switch (MovingDirection)
            {
                case Direction.Top:
                default:
                    Button.Top = 0;
                    Button.Left = left;
                    break;
                case Direction.Down:
                    Button.Top = form.Height;
                    Button.Left = left;
                    break;
                case Direction.Right:
                    Button.Left = form.Width;
                    Button.Top = left;
                    break;
                case Direction.Left:
                    Button.Top = left;
                    Button.Left = 0;
                    break;
            }

            Button.Show();
            Button.BringToFront();
        }

        public void Fall()
        {
            switch (MovingDirection)
            {
                default:
                case Direction.Top:
                    Button.Top = Button.Top + _fallSpeed;
                    break;
                case Direction.Down:
                    Button.Top = Button.Top - _fallSpeed;
                    break;
                case Direction.Right:
                    Button.Left = Button.Left - _fallSpeed;
                    break;
                case Direction.Left:
                    Button.Left = Button.Left + _fallSpeed;
                    break;
            }
        }

        public void Dispose()
        {
            Button.Hide();
            Button.Dispose();
        }
    }
}