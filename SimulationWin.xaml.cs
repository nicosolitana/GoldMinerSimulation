using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GoldMinerSimulation
{
    public enum Direction
    {
        Null = 0,
        North = 1,
        East = 2,
        West = 3,
        South = 4
    }

    public enum Type
    {
        Gold,
        Pit,
        Beacon,
        Empty,
        Miner,
        Captured
    }

    public class MineObjects
    {
        public Image img;
        public Type cellType;
        public Direction dir;
    }

    /// <summary>
    /// Interaction logic for SimulationWin.xaml
    /// </summary>
    public partial class SimulationWin : Window
    {
        public List<List<MineObjects>> mineObjs = new List<List<MineObjects>>();
        private int gridSize;
        private int mode;
        private int minerX;
        private int minerY;
        private int goldCol = 0;
        private int goldRow = 0;
        private bool goldIsNotFound = true;

        public SimulationWin(int _gridSize, int _mode)
        {
            gridSize = _gridSize;
            mode = _mode;
            InitializeComponent();
            InitializeGrid();
            InitializePitsGoldBeacon();
            InitializeMiner();
            StartSimulation();
        }

        private void InitializeGrid()
        {
            int cellSize;
            cellSize = SetCellSize();
            int locTop = 0, locLeft = 0;

            for (int i = 0; i < gridSize; i++)
            {
                List<Image> imgList = new List<Image>();
                List<MineObjects> mObjLst = new List<MineObjects>();
                for (int j = 0; j < gridSize; j++)
                {
                    MineObjects mObj = new MineObjects();
                    Image img = CreateImgControl(cellSize, locLeft, locTop);
                    mainGrid.Children.Add(img);
                    mObj.img = img;
                    mObj.cellType = Type.Empty;
                    mObj.dir = Direction.Null;
                    mObjLst.Add(mObj);
                    locLeft += cellSize;
                }
                mineObjs.Add(mObjLst);
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

                if ((mineObjs[xAxis][yAxis].cellType == Type.Pit)
                || ((i == pitCount - 1)
                    && ((xAxis == 0 || xAxis == gridSize - 1)
                        || (yAxis == 0 || yAxis == gridSize - 1))))
                {
                    pitCount++;
                }

                if (mineObjs[xAxis][yAxis].cellType == Type.Empty)
                {
                    if (i == pitCount - 1)
                    {
                        SetImageControl(xAxis, yAxis, 3, Type.Gold);
                        InitializeBeacons(xAxis, yAxis);
                    }
                    else
                    {
                        SetImageControl(xAxis, yAxis, 4, Type.Pit);
                    }
                }
            }
        }

        private void InitializeMiner()
        {
            minerX = 0;
            minerY = 0;
            SetImageControl(minerX, minerY, 8, Type.Miner);
            mineObjs[0][0].dir = Direction.East;
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
                SetImageControl(xVar, yVar, 6, Type.Beacon);
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

        private void SetImageControl(int xAxis, int yAxis, int iconIndex, Type cType)
        {
            mineObjs[xAxis][yAxis].img.Source = IconManipulations.ConvertIcon(IconManipulations.SelectIcon(iconIndex));
            mineObjs[xAxis][yAxis].cellType = cType;
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
            else Task.Factory.StartNew(() => SmartSimulation());
        }

        private void RandomSimulation()
        {

        }

        private void SmartSimulation()
        {
            while (goldIsNotFound)
            {
                if ((minerX > gridSize-1) || (minerY > gridSize-1))
                {
                    break;
                }

                if ((goldRow > 0) && (minerX > goldRow))
                {
                    MoveEast();
                }

                if ((ScanRow()) && (goldRow == 0))
                {
                    goldRow = minerY;
                    MoveEast();
                } else
                {
                    MoveSouthOrEast();
                }

            }

            if (goldIsNotFound)
                MessageBox.Show("Gold has not been found.");
            else
                MessageBox.Show("Gold has been found.");
        }

        private bool ScanRow()
        {
            if (mineObjs[minerY].Where(x => x.cellType == Type.Gold).Count() > 0)
                return true;
            return false;
        }

        private bool ScanColumn()
        {
            for (int i = minerY + 1; i < gridSize; i++)
            {
                if (mineObjs[i][minerX].cellType == Type.Gold)
                    return true;
            }
            return false;
        }

        private Type ScanFront(int yAxis, int xAxis)
        {
            switch (mineObjs[yAxis][xAxis].cellType)
            {
                case Type.Pit: return Type.Pit;
                case Type.Gold: return Type.Gold;
                default: return Type.Empty;
            }
        }

        //private Direction ScanAllDirections(int currY, int currX)
        //{
        //    Direction nextDir = Direction.Null;

        //    if (ScanFront(currY - 1, currX) == Type.Empty)
        //        nextDir = Direction.North;
        //    else if (ScanFront(currY + 1, currX) == Type.Empty)
        //        nextDir = Direction.South;
        //    else if (ScanFront(currY, currX - 1) == Type.Empty)
        //        nextDir = Direction.West;
        //    else
        //        nextDir = Direction.East;
        //    return nextDir;
        //}

        private void MoveSouthOrEast()
        {
            //MoveSouth
            if (ScanFront(minerY + 1, minerX) != Type.Pit)
            {
                minerY++;
                MoveOneStep(minerY - 1, minerX, Direction.South);
            }
            else
            {
                //MoveEast
                if (ScanFront(minerY, minerX + 1) != Type.Pit) 
                {
                    minerX++;
                    MoveOneStep(minerY, minerX - 1, Direction.East);
                    if (ScanColumn())
                    {
                        goldCol = minerX;
                        MoveSouth();
                    }
                    if(goldIsNotFound)
                        MoveSouthOrEast();
                }
            }
        }

        private void MoveSouth()
        {
            int init = minerY;
            for (int i = init; i < gridSize; i++)
            {
                minerY++;
                if (ScanFront(minerY, minerX) == Type.Gold)
                {
                    UpdateWinnerCell(minerY-1, minerX);
                    goldIsNotFound = false;
                    break;
                }
                UpdateGrid(i, minerX, Direction.South);
            }
        }

        private void MoveEast()
        {
            int breakFlg = 0;
            int init = minerX;
            for (int i = init; i < gridSize; i++)
            {
                minerX++;
                if (ScanFront(minerY, minerX) == Type.Gold)
                {

                    UpdateWinnerCell(minerY, minerX-1);
                    goldIsNotFound = false;
                    break;
                }
                if (ScanFront(minerY, minerX) == Type.Pit)
                {
                    minerX--;
                    MoveSouthOrEast();
                    breakFlg = 1;
                    break;
                }
                UpdateGrid(minerY, i, Direction.East);

                if (goldRow < minerY)
                {
                    if((ScanFront(minerY-1, minerX) != Type.Pit)
                        && (ScanFront(minerY - 1, minerX) != Type.Gold))
                    {
                        minerY--;
                        MoveOneStep(minerY + 1, minerX, Direction.North);
                    }

                    if (ScanFront(minerY - 1, minerX) == Type.Gold)
                    {
                        minerY++;  // [DEBUG] init minerY-- 
                        //UpdateGrid(minerY, i, Direction.East);
                        UpdateWinnerCell(minerY-2, minerX);
                        goldIsNotFound = false;
                        breakFlg = 0;
                        break;
                    }
                }
            }

            if (breakFlg != 0)
            {
                MoveEast();
            }
        }

        private void MoveOneStep(int emptyY, int emptyX, Direction mineDir)
        {
            UpdateGrid(emptyY, emptyX, mineDir);
        }

        private void UpdateGrid(int emptyY, int emptyX, Direction nextDir)
        {
            int minerIconIndex = IconManipulations.GetMinerIcon(nextDir);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SetImageControl(emptyY, emptyX, 30, Type.Empty);
                SetImageControl(minerY, minerX, minerIconIndex, Type.Miner); //Change miner
            }), DispatcherPriority.Background);
            Thread.Sleep(1000);
        }

        private void UpdateWinnerCell(int emptyY, int emptyX)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SetImageControl(emptyY, emptyX, 30, Type.Empty);
                SetImageControl(minerY, minerX, 11, Type.Captured); //Change miner
            }), DispatcherPriority.Background);
            Thread.Sleep(1000);
        }
    }
}
