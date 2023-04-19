using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using System.IO;
using System.Data;

namespace HNAssistant
{
    /// <summary>该类为使用参考类
    /// 完全复制该类，后续根据需求添加
    /// 命名空间：
    /// using System.IO;
    /// using System.Data;
    /// using NPOI.HSSF.UserModel;
    /// using NPOI.SS.UserModel;
    /// using NPOI.XSSF.UserModel;
    /// using NPOI.SS.Util;
    /// 引用：
    /// ICSharpCode.SharpZipLib
    /// NPOI
    /// NPOI.OOXML
    /// NPOI.OpenXml4Net
    /// NPOI.OpenXmlFormats
    /// dll：
    /// ICSharpCode.SharpZipLib.dll
    /// NPOI.dll
    /// NPOI.OOXML.dll
    /// NPOI.OpenXml4Net.dll
    /// NPOI.OpenXmlFormats.dll
    /// </summary>
    public class ExcelAssistant
    {
        #region base

        /*常用函数说明
         * IWorkbook:  
                该接口用于操作excel工作簿的一个接口,主要有两个实现
                    HSSFWorkbook : 用于读取excel2007版本以下的xls文件
                    XSSFWorkbook : 用于读取.xlsx 文件		
                主要方法：
                    CreateSheet()       : 创建一个 工作表
                    GetSheetAt(index)   : 根据索引获取工作表对象
                    GetSheet(sheetName) : 根据名称获取工作表对象
                    CreateCellStyle()   : 创建单元格样式
                    CreateFont()        : 创建字体样式
            ISheet :
                excel工作表对象
                主要属性：
                    LastRowNum ： 最后一行的索引
                    FirstRowNum： 第一行的索引
                主要方法:
                    GetRow(int index) : 根据索引获取一行
                    CreateRow(int index) : 创建一个数据行
            IRow :
                数据行对象
                主要方法
                    CreateCell(int index) : 指定索引创建一个单元格
                    GetCell(int index) : 获取单元格
            ICell
                单元格对象
                主要方法
                    SetCellValue(string value) : 设置单元格的值
                    ToString()  : 获取该单元格填充的内容
         */

