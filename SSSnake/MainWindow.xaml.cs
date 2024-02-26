using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Timers;
using System.ComponentModel;
using SSSnake;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SSSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        
        Timer timer = new Timer();

        public event PropertyChangedEventHandler PropertyChanged;

        private string test;
        DispatcherTimer foodTimer;
        DispatcherTimer snakeTimer;
        private bool initiateFoodCleanup;
        private int score;
        public string Score {
            get
            {
                return $"Score {score}";
            }
        }

        private int[] foodLoaction;
        private Boolean gameOn;
        private List<ObservableCollection<GridCell>> playingField;
        public List<ObservableCollection<GridCell>> PlayingField { get { return playingField; } set { playingField = value; } }

        List<Snake> Basket;

        public string Test
        {
            get { return test; }
            set { test = value;
                //NotifyPropertyChanged("Test");
            }
        }

       
        public MainWindow()
        {
            Test = "Hello";
            gameOn = false;
            initiateFoodCleanup = true;

            foodLoaction = new int[2];
            InitializeComponent();
            GridCell[,] grid = new GridCell[20,20];
            Basket = new List<Snake>();

            for (int column = 0; column < 20; column++)
            {
                for (int row = 0; row < 20; row++)
                {
                    grid[column, row] = new GridCell(Enums.CellType.Empty, row, column );
                }
            }
            PlayingField = ConvertToList(grid);

            Snake SnakeA = new Snake(10, 8);
            SnakeA.ArrowControlled = true;

            Basket.Add(SnakeA);
           
            UpdatePlayingField();

            foodTimer = new DispatcherTimer();
            foodTimer.Interval = TimeSpan.FromSeconds(5); 
            foodTimer.Tick += FoodTimer_Tick;

            snakeTimer = new DispatcherTimer();
            snakeTimer.Interval = TimeSpan.FromSeconds(1);
            snakeTimer.Tick += SnakeTimer_Tick;
            
            DataContext = this;
        }


        private void SnakeTimer_Tick(object sender, EventArgs e)
        {
            if (sender != null)
                ((DispatcherTimer)sender).Stop();

            Console.WriteLine(DateTime.Now.Second.ToString());
            AutomateSnakes();
            if (sender != null)
                ((DispatcherTimer)sender).Start();
        }

        private void FoodTimer_Tick(object sender, EventArgs e)
        {
            if (sender != null)
                ((DispatcherTimer)sender).Stop();

            SpawnFood();
            if (sender != null)
                ((DispatcherTimer)sender).Start();
        }
        
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdatePlayingField()
        {
            if (playingField[foodLoaction[0]][foodLoaction[1]].CellType == Enums.CellType.Empty)
                FoodTimer_Tick(foodTimer, null);

            foreach (Snake snake in Basket)
            {
                if (SnakeAte(snake))
                {
                    snakeTimer.Stop();
                    UpdatePlayingField();
                }
                
                foreach (GridCell limb in snake.SnakeBody)
                {
                    PrepForWarp(limb);

                    if (limb.CellType == Enums.CellType.Head && collision(limb))
                    {
                        snake.Alive = false;
                    }
                    PlayingField[limb.Row][limb.Column] = limb;
                }

                NotifyPropertyChanged("PlayingField");

                if (snakeTimer != null)
                {
                    if (!snakeTimer.IsEnabled)
                        snakeTimer.Start();
                    Console.WriteLine($"snake interval: {snakeTimer.Interval} Time: {DateTime.Now.Millisecond}");
                }

            }
        }

        private  void PrepForWarp(GridCell limb)
        {
            if(limb.Row >= PlayingField.Count)
                limb.Row = (limb.Row % PlayingField.Count);
             else if( limb.Row < 0)
                limb.Row += PlayingField.Count;

            if (limb.Column >= PlayingField[0].Count)
                limb.Column = (limb.Column % PlayingField[0].Count);
            else if (limb.Column < 0)
                limb.Column += PlayingField[0].Count;
        }

        private bool collision(GridCell head)
        {
            var inspectionCell = PlayingField[head.Row][head.Column];

            if (inspectionCell.CellType == Enums.CellType.Body)
            {
                return true;
                //call game over functionality
            }
            return false;
        }

        private Boolean SnakeAte(Snake snake)
        {
            var head = snake.SnakeBody.Last();
            if(PlayingField.ElementAtOrDefault(head.Row) != null)
            {
                if (PlayingField[0].ElementAtOrDefault(head.Column) != null)
                {
                    if (PlayingField[head.Row][head.Column].CellType == Enums.CellType.Food)
                    {
                        snake.Grow();
                        initiateFoodCleanup = false;
                        score += 50;
                        NotifyPropertyChanged("Score");

                        if (snakeTimer.Interval > TimeSpan.FromSeconds(0.1))
                            snakeTimer.Interval -= TimeSpan.FromMilliseconds(50);

                        return true;
                    }
                }
            }
           
            return false;
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (Snake snake in Basket)
            {
                if (snake.ArrowControlled)
                    ArrowController(snake, e);
            }
        }

        private void ArrowController(Snake snake, KeyEventArgs e)
        {
            if (e.Key == Key.Left && snake.Direction != Enums.Direction.Right)
            {
                var cleanupIndex = snake.MoveLeft();
                PlayingField[cleanupIndex[0]][cleanupIndex[1]] = new GridCell(Enums.CellType.Empty, cleanupIndex[1], cleanupIndex[0]);
                UpdatePlayingField();
            }
            else if (e.Key == Key.Right && snake.Direction != Enums.Direction.Left)
            {
                var cleanupIndex = snake.MoveRight();
                PlayingField[cleanupIndex[0]][cleanupIndex[1]] = new GridCell(Enums.CellType.Empty, cleanupIndex[1], cleanupIndex[0]);
                UpdatePlayingField();
            }
            else if (e.Key == Key.Down && snake.Direction != Enums.Direction.Up)
            {
                var cleanupIndex = snake.MoveDown();
                PlayingField[cleanupIndex[0]][cleanupIndex[1]] = new GridCell(Enums.CellType.Empty, cleanupIndex[1], cleanupIndex[0]);
                UpdatePlayingField();
            }
            else if (e.Key == Key.Up && snake.Direction != Enums.Direction.Down)
            {
                var cleanupIndex = snake.MoveUp();
                PlayingField[cleanupIndex[0]][cleanupIndex[1]] = new GridCell(Enums.CellType.Empty, cleanupIndex[1], cleanupIndex[0]);
                UpdatePlayingField();
            }

            if (!gameOn)
            {
                foodTimer.Start();
                snakeTimer.Start();
                gameOn = true;
            }
        }
        private async void loaded_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void AutomateSnakes()
        {
            foreach (Snake snake in Basket)
            {
                if(snake.ArrowControlled) 
                    AutomateMovement(snake);
            }
        }

        private void SpawnFood()
        {
            Random random = new Random();
            int randomColumn;
            int randomRow;

            do
            {
                randomColumn = random.Next(0, 19);
                randomRow = random.Next(0, 19);
            } while (PlayingField[randomRow][randomColumn].CellType != Enums.CellType.Empty);

            if(initiateFoodCleanup)
                PlayingField[foodLoaction[0]][foodLoaction[1]] = new GridCell(Enums.CellType.Empty, foodLoaction[0], foodLoaction[1]);

            PlayingField[randomRow][randomColumn] = new GridCell(Enums.CellType.Food, randomRow, randomColumn);
            NotifyPropertyChanged("PlayingField");
            foodLoaction[0] = randomRow;
            foodLoaction[1] = randomColumn;
            initiateFoodCleanup = true;
        }

        private void AutomateMovement(Snake snake)
        {
            if (!snake.Alive)
                return;

            if (snake.Direction == Enums.Direction.Left)
            {
                var cleanupIndex = snake.MoveLeft();
                PlayingField[cleanupIndex[0]][cleanupIndex[1]] = new GridCell(Enums.CellType.Empty, cleanupIndex[1], cleanupIndex[0]);
                UpdatePlayingField();
            }
            else if (snake.Direction == Enums.Direction.Right)
            {
                var cleanupIndex = snake.MoveRight();
                PlayingField[cleanupIndex[0]][cleanupIndex[1]] = new GridCell(Enums.CellType.Empty, cleanupIndex[1], cleanupIndex[0]);
                UpdatePlayingField();
            }
            else if (snake.Direction == Enums.Direction.Down)
            {
                var cleanupIndex = snake.MoveDown();
                PlayingField[cleanupIndex[0]][cleanupIndex[1]] = new GridCell(Enums.CellType.Empty, cleanupIndex[1], cleanupIndex[0]);
                UpdatePlayingField();
            }
            else if (snake.Direction == Enums.Direction.Up)
            {
                var cleanupIndex = snake.MoveUp();
                PlayingField[cleanupIndex[0]][cleanupIndex[1]] = new GridCell(Enums.CellType.Empty, cleanupIndex[1], cleanupIndex[0]);
                UpdatePlayingField();
            }
        }

        private List<ObservableCollection<GridCell>> ConvertToList(GridCell[,] data)
        {
            List<ObservableCollection<GridCell>> items = new List<ObservableCollection<GridCell>>();

            for (int i = 0; i < data.GetLength(0); i++)
            {
                ObservableCollection<GridCell> row = new ObservableCollection<GridCell>();
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    row.Add(new GridCell(data[i, j].CellType, data[i, j].Row, data[i, j].Column));
                }
                items.Add(row);
            }

            return items;
        }
    }
}
