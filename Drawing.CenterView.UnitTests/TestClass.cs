using NUnit.Framework;
using System;
using System.Collections.Generic;
using Tekla.Structures.Drawing;

namespace Drawing.CenterView.UnitTests;

public class TestClass
{
    //[Test]
    public void TestMethod1()
    {
        var drawingHandler = new Tekla.Structures.Drawing.DrawingHandler();
        var allDwgs = drawingHandler.GetDrawingSelector().GetSelected();
        while (allDwgs.MoveNext())
            if (allDwgs.Current is GADrawing)
            {
                drawingHandler.SetActiveDrawing(allDwgs.Current, false);
                var x = allDwgs.Current.GetSheet().GetAllViews();
                Console.WriteLine(((GADrawing)allDwgs.Current).Name + " : " + x.GetSize());
                while (x.MoveNext())
                {
                    x.Current.GetStringUserProperties(out var viewType);
                    Console.WriteLine(viewType.TryGetValue("ViewType", out var vt));
                }

                drawingHandler.CloseActiveDrawing();
            }
    }

    //[Test]
    public void CloseCurrentDrawing()
    {
        var drawingHandler = new Tekla.Structures.Drawing.DrawingHandler();
        drawingHandler.CloseActiveDrawing();
    }
}