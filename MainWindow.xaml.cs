using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GoldMinerSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int mode;
        public MainWindow()
        {
            InitializeComponent();
            randomBtn.IsChecked = true;
        }

        private void startSimulation_Click(object sender, RoutedEventArgs e)
        {
            int res;
            if (int.TryParse(gridSizeTxt.Text, out res)) 
            {
                int gridSize = int.Parse(gridSizeTxt.Text);
                if((gridSize >= 8) && (gridSize <= 64))
                {
                    SimulationWin sw = new SimulationWin(gridSize, mode);
                    sw.Show();
                    this.Close();
                } else
                {
                    MessageBox.Show("There are invalid inputs.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                MessageBox.Show("There are invalid inputs.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if(randomBtn.IsChecked == true) mode = 1;
            else mode = 2;
        }
    }
}
