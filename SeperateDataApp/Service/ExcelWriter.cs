using SeperateDataApp.Service.Logger;
using System;
using System.Collections.Generic;

namespace SeperateDataApp.Service
{
    class ExcelWriter
    {
        private readonly LogHelper logHelper;

        public ExcelWriter()
        {
            logHelper = new(this);
        }

        public void WriteToFile(string excelPathToSave, string sheetName, List<List<object>> headers, List<List<object>> bodyData)
        {
            Microsoft.Office.Interop.Excel._Workbook oWB = null;
            try
            {
                Microsoft.Office.Interop.Excel.Application oXL = new();
                oWB = oXL.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet oSheet = oWB.Sheets.Add();

                oSheet.Name = sheetName.Length >= 31 ? sheetName.Substring(0, 30) : sheetName;

                SaveDataToCells(oSheet.Cells, headers, bodyData);

                oWB.SaveAs(excelPathToSave);

                logHelper.Info($"Export done for sheet {sheetName} to path: {excelPathToSave}!");
            }
            catch (Exception ex)
            {
                logHelper.Error(ex);
            }
            finally
            {
                if (oWB != null)
                {
                    oWB.Close();
                }
            }
        }

        private static void SaveDataToCells(Microsoft.Office.Interop.Excel.Range rangeExcelToSave, List<List<object>> headers, List<List<object>> bodyData)
        {
            int row = 1;
            int col = 1;

            /// SAVE HEADERs
            foreach (List<object> header in headers)
            {
                foreach (object iHeader in header)
                {
                    rangeExcelToSave[row, col] = iHeader;
                    col += 1;
                }
                col = 1;
                row += 1;
            }

            /// SAVE BODY DATA
            foreach (List<object> rowData in bodyData)
            {
                foreach (object cellData in rowData)
                {
                    rangeExcelToSave[row, col] = cellData;
                    col += 1;
                }
                col = 1;
                row += 1;
            }
        }
    }
}
