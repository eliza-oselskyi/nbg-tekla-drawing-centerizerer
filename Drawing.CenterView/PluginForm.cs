/*
 *
 *  Drawing Centerizer-er: Mainly centers Tekla drawings, specifically NBG's flavor.
 *       Copyright (C) 2025  Eliza Oselskyi
 *
 *       This program is free software: you can redistribute it and/or modify
 *       it under the terms of the GNU Lesser General Public License as published by
 *       the Free Software Foundation, either version 3 of the License, or
 *       (at your option) any later version.
 *
 *       This program is distributed in the hope that it will be useful,
 *       but WITHOUT ANY WARRANTY; without even the implied warranty of
 *       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *       GNU Lesser General Public License for more details.
 *
 *       You should have received a copy of the GNU Lesser General Public License
 *       along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Svg;
using System.Windows.Forms;
using Drawing.CenterView.Properties;
using Tekla.Structures;
using Tekla.Structures.Catalogs;
using Tekla.Structures.Dialog;
using Tekla.Structures.Dialog.UIControls;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using Tekla.Structures.DrawingInternal;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using Tekla.Structures.ModelInternal;
using Color = RenderData.Color;
using ModelObjectSelector = Tekla.Structures.Model.UI.ModelObjectSelector;
using Polygon = Tekla.Structures.Model.Polygon;
using Size = System.Drawing.Size;
using TSG = Tekla.Structures.Geometry3d;
using TSD = Tekla.Structures.Drawing;

// ReSharper disable LocalizableElement

namespace Drawing.CenterView
{
    public partial class PluginForm : ApplicationFormBase
    {
        private readonly Model _myModel;
        private static DrawingHandler? DrawingHandler { get; set; }

#pragma warning disable CS8618, CS9264
        public PluginForm() // Main entry point for the form.
#pragma warning restore CS8618, CS9264
        {
            InitializeComponent();
            base.InitializeForm(); // Required for TeklaAPI

            // Set up the private, read-only fields. Can do only in here (the constructor method).
            _myModel = new Model();
            DrawingHandler = new DrawingHandler();
        }

        private ViewBase? GetValidViewInActiveDrawing()
        {
            var allViews = DrawingHandler.GetActiveDrawing().GetSheet().GetAllViews();
            var memberCount = 0;
            var memberList = new ArrayList();
            while (allViews.MoveNext())
            {
                allViews.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes);
                var type = PluginForm.GetViewTypeEnum(viewTypes);
                if (type is ViewType.None) continue;
                memberCount++;
                memberList.Add(allViews.Current);
            }

            if (memberCount > 1) return null;

            var memberListArray = memberList.ToArray();
            allViews.Reset();

            if (memberListArray[0] == null) return null;
            var currentView = memberListArray[0];
            try
            {
                return (ViewBase?)currentView;
            }
            catch (Exception e) when (e is KeyNotFoundException)
            {
                Console.WriteLine(@"Invalid View: " + currentView.ToString());
            }

            return null;
        }

        private void CenterViewDriver()
        {
            var currView = GetValidViewInActiveDrawing();
            if (currView != null)
            {
                currView.GetStringUserProperties(out Dictionary<string, string> viewTypes);
                CenterView(currView, (int)GetViewTypeEnum(viewTypes));
            }
            else
            {
                InfoBox.OnError(infoBox,
                    new Exception("Something went wrong.\nDo you have more than one main view?",
                        new NullReferenceException()));
            }
        }

        private void CenterView(ViewBase view, int viewType)
        {
            var sheet = view.GetDrawing().GetSheet();
            double sheetHeightOffset = 0;
            switch (viewType)
            {
                case 1:
                    sheetHeightOffset = 25.4; // 1"
                    break;
                case >= 2 and <= 24:
                    sheetHeightOffset = 22.225; // 7/8"
                    break;
                default: break;
            }

            sheet.Origin.Y = sheetHeightOffset;
            var originalOriginX = view.Origin.X;
            var originalOriginY = view.Origin.Y;
            view.Origin = sheet.Origin;
            var viewCenterPoint = view.GetAxisAlignedBoundingBox().GetCenterPoint();

            var sheetHeight = sheet.Height / 2;
            var sheetWidth = (sheet.Width - 33.274) / 2;
            var xOffset = sheetWidth - viewCenterPoint.X;
            var yOffset = sheetHeight - viewCenterPoint.Y;

            if (Math.Abs(originalOriginX - xOffset) < 0.0001 &&
                Math.Abs(originalOriginY - yOffset - sheetHeightOffset) < 0.0001)
            {
                InfoBox.OnInfo(infoBox, @"Nothing To Do.");
            }
            else if (Math.Abs(view.ExtremaCenter.X - sheetWidth) > 0.0001 ||
                     Math.Abs(view.ExtremaCenter.Y - sheetHeight) > 0.0001)
            {
                InfoBox.OnInfo(infoBox, $"Centering {(ViewType)viewType}");
                view.Origin.X += xOffset;
                view.Origin.Y += yOffset;

                view.Modify();
                DrawingHandler.GetActiveDrawing().CommitChanges("Center View");
            }
        }

        // NOTE: Overloads default Windows Forms OnLoad() method.
        // I'm setting up the environment for the application, here.
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                UI();

                InfoBox.OnInfo(infoBox,
                    _myModel.GetConnectionStatus() ? "Connection succeeded" : "Connection failed");

                if (!_myModel.GetConnectionStatus())
                    // Disable buttons if no connection found
                    selectedObjectsButton.Enabled = false;

                if (onTopCheckBox.Checked) this.TopMost = true;
            }
            catch (Exception)
            {
                // InfoBox.OnError(infoBox, e);
                InfoBox.OnError(infoBox, new Exception("Connection failed. Do you have a model open?" +
                                                       "\n Currently, there is no way to reestablish connection. " +
                                                       "You must restart this application to do so."));
            }
        }
    }

}