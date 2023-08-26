using System.Drawing;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    class DataGridBitmapHeaderCell : DataGridViewColumnHeaderCell
    {
        Point bitmapLocation;
        Size bitmapSize = new Size(20, 20);
        Bitmap bitmap;

        Point cellLocation = new Point();

        public DataGridBitmapHeaderCell()
        {
        }

        public DataGridBitmapHeaderCell(Bitmap originalBitmap, string toolTipText)
        {
            bitmap = originalBitmap;
            ToolTipText = toolTipText;
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                dataGridViewElementState, value,
                "" /*formattedValue*/, errorText, cellStyle,
                advancedBorderStyle, paintParts);

            Point p = new Point();
            p.X = cellBounds.Location.X +
                (cellBounds.Width / 2) - (bitmapSize.Width / 2);
            p.Y = cellBounds.Location.Y +
                (cellBounds.Height / 2) - (bitmapSize.Height / 2);
            cellLocation = cellBounds.Location;
            bitmapLocation = p;

            graphics.DrawImage(bitmap, new Rectangle(bitmapLocation, bitmapSize));
        }
    }
}
