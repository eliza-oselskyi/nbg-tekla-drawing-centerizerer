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
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Drawing.CenterView.Views;

namespace Drawing.CenterView;

public partial class PluginForm
{
    private SvgIcon _rightArrow;
    private SvgIcon _leftArrow;
    private SvgIcon _topArrow;
    private SvgIcon _bottomArrow;
    private SvgIcon _rightChevron;
    private SvgIcon _leftChevron;
    private SvgIcon _topChevron;
    private SvgIcon _bottomChevron;
    private SvgIcon _paper;
    private SvgIcon _center;

    private readonly Dictionary<string, SvgIcon>
        _svgIcons = new Dictionary<string, SvgIcon>(); // used to get associated SvgIcon from PictureBox

    // ReSharper disable once InconsistentNaming
    private Color InvertColor(Color color)
    {
        var colorR = color.R;
        var colorG = color.G;
        var colorB = color.B;
        var newColor = Color.FromArgb(Math.Abs(colorR - 255), Math.Abs(colorG - 255), Math.Abs(colorB - 255));
        return newColor;
    }

    // ReSharper disable once InconsistentNaming
    private void UI()
    {
        // NOTE: for sake debugging purposes, the property PluginForm.ShowInTaskbar is set to TRUE.
        // Tekla style guidelines states to have this set to FALSE in deployment.
        // Deployment: false, Testing: true
        if (PluginForm.ActiveForm != null) PluginForm.ActiveForm.ShowInTaskbar = false;

        // Events 
        _uiEvents.OnDrawingLoaded("CheckDrawingState");
        _uiEvents.DrawingEditorClosed += ExitApplication;
        _uiEvents.DrawingLoaded += CheckDrawingState;
        _events.DrawingChanged += CheckDrawingState;
        _uiEvents.Register();
        _events.Register();

        InfoBox.ToDefault(infoBox);
        // Set some default values
        infoBox.AutoSize = true;
        infoBox.MinimumSize = new Size(245, 0);
        infoBox.MaximumSize = new Size(245, 0);
        infoBox.BringToFront();
        try
        {
            Color targetColor;
            if (invertColorsCheckBox.Checked)
            {
                targetColor = Color.Black;
                var backColor = Color.FromArgb(Math.Abs(40 - 255), Math.Abs(57 - 255), Math.Abs(56 - 255));
                if (PluginForm.ActiveForm != null)
                {
                    ActiveForm.BackColor = backColor;
                    header.ForeColor = targetColor;
                    version.ForeColor = targetColor;
                    foreach (Control control in Controls)
                        //control.BackColor = backColor;
                        control.ForeColor = InvertColor(control.ForeColor);
                    //control.BackColor = InvertColor(control.BackColor);
                }
            }
            else
            {
                targetColor = Color.White;
            }

            version.Text = @"v" + Assembly.GetEntryAssembly()?.GetName().Version.ToString();

            if (DrawingHandler.GetActiveDrawing().Title3.Equals("X")) excludeCheckBox.Checked = true;

            _rightArrow = new SvgIcon(SvgIcon.Icon.RightArrow);
            _rightArrow.ChangeIconColors(Color.Black, targetColor);
            _rightArrow.Resize(25, 25);
            _svgIcons.Add(rightArrowImage.Name.ToString(), _rightArrow);
            rightArrowImage.Image = _rightArrow.Bmp;

            _leftArrow = new SvgIcon(SvgIcon.Icon.LeftArrow);
            _leftArrow.ChangeIconColors(Color.Black, targetColor);
            _leftArrow.Resize(25, 25);
            _svgIcons.Add(leftArrowImage.Name.ToString(), _leftArrow);
            leftArrowImage.Image = _leftArrow.Bmp;

            _topArrow = new SvgIcon(SvgIcon.Icon.TopArrow);
            _topArrow.ChangeIconColors(Color.Black, targetColor);
            _topArrow.Resize(25, 25);
            _svgIcons.Add(topArrowImage.Name.ToString(), _topArrow);
            topArrowImage.Image = _topArrow.Bmp;

            _bottomArrow = new SvgIcon(SvgIcon.Icon.BottomArrow);
            _bottomArrow.ChangeIconColors(Color.Black, targetColor);
            _bottomArrow.Resize(25, 25);
            _svgIcons.Add(bottomArrowImage.Name.ToString(), _bottomArrow);
            bottomArrowImage.Image = _bottomArrow.Bmp;

            _rightChevron = new SvgIcon(SvgIcon.Icon.RightChevron);
            _rightChevron.ChangeIconColors(Color.Black, targetColor);
            _rightChevron.Resize(25, 25);
            _svgIcons.Add(rightChevronImage.Name.ToString(), _rightChevron);
            rightChevronImage.Image = _rightChevron.Bmp;

            _leftChevron = new SvgIcon(SvgIcon.Icon.LeftChevron);
            _leftChevron.ChangeIconColors(Color.Black, targetColor);
            _leftChevron.Resize(25, 25);
            _svgIcons.Add(leftChevronImage.Name.ToString(), _leftChevron);
            leftChevronImage.Image = _leftChevron.Bmp;

            _topChevron = new SvgIcon(SvgIcon.Icon.TopChevron);
            _topChevron.ChangeIconColors(Color.Black, targetColor);
            _topChevron.Resize(25, 25);
            _svgIcons.Add(topChevronImage.Name.ToString(), _topChevron);
            topChevronImage.Image = _topChevron.Bmp;

            _bottomChevron = new SvgIcon(SvgIcon.Icon.BottomChevron);
            _bottomChevron.ChangeIconColors(Color.Black, targetColor);
            _bottomChevron.Resize(25, 25);
            _svgIcons.Add(bottomChevronImage.Name.ToString(), _bottomChevron);
            bottomChevronImage.Image = _bottomChevron.Bmp;

            _paper = new SvgIcon(SvgIcon.Icon.Paper);
            _paper.Resize(125, 125);
            _paper.ChangeIconColors(Color.Black, Color.White);
            if (_paper.Bmp != null)
            {
                _paper.Bmp.RotateFlip(RotateFlipType.Rotate90FlipY);
                _svgIcons.Add(paperImage.Name.ToString(), _paper);
                paperImage.Image = _paper.Bmp;

                _center = new SvgIcon(SvgIcon.Icon.Center);
                _center.Resize(25, 25);
                _center.ChangeIconColors(Color.Black, Color.White);
                _svgIcons.Add(centerImage.Name.ToString(), _center);
                centerImage.Image = _center.Bmp;
            }

            Refresh();
        }
        catch (Exception ex)
        {
            InfoBox.OnError(infoBox, ex);
        }
    }

