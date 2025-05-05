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
using System.Text;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Tekla.Structures.Model;
using Tekla.Structures.ModelInternal;
using Color = System.Drawing.Color;
using ModelObjectSelector = Tekla.Structures.Model.UI.ModelObjectSelector;

namespace Drawing.CenterView;

public partial class PluginForm
{
    private readonly Tekla.Structures.Drawing.UI.Events _UiEvents = new Tekla.Structures.Drawing.UI.Events();
    private readonly Tekla.Structures.Drawing.Events _events = new Tekla.Structures.Drawing.Events();
    private readonly object _exitEventHandlerLock = new object();

    /// <summary>
    ///     Method <c>CheckDrawingState</c> Checks the state of current active drawing and sets UI elements accordingly
    /// </summary>
    private void CheckDrawingState()
    {
        var drawing = DrawingHandler.GetActiveDrawing();
        if (drawing.Title3 == "X")
        {
            if (excludeCheckBox.InvokeRequired)
                this.Invoke(new MethodInvoker(delegate { excludeCheckBox.CheckState = CheckState.Checked; }));
        }
        else
        {
            if (excludeCheckBox.InvokeRequired)
                this.Invoke(new MethodInvoker(delegate { excludeCheckBox.CheckState = CheckState.Unchecked; }));
        }
    }

    /// <summary>
    ///     Method <c>onTopCheckBox_CheckedChanged</c> toggles "always stay on top"
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onTopCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        this.TopMost = onTopCheckBox.Checked;
    }

    /// <summary>
    ///     Method <c>selectedObjectsButton_Click</c> retrieves all selected objects in model and prints them in the infobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void selectedObjectsButton_Click(object sender, EventArgs e)
    {
        InfoBox.ToDefault(infoBox); // restore to defaults

        // Create a selector and enumerator of selected objects. StringBuilder for the final output of the 
        // aggregated objects.
        var modelObjectSelector = new ModelObjectSelector();
        var selectedObjects = modelObjectSelector.GetSelectedObjects();
        var selectedObjectString = new StringBuilder();

        if (selectedObjects.GetSize() > 1000)
        {
            InfoBox.OnError(infoBox, new Exception("Selected object count is too large"));
        }
        else if (selectedObjects.GetSize() > 0)
        {
            while (selectedObjects.MoveNext())
                selectedObjectString.AppendLine(selectedObjects.Current + @"=> ID: " +
                                                selectedObjects.Current.Identifier + "\n Up to date? " +
                                                selectedObjects.Current.IsUpToDate);

            InfoBox.OnInfo(infoBox, selectedObjectString.ToString());
        }
        else
        {
            // If no objects selected, replace text of the button with "no objects selected" for 1 second.
            selectedObjectsButton.Text = @"No Objects Selected";
            Wait(1000);
            selectedObjectsButton.Text = @"Get Selected Objects";
        }
    }

    private void refreshButton_Click(object sender, EventArgs e)
    {
        UI();
    }

    private void iconImage_MouseHover(object sender, EventArgs e)
    {
        ChangePictureBoxIconColor(sender, Color.White,
            ((PictureBox)sender).Equals(centerImage) ? Color.Aquamarine : Color.Tomato);
    }

    private void iconImage_MouseLeave(object sender, EventArgs e)
    {
        ChangePictureBoxIconColor(sender, ((PictureBox)sender).Equals(centerImage) ? Color.Aquamarine : Color.Tomato,
            Color.White);
    }

    private void ChangePictureBoxIconColor(object sender, Color sourceColor, Color targetColor)
    {
        if (sender is not PictureBox pictureBox) return;
        _svgIcons.TryGetValue(pictureBox.Name.ToString(), out var icon);
        if (icon == null) return;
        icon.Resize(pictureBox.Width, pictureBox.Height);
        icon.ChangeIconColors(sourceColor, targetColor);
        pictureBox.Image = icon.Bmp;
    }

    private void centerImage_Click(object sender, EventArgs e)
    {
        CenterViewDriver();
    }


    private void leftChevronImage_Click(object sender, EventArgs e)
    {
        ShiftViewLeft(5);
    }

    private void leftArrowImage_Click(object sender, EventArgs e)
    {
        ShiftViewLeft(20);
    }

    private void topChevronImage_Click(object sender, EventArgs e)
    {
        ShiftViewUp(5);
    }

    private void topArrowImage_Click(object sender, EventArgs e)
    {
        ShiftViewUp(20);
    }

    private void rightChevronImage_Click(object sender, EventArgs e)
    {
        ShiftViewRight(5);
    }

    private void rightArrowImage_Click(object sender, EventArgs e)
    {
        ShiftViewRight(20);
    }

    private void bottomChevronImage_Click(object sender, EventArgs e)
    {
        ShiftViewDown(5);
    }

    private void bottomArrowImage_Click(object sender, EventArgs e)
    {
        ShiftViewDown(20);
    }

    private void watermark_MouseHover(object sender, EventArgs e)
    {
        var label = sender as Label;
        if (label != null) label.ForeColor = System.Drawing.Color.PowderBlue;
        if (label != null) label.BackColor = System.Drawing.Color.DimGray;
    }

    private void watermark_MouseLeave(object sender, EventArgs e)
    {
        var label = sender as Label;
        var newColor = System.Drawing.Color.FromArgb(40, 57, 56);
        if (label != null) label.ForeColor = newColor;
        if (label != null) label.BackColor = System.Drawing.Color.Transparent;
    }

    private void excludeCheckBox_CheckStateChanged(object sender, EventArgs e)
    {
        if (sender is not CheckBox checkBox) return;

        var currDrawing = DrawingHandler.GetActiveDrawing();
        currDrawing.Title3 = checkBox.CheckState switch
        {
            CheckState.Unchecked => "",
            CheckState.Checked => "X",
            CheckState.Indeterminate => "",
            _ => throw new ArgumentOutOfRangeException()
        };

        currDrawing.CommitChanges();
        currDrawing.Modify();
    }

    private void excludeCheckBox_MouseHover(object sender, EventArgs e)
    {
        if (sender is not CheckBox checkBox) return;
        Wait(100);
        checkBox.Text = @"Must Save Drawing Before Exiting Drawing!";
    }

    private void excludeCheckBox_MouseLeave(object sender, EventArgs e)
    {
        if (sender is not CheckBox checkBox) return;
        checkBox.Text = @"Exclude Drawing From Centering Macro?";
    }

    /// <summary>
    ///     Method <c>Wait</c> waits asynchronously for x amount of milliseconds
    /// </summary>
    /// <param name="milliseconds"></param>
    // Create a timer, instead of using sleep, to not lock up the UI. (Sleep pauses the entire thread for a given amount of milliseconds)
    private static void Wait(int milliseconds)
    {
        if (milliseconds == 0 | milliseconds < 0) return;

        var timer = new Timer();
        timer.Interval = milliseconds;
        timer.Enabled = true;
        timer.Start();
        timer.Tick += (_, _) => // this is the event. All it does is stop the timer after 1 second.
        {
            timer.Enabled = false; // Stops the while loop
            timer.Stop();
        };
        while (timer.Enabled) Application.DoEvents(); // Goes to the event
    }

    private void PluginForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        _UiEvents.UnRegister();
        _events.UnRegister();
    }

    private void ExitApplication()
    {
        lock (_exitEventHandlerLock)
        {
            Application.Exit();
        }
    }
}