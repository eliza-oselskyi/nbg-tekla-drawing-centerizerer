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
using System.IO;
using System.Threading;
using Tekla.Structures;
using Tekla.Structures.Model;

namespace Drawing.CenterView
{
    public abstract partial class QuickCenterClass
    {
    }
}

namespace Drawing.CenterView
{
    public static class Reports
    {
        public static void GenerateAndDisplayReport(string reportTemplate, string reportString)
        {
            Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Generating Report...");

            var reportsFolder = QuickCenterClass.Model1.GetInfo().ModelPath + @"\Reports\";
            var firmFolder = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_FIRM", ref firmFolder);
            var reportsTemplateFolder = Path.Combine(firmFolder, @"Reports\");
            if (!File.Exists(reportsTemplateFolder + reportTemplate + ".rpt"))
                File.Copy($@"{firmFolder}\Macros\modeling\Center_Drawings\Release\report_template\{reportTemplate}.rpt",
                    $@"{reportsTemplateFolder}\{reportTemplate}.rpt");

            Directory.CreateDirectory(reportsFolder);
            if (File.Exists(reportsFolder + reportTemplate + ".xsr"))
                File.Delete(reportsFolder + reportTemplate + ".xsr");

            var epochTimeOffset = (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalSeconds;
            var dateTime = (int)DateTimeOffset.Now.ToUnixTimeSeconds() + epochTimeOffset;

            Tekla.Structures.Model.Operations.Operation.CreateReportFromAll(reportTemplate,
                reportTemplate + ".xsr",
                dateTime.ToString(),
                "",
                "");

            Thread.Sleep(0);
            if (!File.Exists(reportsFolder + reportTemplate + ".xsr")) return;
            if (IfLockedWait(reportsFolder + reportTemplate + ".xsr"))
                Tekla.Structures.Model.Operations.Operation.DisplayReport(reportsFolder + reportTemplate + ".xsr");
        }

        private static bool IfLockedWait(string fileName)
        {
            // try 10 times
            var retryNumber = 10;
            while (true)
                try
                {
                    using var fileStream = new FileStream(
                        fileName, FileMode.Open,
                        FileAccess.ReadWrite, FileShare.ReadWrite);
                    var readText = new byte[fileStream.Length];
                    fileStream.Seek(0, SeekOrigin.Begin);
                    QuickCenterClass.Read = fileStream.Read(readText, 0, (int)fileStream.Length);

                    return true;
                }
                catch (IOException)
                {
                    // wait one second
                    Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Retrying Report Generation...");
                    Thread.Sleep(1000);
                    retryNumber--;
                    if (retryNumber == 0)
                        return false;
                }
        }
    }
}