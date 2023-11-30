using System.Reflection;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        private Board msBoard;
        private Button[,] cellButtons;
        public Form1()
        {
            InitializeComponent();
            msBoard = new Board(9,9,10);
            InitializeUI();
        }
        private void InitializeUI()
        {
            const int buttonSize = 30;
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
                    button.Click += new EventHandler(Cell_click);
                    
                    Controls.Add(button);
                    cellButtons[i,j] = button;

                }
            }
        }
        private void Cell_click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string[] position = clickedButton.Name.Split('_');

            if (position.Length == 2 && int.TryParse(position[0], out int row) && int.TryParse(position[1], out int col)) 
            { 
                msBoard.ReveallCell(row, col);
                UpdateUI();
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
                    var cell = msBoard.cells[i,j];
                    var button = cellButtons[i, j];

                    button.Enabled = !cell.IsRevealed;

                    if (cell.IsRevealed)
                    {
                        button.Text = cell.GetDisplayValue();
                    }
                    else
                    {
                        button.Text = "";
                    }
                }
            }
        }
        
    }
}