using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSSnake
{
    public class Snake
    {
        public Enums.Direction Direction { get; set; }
        private bool alive;

        public bool Alive {
            get
            {
                return alive;
            }
            set
            {
                alive = value;
                if (!value)
                    SnakeBody.Last().CellType = Enums.CellType.Dead;
            }
        }

        public List<GridCell> SnakeBody { get; set; }

        public bool ArrowControlled { get; set; }

        public Snake(int startRowPos, int startColumnPos)
        {
            SnakeBody = new List<GridCell>();
            Alive = true;
            SnakeBody.Add(new GridCell(Enums.CellType.Head, startColumnPos, startRowPos));
            Direction = Enums.Direction.Right;
            for (int i = 0; i < 2; i++)
            {
                Grow();
            }
            
            ArrowControlled = false;
        }

        public int[] MoveUp()
        {
            Direction = Enums.Direction.Up;
            SnakeBody.Last().Row--;
            return MoveBody(SnakeBody.Last().Row + 1, SnakeBody.Last().Column);
        }

        public int[] MoveDown()
        {
            Direction = Enums.Direction.Down;
            SnakeBody.Last().Row++ ;
           return MoveBody(SnakeBody.Last().Row - 1, SnakeBody.Last().Column);
        }

        public int[] MoveLeft()
        {
            Direction = Enums.Direction.Left;
            SnakeBody.Last().Column--;
            return MoveBody(SnakeBody.Last().Row, SnakeBody.Last().Column + 1);
        }

        public int[] MoveRight()
        {
            Direction = Enums.Direction.Right;
            SnakeBody.Last().Column ++;
            return MoveBody(SnakeBody.Last().Row, SnakeBody.Last().Column - 1);
        }

        public void Grow()
        {
            switch (Direction)
            {
                case Enums.Direction.Up:
                    AddLimb(SnakeBody.Last().Row - 1, SnakeBody.Last().Column);
                    break;
                case Enums.Direction.Down:
                    AddLimb(SnakeBody.Last().Row + 1, SnakeBody.Last().Column);
                    break;
                case Enums.Direction.Left:
                    AddLimb(SnakeBody.Last().Row, SnakeBody.Last().Column - 1);
                    break;
                case Enums.Direction.Right:
                    AddLimb(SnakeBody.Last().Row, SnakeBody.Last().Column + 1);
                    break;
                default:
                    break;
            }
        }

        private void AddLimb(int row, int column)
        {
            SnakeBody.Last().CellType = Enums.CellType.Body;
            
            SnakeBody.Add(new GridCell(Enums.CellType.Head, row, column));
        }

        private int[] MoveBody(int originalRow, int originalColumn)
        {
            var tempRow = originalRow;
            var tempColumn = originalColumn;
            SnakeBody.Reverse();
            foreach (GridCell limb in SnakeBody)
            {
                if (limb.CellType == Enums.CellType.Head)
                    continue;

                var nextRow = limb.Row;
                var nextColumn = limb.Column;
                
                limb.Row = tempRow;
                tempRow = nextRow;
                limb.Column = tempColumn;
                tempColumn = nextColumn;
            }
            SnakeBody.Reverse();
            return new int[] { tempRow, tempColumn };
        }
    }
}