        /// <summary>将DataTable数据写入指定路径
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="targetPath"></param>
        public static void DataTableToExcel(DataTable dt, string targetPath)
        {
            try
            {
                //1、创建新的工作簿实例
                IWorkbook workbook = WorkBookFactory.Create(targetPath);
                //2、工作表
                string sheetname = dt.TableName;
                if (sheetname == "" || sheetname == null) sheetname = "DataTable";
                ISheet sheet = workbook.CreateSheet(sheetname);
                //3、设置表头
                IRow head = sheet.CreateRow(0);
                ICell cell;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    cell = head.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }
                //4、填充数据
                IRow rowData;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    rowData = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = rowData.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }
                //5、保存数据
                using (FileStream stream = File.Create(targetPath, 1024 * 1024))
                {
                    workbook.Write(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>将指定文件的第一张表读取转换为DataTable
        /// 
        /// </summary>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string targetPath)
        {
            try
            {
                DataTable dt = new DataTable();
                //1、创建实例
                IWorkbook workbook = WorkBookFactory.GetWorkBook(targetPath);
                //2、选取第一张数据表
                ISheet sheet = workbook.GetSheetAt(0);
                //3、获取有效行列数据，以第一行存在的数据为表头，其余部分不计入数据
                int lastRow = sheet.LastRowNum;
                if (lastRow == 0) return new DataTable();
                int headRow = sheet.FirstRowNum;
                IRow head = sheet.GetRow(headRow);
                int firstColumnNum = head.FirstCellNum;
                int lastColumnNum = head.LastCellNum;
                //4、创建表头
                int index = 0;
                for (int i = firstColumnNum; i < lastColumnNum; i++)
                {
                    string columnName = head.Cells[index].ToString();
                    dt.Columns.Add(columnName);
                    index++;
                }
                //5、填充数据
                IRow dataRow;
                for (int i = headRow + 1; i < lastRow + 1; i++)
                {
                    dataRow = sheet.GetRow(i);
                    if (dataRow.Cells.Count > dt.Columns.Count) throw new Exception("数据格式错误！");
                    DataRow dr = dt.NewRow();
                    index = 0;
                    foreach (ICell cell in dataRow.Cells)
                    {
                        dr[index] = cell.ToString();
                        index++;
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>根据表名或表索引，获取该表值
        /// 如果sheetName不为空，以sheetName为准，否则使用sheetIndex
        /// </summary>
        /// <param name="targetPath"></param>
        /// <param name="sheetName"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string targetPath, string sheetName, int sheetIndex)
        {
            try
            {
                DataTable dt = new DataTable();
                //1、创建实例
                IWorkbook workbook = WorkBookFactory.GetWorkBook(targetPath);
                //2、选取第一张数据表
                ISheet sheet;
                if (sheetName != "" && sheetName != null)
                    sheet = workbook.GetSheet(sheetName);
                else
                    sheet = workbook.GetSheetAt(sheetIndex);
                //3、获取有效行列数据，以第一行存在的数据为表头，其余部分不计入数据
                int lastRow = sheet.LastRowNum;
                if (lastRow == 0) return new DataTable();
                int headRow = sheet.FirstRowNum;
                IRow head = sheet.GetRow(headRow);
                int firstColumnNum = head.FirstCellNum;
                int lastColumnNum = head.LastCellNum;
                //4、创建表头
                int index = 0;
                for (int i = firstColumnNum; i < lastColumnNum; i++)
                {
                    string columnName = head.Cells[index].ToString();
                    dt.Columns.Add(columnName);
                    index++;
                }
                //5、填充数据
                IRow dataRow;
                for (int i = headRow + 1; i < lastRow + 1; i++)
                {
                    dataRow = sheet.GetRow(i);
                    if (dataRow.Cells.Count > dt.Columns.Count) throw new Exception("数据格式错误！");
                    DataRow dr = dt.NewRow();
                    index = 0;
                    foreach (ICell cell in dataRow.Cells)
                    {
                        dr[index] = cell.ToString();
                        index++;
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataSet ExcelToDataSet()
        {
            try
            {
                DataSet ds = new DataSet();

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public object GetCellValue(ICell cell)
        {
            object value = null;
            if (cell == null) return value;
            if (cell.CellType == CellType.Blank)
            {
                return value;
            }
            else if (cell.CellType == CellType.Boolean)
            {
                value = cell.BooleanCellValue;
            }
            else if (cell.CellType == CellType.Error)
            {
                value = cell.ErrorCellValue;
            }
            else if (cell.CellType == CellType.Formula)
            {
                value = cell.CellFormula;
            }
            else if (cell.CellType == CellType.Numeric)
            {
                if (DateUtil.IsCellDateFormatted(cell))
                {
                    value = cell.DateCellValue;
                }
                else
                {
                    value = cell.NumericCellValue;
                }
            }
            else if (cell.CellType == CellType.String)
            {
                value = cell.StringCellValue;
            }
            else if (cell.CellType == CellType.Unknown)
            {
                value = cell.StringCellValue;
            }
            else
            {
                value = cell.StringCellValue;
            }
            return value;
        }

        public static void SetRowValue(IRow row, object[] values, int startIndex = 0)
        {
            int index = 0;
            foreach (object value in values)
            {
                ICell cell = row.CreateCell(index + startIndex);
                SetCellValue(cell, value == null ? "" : value);
                index++;
            }
        }

        public static void SetCellValue(ICell cell, object obj)
        {
            if (obj.GetType() == typeof(int))
            {
                cell.SetCellValue((int)obj);
            }
            else if (obj.GetType() == typeof(double))
            {
                cell.SetCellValue((double)obj);
            }
            else if (obj.GetType() == typeof(IRichTextString))
            {
                cell.SetCellValue((IRichTextString)obj);
            }
            else if (obj.GetType() == typeof(string))
            {
                cell.SetCellValue(obj.ToString());
            }
            else if (obj.GetType() == typeof(DateTime))
            {
                cell.SetCellValue((DateTime)obj);
            }
            else if (obj.GetType() == typeof(bool))
            {
                cell.SetCellValue((bool)obj);
            }
            else
            {
                cell.SetCellValue(obj.ToString());
            }
        }

        #region
        /// <summary>工作薄工厂类,根据类型创建IWorkbook的实例
        /// 
        /// </summary>
        public class WorkBookFactory
        {
            /// <summary>根据指定类型，创建新的对象
            /// 创建一个工作簿对象
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static IWorkbook Create(WorkBookType type)
            {
                IWorkbook workbook = null;
                switch (type)
                {
                    case WorkBookType.xls:
                        workbook = new HSSFWorkbook();
                        break;
                    case WorkBookType.xlsx:
                        workbook = new XSSFWorkbook();
                        break;
                    default:
                        break;
                }
                return workbook;
            }

            /// <summary>通过指定存储路径的后缀，创建新的工作表实例
            /// 
            /// </summary>
            /// <returns></returns>
            public static IWorkbook Create(string targetPath)
            {
                string ext = Path.GetExtension(targetPath).ToLower();
                IWorkbook workbook = null;
                if (ext == ".xls")
                    workbook = new HSSFWorkbook();
                else if (ext == ".xlsx")
                    workbook = new XSSFWorkbook();
                else
                    throw new Exception(string.Format("路径：{0}\r\n后缀非《.xls》《.xlsx》Excel后缀", targetPath));
                return workbook;
            }

            /// <summary>创建现有文件对象
            /// 根据文件名获取工作簿对象
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static IWorkbook GetWorkBook(string file)
            {
                if (File.Exists(file) == false)
                    throw new Exception(string.Format("文件：{0} \r\n不存在", file));
                string ext = Path.GetExtension(file).ToLower();
                if (ext != ".xls" && ext != ".xlsx")
                {
                    throw new Exception("excel文件格式不正确");
                }
                using (FileStream readStram = File.OpenRead(file))
                {
                    IWorkbook workbook = null;
                    if (ext == ".xls")
                    {
                        workbook = new HSSFWorkbook(readStram);
                    }
                    else
                    {
                        workbook = new XSSFWorkbook(readStram);
                    }
                    return workbook;
                }
            }

        }
        /// <summary>工作薄类型
        /// 
        /// </summary>
        public enum WorkBookType
        {
            /// <summary>
            /// xls
            /// </summary>
            xls,
            /// <summary>
            /// xlsx
            /// </summary>
            xlsx
        }
        #endregion

        #endregion

        #region PAST
        public class x2003
        {
            /// <summary>
            /// 将Excel文件中的数据读出到DataTable中(xls)
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static DataTable ExcelToTableForXLS(string file)
            {
                DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                    ISheet sheet = hssfworkbook.GetSheetAt(0);

                    //表头
                    IRow header = sheet.GetRow(sheet.FirstRowNum);
                    List<int> columns = new List<int>();
                    for (int i = 0; i < header.LastCellNum; i++)
                    {
                        object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                        if (obj == null || obj.ToString() == string.Empty)
                        {
                            dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                            //continue;
                        }
                        else
                            dt.Columns.Add(new DataColumn(obj.ToString()));
                        columns.Add(i);
                    }
                    //数据
                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        foreach (int j in columns)
                        {
                            dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
                            if (dr[j] != null && dr[j].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }
                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt;
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xls)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void TableToExcelForXLS(DataTable dt, string file)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                ISheet sheet = hssfworkbook.CreateSheet("Test");

                //表头
                IRow row = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = row.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }

                //数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                hssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 获取单元格类型(xls)
            /// </summary>
            /// <param name="cell"></param>
            /// <returns></returns>
            private static object GetValueTypeForXLS(HSSFCell cell)
            {
                if (cell == null)
                    return null;
                switch (cell.CellType)
                {
                    case CellType.Blank: //BLANK:
                        return null;
                    case CellType.Boolean: //BOOLEAN:
                        return cell.BooleanCellValue;
                    case CellType.Numeric: //NUMERIC:
                        return cell.NumericCellValue;
                    case CellType.String: //STRING:
                        return cell.StringCellValue;
                    case CellType.Error: //ERROR:
                        return cell.ErrorCellValue;
                    case CellType.Formula: //FORMULA:
                    default:
                        return "=" + cell.CellFormula;
                }
            }
        }

        /// <summary>XSS
        /// XSSFWorkbook：一个Excel表格
        /// ISheet      ：一个工作表
        /// </summary>
        public class x2007
        {
            /// <summary>
            /// 将Excel文件中的数据读出到DataTable中(xlsx)
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static DataTable ExcelToTableForXLSX(string file)
            {
                DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                    ISheet sheet = xssfworkbook.GetSheetAt(0);

                    //表头
                    IRow header = sheet.GetRow(sheet.FirstRowNum);
                    List<int> columns = new List<int>();
                    for (int i = 0; i < header.LastCellNum; i++)
                    {
                        object obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
                        if (obj == null || obj.ToString() == string.Empty)
                        {
                            dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                            //continue;
                        }
                        else
                            dt.Columns.Add(new DataColumn(obj.ToString()));
                        columns.Add(i);
                    }
                    //数据
                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        foreach (int j in columns)
                        {
                            dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
                            if (dr[j] != null && dr[j].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }
                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt;
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xlsx)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void TableToExcelForXLSX(DataTable dt, string file)
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook();
                ISheet sheet = xssfworkbook.CreateSheet("Test");

                //表头
                IRow row = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = row.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }

                //数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 获取单元格类型(xlsx)
            /// </summary>
            /// <param name="cell"></param>
            /// <returns></returns>
            private static object GetValueTypeForXLSX(XSSFCell cell)
            {
                if (cell == null)
                    return null;
                switch (cell.CellType)
                {
                    case CellType.Blank: //BLANK:
                        return null;
                    case CellType.Boolean: //BOOLEAN:
                        return cell.BooleanCellValue;
                    case CellType.Numeric: //NUMERIC:
                        return cell.NumericCellValue;
                    case CellType.String: //STRING:
                        return cell.StringCellValue;
                    case CellType.Error: //ERROR:
                        return cell.ErrorCellValue;
                    case CellType.Formula: //FORMULA:
                    default:
                        return "=" + cell.CellFormula;
                }
            }
        }

        public static DataTable GetDataTable(string filepath)
        {
            DataTable dt;
            if (filepath.Last() == 's') dt = new DataTable("xls");
            else dt = new DataTable("xlsx");
            if (filepath.Last() == 's')
            {
                dt = x2003.ExcelToTableForXLS(filepath);
            }
            else
            {
                dt = x2007.ExcelToTableForXLSX(filepath);
            }
            return dt;
        }
        #endregion

        #region Demo

        /// <summary>演示1：设置模板，并写入数据
        /// 
        /// </summary>
        /// <param name="targetPath"></param>
        public static void Demonstration1(string targetPath)
        {
            //1、实例化新的工作簿
            IWorkbook workbook = WorkBookFactory.Create(targetPath);
            //2、实例化新的工作表
            ISheet sheet = workbook.CreateSheet("100G_COB耦合");
            //3、新建表头
            Dictionary<int, string> rowValueDic = new Dictionary<int, string>();
            //3.1、第1行 
            IRow rowhead = sheet.CreateRow(0);
            #region 第1行设置
            //单元格赋值
            ICell cellhead = rowhead.CreateCell(0);
            SetCellValue(cellhead, "100G COB Data sheet for Process ");
            //单元格合并
            CellRangeAddress region = new CellRangeAddress(0, 0, 0, 28);
            sheet.AddMergedRegion(region);
            //样式
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;       //水平对齐
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直对齐
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
            //字体
            IFont font = workbook.CreateFont();
            font.FontName = "楷体";
            font.Color = NPOI.HSSF.Util.HSSFColor.Red.Index;
            font.Boldweight = (short)FontBoldWeight.Normal;
            style.SetFont(font);
            cellhead.CellStyle = style;
            //行高
            rowhead.Height = 2 * 256;//一个字符高度是256个单位
            #endregion
            //3.2、第2行
            rowhead = sheet.CreateRow(1);
            #region 第2行设置
            //设置值(Excel对应位置减1)
            rowValueDic.Clear();
            rowValueDic.Add(0, "SN");
            rowValueDic.Add(1, "ID");
            rowValueDic.Add(2, "CH");
            rowValueDic.Add(3, "汇聚耦合");
            rowValueDic.Add(11, "平行耦合");
            rowValueDic.Add(19, "组装后");
            //单元格循环赋值
            foreach (var item in rowValueDic)
            {
                //赋值
                cellhead = rowhead.CreateCell(item.Key);
                SetCellValue(cellhead, item.Value);
                //样式
                style = workbook.CreateCellStyle();
                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;       //水平对齐
                style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直对齐
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cellhead.CellStyle = style;
            }
            //单元格合并
            region = new CellRangeAddress(1, 1, 3, 10);
            sheet.AddMergedRegion(region);
            region = new CellRangeAddress(1, 1, 11, 18);
            sheet.AddMergedRegion(region);
            region = new CellRangeAddress(1, 1, 19, 28);
            sheet.AddMergedRegion(region);
            #endregion
            //3.3、第3行
            rowhead = sheet.CreateRow(2);
            #region 第3行设置
            //设置值(Excel对应位置减1)
            rowValueDic.Clear();
            rowValueDic.Add(3, "Ptx（dBm）");
            rowValueDic.Add(4, "Ibias(mA)");
            rowValueDic.Add(5, "Temp(℃)");
            rowValueDic.Add(6, "mPD");
            rowValueDic.Add(7, "Prx（dBm）");
            rowValueDic.Add(8, "IPD(uA)");
            rowValueDic.Add(9, "Re(uA/uW)");
            rowValueDic.Add(10, "日期");
            rowValueDic.Add(11, "Ptx（dBm）");
            rowValueDic.Add(12, "Ibias(mA)");
            rowValueDic.Add(13, "Temp(℃)");
            rowValueDic.Add(14, "mPD");
            rowValueDic.Add(15, "Prx（dBm）");
            rowValueDic.Add(16, "IPD(uA)");
            rowValueDic.Add(17, "Re(uA/uW)");
            rowValueDic.Add(18, "日期");
            rowValueDic.Add(19, "Ptx（dBm）");
            rowValueDic.Add(20, "Ibias(mA)");
            rowValueDic.Add(21, "Temp(℃)");
            rowValueDic.Add(22, "Mpd");
            rowValueDic.Add(23, "Prx（dBm）");
            rowValueDic.Add(24, "IPD(uA)");
            rowValueDic.Add(25, "Re(uA/uW)");
            rowValueDic.Add(26, "暗电流");
            rowValueDic.Add(27, "result");
            rowValueDic.Add(28, "日期");
            rowValueDic.Add(29, "跳线编号");
            rowValueDic.Add(30, "LENS型号");
            rowValueDic.Add(31, "LENS批次号");
            rowValueDic.Add(32, "LENS日期");
            rowValueDic.Add(33, "VTEC");
            rowValueDic.Add(34, "ITEC");
            rowValueDic.Add(35, "VCC");
            //单元格循环赋值
            foreach (var item in rowValueDic)
            {
                //赋值
                cellhead = rowhead.CreateCell(item.Key);
                SetCellValue(cellhead, item.Value);
                //样式
                style = workbook.CreateCellStyle();
                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;       //水平对齐
                style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直对齐
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cellhead.CellStyle = style;
            }
            #endregion
            //单元格合并
            region = new CellRangeAddress(1, 2, 0, 0);
            sheet.AddMergedRegion(region);
            region = new CellRangeAddress(1, 2, 1, 1);
            sheet.AddMergedRegion(region);
            region = new CellRangeAddress(1, 2, 2, 2);
            sheet.AddMergedRegion(region);
            for (int i = 0; i < 35; i++)
            {
                //自动列宽
                sheet.AutoSizeColumn(i);
            }

            //4保存数据
            using (FileStream stream = File.Create(targetPath))
            {
                workbook.Write(stream);
            }
        }

        /// <summary>
        /// ICellStyle只能由工作表通过函数CreateCellStyle()创建
        /// ICellStyle style=workbook.CreateCellStyle();
        /// </summary>
        /// <param name="style"></param>
        private void SetCellStyle(IWorkbook workbook, ICellStyle style)
        {
            //1、对齐方式
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;       //水平对齐
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直对齐
            //2、边框
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            //3、字符自动换行
            style.WrapText = true;
            //4、背景色
            style.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
            //5、字体色
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
            //6、填充模式
            style.FillPattern = FillPattern.SolidForeground;
            //7、设置字体样式
            IFont font = workbook.CreateFont();
            font.FontName = "楷体";//样式
            font.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;//字体颜色
            font.Boldweight = (short)FontBoldWeight.Normal;//加粗
            style.SetFont(font);

        }
        #endregion

        #region user code
        //public static void SetCOBLIVData(string targetPath, LIVExcelData data)
        //{
        //    //1、实例化新的工作簿
        //    IWorkbook workbook = WorkBookFactory.Create(targetPath);
        //    int ch = 0;
        //    //2、Result
        //    #region Result
        //    ISheet sheetResult = workbook.CreateSheet(string.Format("Result"));
        //    foreach (SingleData sd in data.DS)
        //    {
        //        int rowIndex = 5 * ch;
        //        //CH提示
        //        IRow row = sheetResult.CreateRow(rowIndex);
        //        ICell cell = row.CreateCell(0);
        //        SetCellValue(cell, string.Format("CH{0}", ch + 1));
        //        //单元格合并
        //        CellRangeAddress region = new CellRangeAddress(rowIndex, rowIndex, 0, 1);
        //        sheetResult.AddMergedRegion(region);
        //        //Ith
        //        row = sheetResult.CreateRow(rowIndex + 1);
        //        SetRowValue(row, new object[] { "Ith", sd.R.Ith });
        //        //SE
        //        row = sheetResult.CreateRow(rowIndex + 2);
        //        SetRowValue(row, new object[] { "SE", sd.R.SE });
        //        //Piop
        //        row = sheetResult.CreateRow(rowIndex + 3);
        //        SetRowValue(row, new object[] { "Piop", sd.R.Lop.P });

        //        ch++;
        //    }
        //    #endregion

        //    //3、Detail
        //    #region Detail
        //    ISheet sheetDetail = workbook.CreateSheet(string.Format("Detail"));
        //    ch = 0;
        //    foreach (SingleData sd in data.DS)
        //    {
        //        int rowIndex = 15 * ch;
        //        //CH提示
        //        IRow row = sheetDetail.CreateRow(rowIndex);
        //        ICell cell = row.CreateCell(0);
        //        SetCellValue(cell, string.Format("CH{0}", ch + 1));
        //        //单元格合并
        //        CellRangeAddress region = new CellRangeAddress(rowIndex, rowIndex, 0, 3);
        //        sheetDetail.AddMergedRegion(region);
        //        //Ith
        //        row = sheetDetail.CreateRow(rowIndex + 1);
        //        SetRowValue(row, new object[] { "Ith", sd.R.Ith });
        //        //SE
        //        row = sheetDetail.CreateRow(rowIndex + 2);
        //        SetRowValue(row, new object[] { "SE", sd.R.SE });
        //        //Piop
        //        row = sheetDetail.CreateRow(rowIndex + 3);
        //        SetRowValue(row, new object[] { "Piop", sd.R.Lop.P });
        //        //i1p1
        //        row = sheetDetail.CreateRow(rowIndex + 5);
        //        SetRowValue(row, new object[] { "I1", sd.R.L1.I, "P1", sd.R.L1.P });
        //        //i2p2
        //        row = sheetDetail.CreateRow(rowIndex + 6);
        //        SetRowValue(row, new object[] { "I2", sd.R.L2.I, "P2", sd.R.L2.P });
        //        //Ith
        //        row = sheetDetail.CreateRow(rowIndex + 7);
        //        SetRowValue(row, new object[] { "Ith", sd.R.Ith });
        //        //IC12
        //        row = sheetDetail.CreateRow(rowIndex + 8);
        //        SetRowValue(row, new object[] { "Ic1", data.P.Ic1, "Ic2", data.P.Ic2 });
        //        //Lop1
        //        row = sheetDetail.CreateRow(rowIndex + 9);
        //        SetRowValue(row, new object[] { "Iop1", sd.R.Lop1.I, "Pop1", sd.R.Lop1.P });
        //        //Lop2
        //        row = sheetDetail.CreateRow(rowIndex + 10);
        //        SetRowValue(row, new object[] { "Iop2", sd.R.Lop2.I, "Pop2", sd.R.Lop2.P });
        //        //SE
        //        row = sheetDetail.CreateRow(rowIndex + 11);
        //        SetRowValue(row, new object[] { "SE", sd.R.SE });
        //        //Ic
        //        row = sheetDetail.CreateRow(rowIndex + 12);
        //        SetRowValue(row, new object[] { "Ic", data.P.Ic });
        //        //Lop
        //        row = sheetDetail.CreateRow(rowIndex + 13);
        //        SetRowValue(row, new object[] { "Iop", sd.R.Lop.I, "Pop", sd.R.Lop.P });
        //        ch++;
        //    }
        //    #endregion

        //    //4、Chart
        //    #region Chart
        //    ISheet sheetChart = workbook.CreateSheet(string.Format("Chart"));
        //    //设置头部
        //    //提示
        //    IRow rowCH = sheetChart.CreateRow(0);
        //    SetRowValue(rowCH, new object[] { "CH1", "", "", "CH2", "", "", "CH3", "", "", "CH4" });
        //    IRow rowIP = sheetChart.CreateRow(1);
        //    SetRowValue(rowIP, new object[] { "I", "P", "", "I", "P", "", "I", "P", "", "I", "P" });

        //    List<int> Lengths = new List<int>();
        //    foreach (SingleData sd in data.DS)
        //    {
        //        Lengths.Add(sd.xys.Count);
        //    }
        //    int max = Lengths.Max();
        //    int index = 2;
        //    for (int i = 0; i < max; i++)
        //    {
        //        IRow row = sheetChart.CreateRow(index);
        //        object[] temp = GetIPTemp(data.DS, i);
        //        SetRowValue(row, temp);
        //        index++;
        //    }

        //    #endregion

        //    //5、Parameter
        //    #region Parameter
        //    ISheet sheetParameter = workbook.CreateSheet(string.Format("Parameter"));
        //    IRow rowp = sheetParameter.CreateRow(0);
        //    SetRowValue(rowp, new object[] { "I1", data.P.I1 });
        //    rowp = sheetParameter.CreateRow(1);
        //    SetRowValue(rowp, new object[] { "I2", data.P.I2 });
        //    rowp = sheetParameter.CreateRow(2);
        //    SetRowValue(rowp, new object[] { "Ic", data.P.Ic });
        //    rowp = sheetParameter.CreateRow(3);
        //    SetRowValue(rowp, new object[] { "Ic1", data.P.Ic1 });
        //    rowp = sheetParameter.CreateRow(4);
        //    SetRowValue(rowp, new object[] { "Ic2", data.P.Ic2 });
        //    #endregion

        //    //5、保存数据
        //    using (FileStream stream = File.Create(targetPath, 1024 * 1024))
        //    {
        //        workbook.Write(stream);
        //    }
        //}

        //private static object[] GetIPTemp(List<SingleData> DS, int index)
        //{
        //    object[] temp = new object[11];
        //    if (DS.Count > 0)
        //    {
        //        if (DS[0].xys.Count > index + 1)
        //        {
        //            temp[0] = DS[0].xys[index].I;
        //            temp[1] = DS[0].xys[index].P;
        //        }
        //    }
        //    if (DS.Count > 1)
        //    {
        //        if (DS[1].xys.Count > index + 1)
        //        {
        //            temp[3] = DS[1].xys[index].I;
        //            temp[4] = DS[1].xys[index].P;
        //        }
        //    }
        //    if (DS.Count > 2)
        //    {
        //        if (DS[2].xys.Count > index + 1)
        //        {
        //            temp[6] = DS[2].xys[index].I;
        //            temp[7] = DS[2].xys[index].P;
        //        }
        //    }
        //    if (DS.Count > 3)
        //    {
        //        if (DS[3].xys.Count > index + 1)
        //        {
        //            temp[9] = DS[3].xys[index].I;
        //            temp[10] = DS[3].xys[index].P;
        //        }
        //    }
        //    return temp;
        //}
        #endregion
    }
}
