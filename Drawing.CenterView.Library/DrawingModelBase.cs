using System.Collections;
using Tekla.Structures.Drawing;

namespace Drawing.CenterView.Library;

public abstract class DrawingModelBase : IValidation, IDrawingOperations
{

    /// <summary>
    /// The Tekla Drawing object associated with this instance.
    /// </summary>
    public Tekla.Structures.Drawing.Drawing Drawing { get; set; }
    /// <summary>
    /// The name of the drawing.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The Title_3 property of the drawing.
    /// </summary>
    private string _title3;
    /// <summary>
    /// The Tekla drawing type of the current drawing.
    /// </summary>
    public Type DrawingType { get; set; }
    /// <summary>
    /// All the views contained in the sheet.
    /// </summary>
    private Tekla.Structures.Drawing.DrawingObjectEnumerator Views { get; set; }
    /// <summary>
    /// The list of ViewModel that are valid for centering. Currently, if this list is greater than one,
    /// the drawing gets skipped for centering.
    /// </summary>
    public List<Tekla.Structures.Drawing.View> ValidViews { get; } = [];
    /// <summary>
    /// True if drawing is excluded from being centered.
    /// Currently, this value is derived from the Title_3 property;
    /// if it is "X", the drawing is excluded.
    /// </summary>
    public virtual bool Excluded { get; }
    /// <summary>
    /// The value that represents whether the drawing is open in the drawing editor.
    /// If true, the GUI will appear.
    /// </summary>
    private bool _guiMode;

    /// <summary>
    /// The DrawingSet current drawing belongs to.
    /// </summary>
    private DrawingSetModel _parentDrawingSet;


    protected DrawingModelBase(Tekla.Structures.Drawing.Drawing drawing)
    {
        Drawing = drawing;
        DrawingType = Drawing.GetType();
        Views = Drawing.GetSheet()
                       .GetViews();
    }

    protected void GetValidViews()
    {
        throw new NotImplementedException();
    }

    protected void SetExcluded()
    {
        throw new NotImplementedException();
    }

    public bool IsValidForCenter()
    {
        throw new NotImplementedException();
    }

    public void Center()
    {
        throw new NotImplementedException();
    }

    public abstract void FilterValid(List<DrawingModelBase> drawings);
}

public class FabDrawingModel(Tekla.Structures.Drawing.Drawing drawing) : DrawingModelBase(drawing)
{
    public override void FilterValid(List<DrawingModelBase> drawings)
    {
        throw new NotImplementedException();
    }
}

public class GaDrawingModel(Tekla.Structures.Drawing.Drawing drawing) : DrawingModelBase(drawing)
{
    public override void FilterValid(List<DrawingModelBase> drawings)
    {
        throw new NotImplementedException();
    }
}

public class NullDrawingModel(Tekla.Structures.Drawing.Drawing drawing) : DrawingModelBase(drawing)
{
    private static readonly Lazy<NullDrawingModel> Lazy =
        new Lazy<NullDrawingModel>(() => new NullDrawingModel());
    public static NullDrawingModel Instance => Lazy.Value;
    private NullDrawingModel() : this(new GADrawing())
    { }
    public override void FilterValid(List<DrawingModelBase> drawings)
    {
    }
    public override bool Excluded => true;
}