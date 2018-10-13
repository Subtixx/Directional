using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Snowflakes.Game.Models;

namespace Snowflakes.Game
{
    public partial class Form1 : Form, IDisposable
    {
        private const int Interval = 50;
        private const int NextLevelUpgrade = 5 * Interval;
        private const int StartingLives = 3;
        private int _level;
        private int _lives;
        private readonly Timer _mainTimer;
        private int _maxSpawnSpeed;

        private readonly Random _random;
        private int _score;
        private int _scorePerSnowflake;
        private readonly List<Snowflake> _snowflakes;
        private readonly int _snowflakeSize;
        private int _ticks;
        private int _timeToNextSpawn;

        public Form1()
        {
            InitializeComponent();
            _mainTimer = new Timer();
            _mainTimer.Tick += Tick;
            _mainTimer.Interval = Interval;
            _snowflakes = new List<Snowflake>();
            _timeToNextSpawn = 0;
            _maxSpawnSpeed = 35;
            _snowflakeSize = 50;
            _level = 1;
            _scorePerSnowflake = 100;
            _lives = StartingLives;

            _random = new Random();
        }

        private bool _alive => _lives > 0;

        public new void Dispose()
        {
            base.Dispose();
            _mainTimer.Tick -= Tick;
            _mainTimer.Dispose();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            _mainTimer.Start();
        }


        private void Form1_Click(object sender, EventArgs e)
        {
            LoseLife();
        }

        private void Tick(object sender, EventArgs args)
        {
            if (_ticks == int.MaxValue - 1)
            {
                _mainTimer.Stop();
                MessageBox.Show("YOU WIN!");
                return;
            }

            _ticks++;

            // TODO: fix this so the snowflakes get smaller by a equasion/algorithm
            if (_ticks % NextLevelUpgrade == 0)
            {
                _level++;
                _scorePerSnowflake = 100 * _level;
                _maxSpawnSpeed--; // TODO: should not be able to hit 0
            }

            _timeToNextSpawn--;

            if (_timeToNextSpawn <= 0)
            {
                _timeToNextSpawn = _random.Next(_maxSpawnSpeed);
                var left = _random.Next(Width - _snowflakeSize);
                var red = _random.Next(255);
                var green = _random.Next(255);
                var blue = _random.Next(255);
                var color = Color.FromArgb(red, green, blue);
                SpawnNewSnowflake(left, color, _maxSpawnSpeed, _maxSpawnSpeed);
            }

            var lifeLost = false;
            _snowflakes.ForEach(s =>
            {
                s.Fall();

                if (s.Bottom >= Height) // You lose
                {
                    LoseLife();
                    lifeLost = true;
                }
            });

            if (lifeLost) ClearSnowflakes();

            UpdateLabels();
        }

        private void ClearSnowflakes()
        {
            _snowflakes.ForEach(s => RemoveSnowflake(s));
            _snowflakes.Clear();
        }

        /// <summary>
        ///     Spawns new snowflake
        /// </summary>
        /// <param name="startLeftPosition">The starting position left aligned</param>
        /// <param name="size">The beginning size of the snowflake</param>
        /// <param name="fallSpeed">^^</param>
        private void SpawnNewSnowflake(int startLeftPosition, Color color, int size, int fallSpeed)
        {
            var snowflake = new Snowflake(this, color, size, fallSpeed);
            snowflake.Button.Click += (_, __) =>
            {
                RemoveSnowflake(snowflake);
                _snowflakes.Remove(snowflake);
                _score += _scorePerSnowflake;
            };
            _snowflakes.Add(snowflake);
            Controls.Add(snowflake.Button);

            snowflake.Show(startLeftPosition);
        }

        private void RemoveSnowflake(Snowflake snowflake)
        {
            snowflake.Dispose();
            Controls.Remove(snowflake.Button);
        }

        private void LoseLife()
        {
            // For indication clear screen T_T?
            _lives--;
            UpdateLabels();

            if (!_alive) GameOver();
        }

        private void UpdateLabels()
        {
            Invoke((MethodInvoker) delegate
            {
                lblLevel.Text = $"Level: {_level:D}";
                lblScore.Text = $"Score: {_score:D}";
                lblLives.Text = $"Lives: {_lives:D}";
            });
        }

        private void GameOver()
        {
            _mainTimer.Stop();
            MessageBox.Show(
                "Game Over\n" +
                $"Level {_level}\n" +
                $"Final Score {_score}");
            Close();
        }
    }
}