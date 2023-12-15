using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        private Board msBoard;
        private Button[,] cellButtons;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(717, 373);  
        }
        private void InitializeUI()
        {
            const int buttonSize = 40;
            cellButtons = new Button[msBoard.Rows, msBoard.Cols];

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
                    }
                }
                else
                {
                    msBoard.ToggleMarkedCell(row, col);
                }

                UpdateUI();

                if (msBoard.CheckForWin())
                {
                    MessageBox.Show("Congratulations! You won the game!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else if (msBoard.IsGameOver)
                {
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
                    else
                    {
                        button.Text = "";
                        button.BackColor = SystemColors.Control;
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

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm();
            this.Size = new Size(660,700);
            msBoard = new Board(16, 16, 40);
            InitializeUI();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearForm();
            this.Size = new Size(1230,700);
            msBoard = new Board(16, 30, 99);
            InitializeUI();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}