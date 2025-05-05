/*
 *
 Drawing Centerizer-er: Mainly centers Tekla drawings, specifically NBG's flavor.
      Copyright (C) 2025  Eliza Oselskyi

      This program is free software: you can redistribute it and/or modify
      it under the terms of the GNU Lesser General Public License as published by
      the Free Software Foundation, either version 3 of the License, or
      (at your option) any later version.

      This program is distributed in the hope that it will be useful,
      but WITHOUT ANY WARRANTY; without even the implied warranty of
      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
      GNU Lesser General Public License for more details.

      You should have received a copy of the GNU Lesser General Public License
      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Resources;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Tekla.Structures.Drawing;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Assembly = Tekla.Structures.Model.Assembly;
using Object = Tekla.Structures.Model.Object;
using CV = Drawing.CenterView;
using View = Tekla.Structures.Drawing.View;

namespace Drawing.CenterView.UnitTests
{
    [TestFixture]
    public class CenterViewTests
    {
        private Model _model;
        private DrawingHandler _drawingHandler;

        // TODO: Refactor these tests to use the Setup and Teardown methods
        [Test]
        public void CenterView_NothingToDo_ReturnNC()
        {
            _drawingHandler = new DrawingHandler();
            var drawing = _drawingHandler.GetActiveDrawing();
            var views = drawing.GetSheet().GetViews();
            if (views.GetSize() == 1)
            {
                views.MoveNext();
                var view = views.Current as View;
                var origViewOrigin = view.Origin;
                views.Current.GetStringUserProperties(out var viewTypes); // Get viewTypes
                var type = PluginForm.GetViewTypeEnum(viewTypes);

                if (type is ViewType.None)
                {
                    Assert.Ignore("No view valid type found");
                }
                else
                {
                    if (view == null) Assert.Inconclusive("Null view found");
                    Console.WriteLine("\n\nTest View Origin: " + view.Origin.ToString());
                    var result = DrawingMethods.CenterView((ViewBase)view, (int)type,
                        out var drawingTuple);
                    Assert.True(result.Contains("Nothing To Do.") && drawingTuple.Item2.Equals("NC"),
                        "Fail, Had something to do.");
                }

                //Reset everything
                view.Origin = origViewOrigin;
                view.Modify();
            }
            else if (views.GetSize() > 1)
            {
                var member = 0;

                while (views.MoveNext())
                {
                    var curr = views.Current as View;
                    if (curr == null) Assert.Inconclusive("Null current view found");
                    curr.GetStringUserProperties(out var viewTypes);
                    var type = PluginForm.GetViewTypeEnum(viewTypes);

                    if (type != ViewType.None) member++;
                }

                if (member > 1) Assert.Inconclusive("More than one \"valid\" view found");
            }
            else
            {
                Assert.Inconclusive("No views found");
            }
        }

        [Test]
        public void CenterView_SomethingToDo_ReturnC()
        {
            _drawingHandler = new DrawingHandler();
            var drawing = _drawingHandler.GetActiveDrawing();
            var views = drawing.GetSheet().GetViews();
            if (views.GetSize() == 1)
            {
                views.MoveNext();
                var view = views.Current as View;
                views.Current.GetStringUserProperties(out var viewTypes); // Get viewTypes
                var type = PluginForm.GetViewTypeEnum(viewTypes);

                if (type is ViewType.None)
                {
                    Assert.Ignore("No view valid type found");
                }
                else
                {
                    if (view == null) Assert.Inconclusive("Null view found");
                    Console.WriteLine("\n\nTest View Origin: " + view.Origin.ToString());
                    var result = DrawingMethods.CenterView((ViewBase)view, (int)type,
                        out var drawingTuple);
                    Assert.True(result.Contains("Centering") && drawingTuple.Item2.Equals("C"),
                        "Fail, Had nothing to do.");
                }
            }
            else if (views.GetSize() > 1)
            {
                var member = 0;

                while (views.MoveNext())
                {
                    var curr = views.Current as View;
                    if (curr == null) Assert.Inconclusive("Null current view found");
                    curr.GetStringUserProperties(out var viewTypes);
                    var type = PluginForm.GetViewTypeEnum(viewTypes);

                    if (type != ViewType.None) member++;
                }

                if (member > 1) Assert.Inconclusive("More than one \"valid\" view found");
            }
            else
            {
                Assert.Inconclusive("No views found");
            }
        }
    }
}