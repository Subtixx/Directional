using System;
using System.Drawing;
using System.Windows.Forms;

namespace Directional.Game
{
    public class Snowflake
    {
        private readonly int _fallSpeed = 3;
        private readonly int _snowflakeSize;
        private readonly Form _form;

        public Snowflake(Form form, Color color, int size, int fallSpeed)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));

            Button = new Button
            {
                Visible = false,
                Width = size,
                Height = size,
                BackColor = color,
                TabStop = false
            };

            _snowflakeSize = size;
        }

        public Button Button { get; }
        public int Bottom => Button.Bottom;

        public void Show(int left)
        {
            Button.Visible = true;
            Button.Top = 0;
            var formWidth = _form.Width;
            Button.Left = left;
            Button.Show();
            Button.BringToFront();
        }

        public void Fall()
        {
            Button.Top = Button.Top + _fallSpeed;
        }

        public void Dispose()
        {
            Button.Hide();
            Button.Dispose();
        }
    }
}