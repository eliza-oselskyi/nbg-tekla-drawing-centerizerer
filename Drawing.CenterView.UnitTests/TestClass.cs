using NUnit.Framework;
using System;
using System.Collections.Generic;
using Tekla.Structures.Drawing;
using Tekla.Structures.DrawingInternal;

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
                    Console.WriteLine(viewType.TryGetValue("GaViewType", out var vt));
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

    [Test]
    public void TestMethod2()
    {
        var drawingHandler = new Tekla.Structures.Drawing.DrawingHandler();
        var picker = drawingHandler.GetPicker();
         picker.PickObject("Pick object", out DrawingObject drawingObject,out ViewBase view);

         
         view.GetIntegerUserProperties(out Dictionary<string, int> properties);

         TestContext.WriteLine(((View)view).ViewType.ToString());
         TestContext.WriteLine(((View)view).Name);
         TestContext.WriteLine(view.GetType().ToString());

         foreach (KeyValuePair<string,int> keyValuePair in properties)
         {
            TestContext.WriteLine(keyValuePair.Key + " : " + keyValuePair.Value);
            TestContext.WriteLine("test");
         }
         
         
    }

    [Test]
    public void GetSelected()
    {
        var drawingHandler = new Tekla.Structures.Drawing.DrawingHandler();
        var drawingEnumerator = drawingHandler.GetDrawingSelector().GetSelected();

        while (drawingEnumerator.MoveNext())
        {
            var t = "";
            var views = drawingEnumerator.Current.GetSheet().GetViews();
            Console.WriteLine(drawingEnumerator.Current.DrawingTypeStr);
            Console.WriteLine(drawingEnumerator.Current.GetType());

            while (views.MoveNext())
            {
                views.Current.GetUserProperty("ViewType", ref t);
                TestContext.WriteLine(t);
            }
            
        }
    }
}