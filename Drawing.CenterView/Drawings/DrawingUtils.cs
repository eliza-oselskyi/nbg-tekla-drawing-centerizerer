using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Drawing.CenterView.Views;
using Tekla.Structures;
using Tekla.Structures.Drawing;

namespace Drawing.CenterView;

public static class DrawingUtils
{
    /// <summary>
    ///     Method <c>IsValidDrawingForCenter</c> checks if there is exactly one view that has a valid property from GaViewType
    ///     enum.
    /// </summary>
    /// <param name="drawing"></param>
    /// <returns></returns>
    public static bool IsValidDrawingForCenter(Tekla.Structures.Drawing.GADrawing drawing)
    {
        var memberCount = 0;
        var views = drawing.GetSheet().GetViews();

        while (views.MoveNext())
        {
            views.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes);
            var type = DrawingMethods.GetViewTypeEnum(viewTypes);
            if (type is not GaViewType.None) memberCount++;
        }

        return memberCount == 1; // valid if memberCount is 1
    }
    
    // TODO needs tests!
    public static bool IsValidDrawingForCenter(AssemblyDrawing drawing)
    {
        var memberCount = 0;
        var views = drawing.GetSheet().GetViews();

        while (views.MoveNext())
        {
            if (views.Current is View { ViewType: not View.ViewTypes._3DView
                    and not View.ViewTypes.DetailView
                    and not View.ViewTypes.DetailView
                })
            {
                memberCount++;
            }
        }
        return memberCount == 1;
    }

    // TODO refactor out this stupid tuple!
    public static void RenameDrawingTitle3FromTuple(Tuple<Tekla.Structures.Drawing.Drawing, string> drawingTuple)
    {
        drawingTuple.Item1.Title3 = drawingTuple.Item2.ToString();
        drawingTuple.Item1.Modify();
    }

    public static void CleanUp(Tekla.Structures.Drawing.Drawing drawing)
    {
        if (drawing.Title3.Equals("X")) return;
        drawing.Title3 = "";
        drawing.Modify();
    }

    // TODO needs tests!
    // TODO refactor out this stupid tuple!
    public static void FinalizeDrawing(Tuple<Tekla.Structures.Drawing.Drawing, string> s)
    {
        var drawingHandler = new DrawingHandler();
        drawingHandler.GetActiveDrawing().CommitChanges("Center View");
        drawingHandler.SaveActiveDrawing();
        drawingHandler.CloseActiveDrawing(true);
        if (s.Item1.Title3.Equals("X")) return;
        s.Item1.Title3 = s.Item2.ToString();
        s.Item1.Modify();
    }

}