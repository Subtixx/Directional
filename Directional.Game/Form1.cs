using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Directional.Game
{
    public partial class Form1 : Form, IDisposable
    {
        private const int Interval = 50;
        private const int NextLevelUpgrade = 5 * Interval;
        private const int StartingLives = 3;
        private const int HitAnimationTime = 5;

        // TODO: Figure out colors
        private readonly List<Color> _boxColors = new List<Color>
        {
            Color.Black,
            Color.Blue,
            Color.Red,
            Color.Aqua
        };

        private readonly Timer _mainTimer;

        private readonly Random _random;
        private readonly List<Box> _boxes;
        private readonly int _boxSize;

        private int _hitAnimationDelay;
        private int _level;
        private int _lives;
        private int _maxSpawnSpeed;
        private int _score;
        private int _scorePerBox;
        private int _ticks;
        private int _timeToNextSpawn;

        public Form1()
        {
            InitializeComponent();
            _mainTimer = new Timer();
            _mainTimer.Tick += Tick;
            _mainTimer.Interval = Interval;
            _boxes = new List<Box>();
            _timeToNextSpawn = 0;
            _maxSpawnSpeed = 35;
            _boxSize = 50;
            _level = 1;
            _scorePerBox = 100;
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

            if (_hitAnimationDelay > 0) _hitAnimationDelay--;

            if (_hitAnimationDelay <= 0 && BackColor != Color.White)
            {
                BackColor = Color.White;
                _hitAnimationDelay = 0;
            }

            _ticks++;

            // TODO: fix this so the boxes get smaller by a equasion/algorithm
            if (_ticks % NextLevelUpgrade == 0)
            {
                _level++;
                _scorePerBox = 100 * _level;
                _maxSpawnSpeed--; // TODO: should not be able to hit 0
            }

            _timeToNextSpawn--;

            if (_timeToNextSpawn <= 0)
            {
                _timeToNextSpawn = _random.Next(_maxSpawnSpeed);

                var movingDir = (Box.Direction) _random.Next(0, Enum.GetValues(typeof(Box.Direction)).Length+1);
                var left = movingDir == 0
                    ? _random.Next(Width - _boxSize)
                    : _random.Next(Height - _boxSize);
                var color = _boxColors[_random.Next(_boxColors.Count)];

                SpawnNewBox(left, color, _maxSpawnSpeed, _maxSpawnSpeed, movingDir);
            }

            var lifeLost = false;
            _boxes.ForEach(s =>
            {
                s.Fall();

                if (s.Bottom >= Height && s.MovingDirection == Box.Direction.Top) // You lose
                {
                    LoseLife();
                    lifeLost = true;
                }
                else if (s.Bottom <= 0 && s.MovingDirection == Box.Direction.Down) // You lose
                {
                    LoseLife();
                    lifeLost = true;
                }
                else if (s.Top >= Width && s.MovingDirection == Box.Direction.Left) // You lose
                {
                    LoseLife();
                    lifeLost = true;
                }
                else if (s.Top <= 0 && s.MovingDirection == Box.Direction.Right) // You lose
                {
                    LoseLife();
                    lifeLost = true;
                }
            });

            if (lifeLost) ClearBoxes();

            UpdateLabels();
        }

        private void ClearBoxes()
        {
            _boxes.ForEach(RemoveBox);
            _boxes.Clear();
        }

        /// <summary>
        ///     Spawns a new box
        /// </summary>
        /// <param name="startPosition">The starting position left aligned</param>
        /// <param name="color"></param>
        /// <param name="size">The beginning size of the box</param>
        /// <param name="fallSpeed">^^</param>
        /// <param name="movingDir"></param>
        private void SpawnNewBox(int startPosition, Color color, int size, int fallSpeed, Box.Direction movingDir)
        {
            var box = new Box(color, size, fallSpeed, movingDir);
            box.Button.Click += (_, __) =>
            {
                RemoveBox(box);
                _boxes.Remove(box);
                _score += _scorePerBox;
            };
            _boxes.Add(box);
            Controls.Add(box.Button);

            box.Show(this, startPosition);
        }

        private void RemoveBox(Box box)
        {
            box.Dispose();
            Controls.Remove(box.Button);
        }

        private void LoseLife()
        {
            _hitAnimationDelay = HitAnimationTime;
            BackColor = Color.Red;
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