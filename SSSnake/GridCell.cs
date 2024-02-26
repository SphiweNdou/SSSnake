using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SSSnake
{
    public class GridCell
    {
        public Brush Color { get; set; }

        private Enums.CellType cellType;
        public Enums.CellType CellType
        {
            get {
                return cellType;
            }

            set {
                cellType = value;
                switch (value)
                {
                    case Enums.CellType.Empty:
                        Color = Brushes.Black;
                        break;
                    case Enums.CellType.Head:
                        Color = Brushes.YellowGreen;
                        break;
                    case Enums.CellType.Body:
                        Color = Brushes.White;
                        break;
                    case Enums.CellType.Food:
                        Color = Brushes.Red;
                        break;
                    case Enums.CellType.Dead:
                        Color = Brushes.SaddleBrown;
                        break;
                    default:
                        break;
                }
            }
        }
        public int Row { get; set; }
        public int Column { get; set; }
        public GridCell(Enums.CellType type, int row, int column)
        {
            CellType = type;
            Row = row;
            Column = column;
        }
    }
}
