using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Tekla.Structures;

namespace Drawing.CenterView;

public static class DrawingUtils
{
    /// <summary>
    ///     Method <c>IsValidDrawingForCenter</c> checks if there is exactly one view that has a valid property from ViewType
    ///     enum.
    /// </summary>
    /// <param name="drawing"></param>
    /// <returns></returns>
    public static bool IsValidDrawingForCenter(Tekla.Structures.Drawing.Drawing drawing)
    {
        var memberCount = 0;
        var views = drawing.GetSheet().GetViews();

        while (views.MoveNext())
        {
            views.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes);
            var type = PluginForm.GetViewTypeEnum(viewTypes);
            if (type is not ViewType.None) memberCount++;
        }

        return memberCount == 1; // valid if memberCount is 1
    }

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

    public static void FinalizeDrawing(Tuple<Tekla.Structures.Drawing.Drawing, string> s)
    {
        QuickCenterClass.DrawingHandler1.GetActiveDrawing().CommitChanges("Center View");
        QuickCenterClass.DrawingHandler1.SaveActiveDrawing();
        QuickCenterClass.DrawingHandler1.CloseActiveDrawing(true);
        if (s.Item1.Title3.Equals("X")) return;
        s.Item1.Title3 = s.Item2.ToString();
        s.Item1.Modify();
    }
}