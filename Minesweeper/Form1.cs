using System.Data;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        private Board msBoard;
        private Button[,] cellButtons;
        private int _ticks;
        private int mins;
        public Label lblTimer;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(717, 373);
        }
        private void InitializeUI()
        {
            const int buttonSize = 40;
            cellButtons = new Button[msBoard.Rows, msBoard.Cols];
            lblTimer = new Label();

            for (int i = 0; i < msBoard.Rows; i++)
            {
                for (int j = 0; j < msBoard.Cols; j++)
                {
                    var button = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(j * buttonSize, i * buttonSize),
                        Name = $"{i}_{j}",
                    };

                    button.MouseDown += Cell_click;

                    Controls.Add(button);
                    cellButtons[i, j] = button;

                }
            }
        }
        private void Cell_click(object sender, EventArgs e)
        {
            MouseEventArgs f = e as MouseEventArgs;

            Button clickedButton = (Button)sender;

            string[] position = clickedButton.Name.Split('_');

            if (position.Length == 2 && int.TryParse(position[0], out int row) && int.TryParse(position[1], out int col))
            {
                Cell cell = msBoard.GetCell(row, col);

                if (f.Button != MouseButtons.Right)
                {
                    if (f.Button == MouseButtons.Left && !cell.IsMarked)
                    {
                        msBoard.ReveallCell(row, col);
                        if (cell.IsMine)
                        {
                            msBoard.IsGameOver = true;
                        }
                    }
                    else if (f.Button == MouseButtons.Middle && !cell.IsMarked && cell.IsRevealed)
                    {
                        int MineCount = msBoard.CountMarkedCellsAround(row, col);
                        int DisplayValue = Int32.Parse(cell.GetDisplayValue());

                        if (!(msBoard.IsGameOver == true) && MineCount == DisplayValue)
                        {
                            RevealUnmarkedCellsAround(row, col);
                        }

                    }
                    else if (f.Button == MouseButtons.XButton2) 
                    {
                        msBoard.ToggleUnknownCell(row, col);
                    }
                }
                else
                {
                    msBoard.ToggleMarkedCell(row, col);
                }

                UpdateUI();

                if (msBoard.CheckForWin())
                {
                    timer1.Stop();
                    MessageBox.Show($"Congratulations! You won the game in {mins} mins and {_ticks} seconds!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Dispose();
                    Close();
                }
                else if (msBoard.IsGameOver)
                {
                    timer1.Stop();
                    timer1.Dispose();
                    MessageBox.Show("You hit a mine! Better luck next time", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }

            }
            else
            {
                MessageBox.Show("Error: Button position information is not set correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void UpdateUI()
        {

            for (int i = 0; i < msBoard.Rows; i++)
            {
                for (int j = 0; j < msBoard.Cols; j++)
                {
                    Button button = cellButtons[i, j];
                    Cell cell = msBoard.GetCell(i, j);

                    if (cell.IsRevealed)
                    {
                        button.Text = cell.GetDisplayValue();
                        button.BackColor = Color.LightGray;

                    }
                    else if (cell.IsMarked)
                    {
                        button.BackColor = Color.ForestGreen;
                    }
                    else if (cell.IsUnkown)
                    {
                        button.BackColor = Color.Blue;
                    }
                    else
                    {
                        button.Text = "";
                        button.BackColor = SystemColors.Control;
                    }
                }
            }
            lblTimer.Text = $"Time elapsed: " + _ticks.ToString() + " seconds";

        }
        private void RevealUnmarkedCellsAround(int centerRow, int centerCol)
        {
            for (int i = Math.Max(0, centerRow - 1);i <= Math.Min(msBoard.Rows - 1, centerRow + 1); i++)
            {
                for (int j = Math.Max(0, centerCol -1); j<= Math.Min(msBoard.Cols - 1 , centerCol + 1); j++)
                {
                    Cell cell = msBoard.GetCell(i, j);

                    if (!cell.IsMarked && !cell.IsRevealed)
                    {
                        msBoard.ReveallCell(i,j);
                    }
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ClearForm()
        {
            foreach (Control control in Controls)
            {
                control.Dispose();
            }

            Controls.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearForm();
            this.Size = new Size(380, 410);
            msBoard = new Board(9, 9, 10);
            InitializeUI();
            timer1.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm();
            this.Size = new Size(660, 700);
            msBoard = new Board(16, 16, 40);
            InitializeUI();
            timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearForm();
            this.Size = new Size(1230, 700);
            msBoard = new Board(16, 30, 99);
            InitializeUI();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _ticks++;
            mins = _ticks % 60;
            
        }
    }
}