    private void ShiftViewRight(int amount)
    {
        var view = GetValidViewInActiveDrawing();
        if (view == null) return;
        view.Origin.X += amount;
        view.Modify();
        view.GetStringUserProperties(out Dictionary<string, string> viewType);
        DrawingMethods.GetViewTypeEnum(viewType);
        InfoBox.OnInfo(infoBox, $"Shifting Right => {(GaViewType)DrawingMethods.GetViewTypeEnum(viewType)}");
        DrawingHandler.GetActiveDrawing().CommitChanges("Shift View Right");
    }

    private void ShiftViewUp(int amount)
    {
        var view = GetValidViewInActiveDrawing();
        if (view == null) return;
        view.Origin.Y += amount;
        view.Modify();
        view.GetStringUserProperties(out Dictionary<string, string> viewType);
        DrawingMethods.GetViewTypeEnum(viewType);
        InfoBox.OnInfo(infoBox, $"{(GaViewType)DrawingMethods.GetViewTypeEnum(viewType)}\nShifting Up =^");
        DrawingHandler.GetActiveDrawing().CommitChanges("Shift View Up");
    }

    private void ShiftViewDown(int amount)
    {
        var view = GetValidViewInActiveDrawing();
        if (view == null) return;
        view.Origin.Y -= amount;
        view.Modify();
        view.GetStringUserProperties(out Dictionary<string, string> viewType);
        DrawingMethods.GetViewTypeEnum(viewType);
        InfoBox.OnInfo(infoBox, $"Shifting Down=v\n{(GaViewType)DrawingMethods.GetViewTypeEnum(viewType)}");
        DrawingHandler.GetActiveDrawing().CommitChanges("Shift View Down");
    }

    private void ShiftViewLeft(int amount)
    {
        var view = GetValidViewInActiveDrawing();
        if (view == null) return;
        view.Origin.X -= amount;
        view.Modify();
        view.GetStringUserProperties(out Dictionary<string, string> viewType);
        DrawingMethods.GetViewTypeEnum(viewType);
        InfoBox.OnInfo(infoBox, $"{(GaViewType)DrawingMethods.GetViewTypeEnum(viewType)} <= Shifting Left");
        DrawingHandler.GetActiveDrawing().CommitChanges("Shift View Left");
    }
}

public static class InfoBox
{
    private static readonly Color BackColorDefault = Color.FromArgb(40, 57, 56);
    private static readonly Color BackColorOnDisplay = Color.Honeydew;
    private static readonly Color ForeColorOnDisplay = Color.Black;
    private const BorderStyle BorderStyleOnDisplay = System.Windows.Forms.BorderStyle.Fixed3D;
    private const BorderStyle BorderStyleOnDefault = System.Windows.Forms.BorderStyle.None;

    public static void OnError(Label errorBox, Exception ex)
    {
        Console.WriteLine(ex);
        errorBox.BackColor = BackColorOnDisplay;
        errorBox.ForeColor = ForeColorOnDisplay;
        errorBox.BorderStyle = BorderStyleOnDisplay;
        // ReSharper disable once LocalizableElement
        errorBox.Text = $"{ex.Message}\n\n{ex}";
    }

    public static void ToDefault(Label errorBox)
    {
        errorBox.Text = string.Empty;
        errorBox.BorderStyle = BorderStyleOnDefault;
        errorBox.BackColor = BackColorDefault;
    }

    public static void OnInfo(Label errorBox, string info)
    {
        errorBox.BorderStyle = BorderStyleOnDisplay;
        errorBox.BackColor = BackColorOnDisplay;
        errorBox.ForeColor = ForeColorOnDisplay;
        errorBox.Text = info;
    }
}