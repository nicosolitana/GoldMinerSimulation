using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GoldMinerSimulation
{
    /// <summary>
    /// Interaction logic for SimulationWin.xaml
    /// </summary>
    public partial class SimulationWin : Window
    {
        public List<List<Image>> imgMainList = new List<List<Image>>();
        private int gridSize;
        private int mode;
        private int goldLocated = 0;

        public SimulationWin(int _gridSize, int _mode)
        {
            gridSize = _gridSize;
            mode = _mode;
            InitializeComponent();
            InitializeGrid();
            InitializePitsGoldBeacon();
            InitializeMiner();
        }

        private void InitializeGrid()
        {
            int cellSize;
            cellSize = SetCellSize();
            int locTop = 0, locLeft = 0;

            for (int i = 0; i < gridSize; i++)
            {
                List<Image> imgList = new List<Image>();
                for (int j = 0; j < gridSize; j++)
                {
                    Image img = CreateImgControl(cellSize, locLeft, locTop);
                    mainGrid.Children.Add(img);
                    imgList.Add(img);
                    locLeft += cellSize;
                }
                imgMainList.Add(imgList);
                locLeft = 0;
                locTop += cellSize;
            }
        }

        private void InitializePitsGoldBeacon()
        {
            Random rnd = new Random();
            int pitCount = rnd.Next(gridSize / 2, gridSize) + 1;

            for (int i = 0; i < pitCount; i++)
            {
                int xAxis = rnd.Next(gridSize);
                int yAxis = rnd.Next(gridSize);

                if ((imgMainList[xAxis][yAxis].Tag.ToString() == "P")
                    || ((i == pitCount - 1)
                    && ((xAxis == 0 || xAxis == gridSize - 1)
                        || (yAxis == 0 || yAxis == gridSize - 1))))
                {
                    pitCount++;
                }

                if (imgMainList[xAxis][yAxis].Tag.ToString() == "N")
                {
                    if (i == pitCount - 1)
                    {
                        SetImageControl(xAxis, yAxis, 3, "G");
                        InitializeBeacons(xAxis, yAxis);
                        //pitCount = SetGoldAndBeaconLocation(xAxis, yAxis, pitCount);
                    }
                    else
                    {
                        SetImageControl(xAxis, yAxis, 4, "P");
                    }
                }
            }
        }

        private void InitializeMiner()
        {
            SetImageControl(0, 0, 5, "M");
        }

        private void InitializeBeacons(int xAxis, int yAxis)
        {
            Random rnd = new Random();
            int xVar = 0, yVar = 0;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0: xVar = rnd.Next(0, xAxis); ; yVar = yAxis; break;
                    case 1: xVar = rnd.Next(xAxis + 1, gridSize); ; yVar = yAxis; break;
                    case 2: xVar = xAxis; yVar = rnd.Next(0, yAxis); break;
                    case 3: xVar = xAxis; yVar = rnd.Next(yAxis + 1, gridSize); break;
                }
                SetImageControl(xVar, yVar, 6, "B");
            }
        }

        private Image CreateImgControl(int cellSize, int locLeft, int locTop)
        {
            Random rnd = new Random();
            int index = rnd.Next(3);
            Image img = new Image
            {
                Height = cellSize,
                Width = cellSize,
                Margin = new Thickness(locLeft, locTop, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Source = IconManipulations.ConvertIcon(IconManipulations.SelectIcon(index)),
                Tag = "N"
            };
            return img;
        }

        private int SetGoldAndBeaconLocation(int xAxis, int yAxis, int pitCount)
        {
            if ((xAxis == 0 || xAxis == gridSize - 1) || (yAxis == 0 || yAxis == gridSize - 1))
            {
                pitCount++;
            }
            else
            {
                SetImageControl(xAxis, yAxis, 3, "G");
                InitializeBeacons(xAxis, yAxis);
            }
            return pitCount;
        }

        private void SetImageControl(int xAxis, int yAxis, int iconIndex, string tagVal)
        {
            imgMainList[xAxis][yAxis].Source = IconManipulations.ConvertIcon(IconManipulations.SelectIcon(iconIndex));
            imgMainList[xAxis][yAxis].Tag = tagVal;
        }

        private int SetCellSize()
        {
            int cellSize;
            if (gridSize < 10) cellSize = 50;
            else if (gridSize <= 20) cellSize = 33;
            else if (gridSize <= 35) cellSize = 20;
            else if (gridSize < 45) cellSize = 17;
            else cellSize = 10;
            return cellSize;
        }

        private void StartSimulation()
        {
            if (mode == 1) RandomSimulation();
            else SmartSimulation();
        }

        private void RandomSimulation()
        {

        }

        private void SmartSimulation()
        {

        }
    }
}
