using Tekla.Structures.Drawing;
using Tekla.Structures.Geometry3d;

namespace Drawing.CenterView.Library;

public class SheetModel
{
    private Tekla.Structures.Drawing.Drawing Drawing;
    private static ContainerView SheetView { get; set; }
    /// <summary>
    /// Represents the adjusted size of the sheet.
    /// </summary>
    public Tuple<double, double> Size { get; set; } = new  Tuple<double, double>(SheetView.Width, SheetView.Height);
    /// <summary>
    /// The title block width.
    /// </summary>
    private double TitleBlockWidth { get; set; }
    /// <summary>
    /// The template block height. Can change depending on the drawing.
    /// </summary>
    private double TemplateBlockHeight { get; set; }
    /// <summary>
    /// Used internally to test equality against the center point of a view.
    /// </summary>
    private Point AdjustedCenter { get; set; }

    public SheetModel(ContainerView sheet)
    {
        SheetView = sheet;
    }
}