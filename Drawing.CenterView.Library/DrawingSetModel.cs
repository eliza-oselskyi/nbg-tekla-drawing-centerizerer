using Tekla.Structures.Drawing;

namespace Drawing.CenterView.Library;

public class DrawingSetModel : IDrawingOperations
{
    private DrawingEnumerator RawDrawingList { get; set; }
    /// <summary>
    /// The list that contains all the valid drawings for centering in the set.
    /// </summary>
    public List<DrawingModelBase> FilteredDrawingsList { get; protected set; }
    /// <summary>
    /// Represents the number of drawings to be processed. This is taken from the FilteredDrawingsList.
    /// </summary>
    public int Count  { get; protected set; }
    /// <summary>
    /// Represents the current progress. Increments by one when moved to the next drawing.
    /// </summary>
    public int Progress { get; protected set; }
    /// <summary>
    /// The current drawing in the set.
    /// </summary>
    public DrawingModelBase CurrentDrawing { get; protected set; }
    private DrawingSetOptions _options;

    public DrawingSetModel()
    {
        var selectedDrawings = GlobalConfig.DrawingHandler.GetDrawingSelector().GetSelected();
    }

    /// <summary>
    /// When option provided, only certain sets of drawings will be included in the centering routine.
    /// </summary>
    /// <param name="options"></param>
    public DrawingSetModel(DrawingSetOptions options)
    {
        _options = options;
        List<DrawingModelBase> partialFilter = _options switch
        {
            DrawingSetOptions.Fab => CreatePartialFilteredDrawingList(typeof(AssemblyDrawing)),
            DrawingSetOptions.Ga => CreatePartialFilteredDrawingList(typeof(GADrawing)),
            DrawingSetOptions.All => CreatePartialFilteredDrawingList(typeof(Tekla.Structures.Drawing.Drawing)),
            DrawingSetOptions.Selected => CreateDrawingListFromSelected(),
            _ => throw new ArgumentOutOfRangeException()
        };
        // Call FilterValid
    }

    // TODO Create Test
    /// <summary>
    /// Creates a partially filtered list from the raw list, of the selected drawings.
    /// The return value gets used by FilterValid to populate the field FilteredDrawingList
    /// </summary>
    /// <returns><list type="DrawingModelBase"></list></returns>
    private List<DrawingModelBase> CreateDrawingListFromSelected()
    {
        var  result = new List<DrawingModelBase>();
        while (RawDrawingList.MoveNext())
        {
            var currDrawing =  RawDrawingList.Current;
            var drawingType = currDrawing.GetType();
            DrawingModelBase drawingModel;

            if (drawingType == typeof(Tekla.Structures.Drawing.AssemblyDrawing))
            {
                drawingModel = new FabDrawingModel(currDrawing);
            }
            else if (drawingType == typeof(GADrawing))
            {
                drawingModel = new GaDrawingModel(currDrawing);
                
            }
            else
            {
                drawingModel = NullDrawingModel.Instance;
            }
            result.Add(drawingModel);
            
        }
        return result;
    }

    // TODO create test
    private List<DrawingModelBase> CreatePartialFilteredDrawingList(Type type)
    {
        var drawingList = new List<DrawingModelBase>();
        while (RawDrawingList.MoveNext())
        {
            var currDrawing = RawDrawingList.Current;
            Type drawingType;

            if (type == typeof(Tekla.Structures.Drawing.GADrawing))
            {
                drawingType = typeof(GaDrawingModel);
            }
            else if (type == typeof(Tekla.Structures.Drawing.AssemblyDrawing))
            {
                drawingType = typeof(GaDrawingModel);
            }
            else
            {
                drawingType = typeof(NullDrawingModel);
            }
            
            if (currDrawing.GetType() != type) continue;
            var drawingModel = (drawingType == typeof(NullDrawingModel))
                ? NullDrawingModel.Instance
                : drawingType.GetConstructor(
                    [type])?.Invoke([]);
                
            drawingList.Add((DrawingModelBase)drawingModel ?? NullDrawingModel.Instance);
        }

        return drawingList;
    }
    

    public void Center()
    {
        throw new NotImplementedException();
    }

    // TODO create test
    public void FilterValid(List<DrawingModelBase> drawings)
    {
        FilteredDrawingsList = [];
        foreach (var drawing in drawings.Where(drawing => drawing.IsValidForCenter()))
        {
            FilteredDrawingsList.Add(drawing);
        }
    }
}

public enum DrawingSetOptions
{
    Fab,
    Ga,
    All,
    Selected
}