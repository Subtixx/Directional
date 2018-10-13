using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snowflakes.Game.Models
{
    public class Snowflake
    {
        private readonly int _fallSpeed = 3;
        private readonly int _snowflakeSize;
        private Form _form;

        public Button Button { get; }
        public int Bottom => Button.Bottom;

        public Snowflake(Form form, Color color, int size, int fallSpeed)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));

            Button = new Button()
            {
                Visible = false,
                Width = size,
                Height = size,
                BackColor = color,
                TabStop = false
            };

            _snowflakeSize = size;
        }

        public void Show(int left)
        {
            Button.Visible = true;
            Button.Top = 0;
            int formWidth = _form.Width;
            Button.Left = left;
            Button.Show();
            Button.BringToFront();
        }

        public void Fall() =>
            Button.Top = Button.Top + _fallSpeed;

        public void Dispose()
        {
            Button.Hide();
            Button.Dispose();
        }
    }
}
