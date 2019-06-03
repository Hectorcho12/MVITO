using MVITO.Clases;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Monitoreo.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Page5 : Page
    {
        private Connection sql = new Connection();
        private string range = "";
        DataSet ds = new DataSet();
        private string fch;
        public Page5()
        {
            this.InitializeComponent();
        }

        private void Load()
        {



            string instruccion;

            instruccion = string.Format("select sum(total ) as TotalDeHoy from Compras where fecha  like '%{0}%'", range);
            ds = sql.Conexion(instruccion);
            if (ds.Tables[0].Rows[0]["TotalDeHoy"].ToString().Length > 0)
                Compras.Text = "Lps: " + Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalDeHoy"].ToString()).ToString("#,###,###.00");
            else Compras.Text = "Lps: 0.0";

            instruccion = string.Format("select sum(total ) as TotalDeHoy from Gastos where fecha  like '%{0}%'", range);
            ds = sql.Conexion(instruccion);
            if (ds.Tables[0].Rows[0]["TotalDeHoy"].ToString().Length > 0)
                Gastos.Text = "Lps: " + Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalDeHoy"].ToString()).ToString("#,###,###.00");
            else Gastos.Text = "Lps: 0.0";

            instruccion = string.Format("select sum(total ) as TotalDeHoy from Perdidas where fecha  like '%{0}%'", range);
            ds = sql.Conexion(instruccion);
            if (ds.Tables[0].Rows[0]["TotalDeHoy"].ToString().Length > 0)
                Perdidas.Text = "Lps: " + Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalDeHoy"].ToString()).ToString("#,###,###.00");
            else Perdidas.Text = "Lps: 0.0";

        }

        private async void CrearExcel(object sender, RoutedEventArgs e)
        {
            ds = sql.Conexion(string.Format("select [compra],[descripcion],[fecha],[total],[isv] from Compras where Fecha like '%{0}%'", range));


            //Create an instance of ExcelEngine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Set the default application version as Excel 2016.
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2016;

                //Create a workbook with a worksheet.
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(3);

                //Access first worksheet from the workbook instance.
                IWorksheet worksheet = workbook.Worksheets[0];

                //Insert sample text into cell “A1”.
                worksheet.Name = "Compras";
                worksheet.Range["A1:E1"].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                worksheet.Range["A1:E1"].CellStyle.Font.Color = ExcelKnownColors.White;
                worksheet.Range["A1:C1"].ColumnWidth = 50;
                worksheet.Range["D1:E1"].ColumnWidth = 25;
                worksheet.Range["A1"].Text = "COMPRA";
                worksheet.Range["B1"].Text = "DESCRIPCION DE LA COMPRA";
                worksheet.Range["C1"].Text = "FECHA";
                worksheet.Range["D1"].Text = "TOTAL";
                worksheet.Range["E1"].Text = "ISV";

                foreach (DataTable table in ds.Tables)
                {
                    int count = table.Rows.Count;
                    for (int j = 0; j < count; j++)
                    {
                        worksheet.Range[string.Format("A{0}", j + 2)].Text = table.Rows[j].ItemArray[0].ToString();
                        worksheet.Range[string.Format("B{0}", j + 2)].Text = table.Rows[j].ItemArray[1].ToString();
                        worksheet.Range[string.Format("C{0}", j + 2)].Text = table.Rows[j].ItemArray[2].ToString();
                        worksheet.Range[string.Format("D{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[3].ToString());
                        worksheet.Range[string.Format("E{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[4].ToString());
                    }

                    //Cuento el total de filas
                    int totalfilas = worksheet.Rows.Count();

                    //les da formato a la fila de "Totales" y las que contendran la sumas del dinero y del isv
                    worksheet.Range[string.Format("C{0}", totalfilas + 1)].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                    worksheet.Range[string.Format("C{0}", totalfilas + 1)].CellStyle.Font.Color = ExcelKnownColors.White;
                    worksheet.Range[string.Format("D{0}", totalfilas + 1)].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                    worksheet.Range[string.Format("D{0}", totalfilas + 1)].CellStyle.Font.Color = ExcelKnownColors.White;
                    worksheet.Range[string.Format("E{0}", totalfilas + 1)].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                    worksheet.Range[string.Format("E{0}", totalfilas + 1)].CellStyle.Font.Color = ExcelKnownColors.White;

                    //establece los valores en las filas (el dinero, isv y la palabra totales)
                    worksheet.Range[string.Format("C{0}", totalfilas + 1)].Text = "Totales";
                    worksheet.Range[string.Format("D{0}", totalfilas + 1)].Formula = "SUM(D2:D" + totalfilas + ")";
                    worksheet.Range[string.Format("E{0}", totalfilas + 1)].Formula = "SUM(E2:E" + totalfilas + ")";

                    //IChartShape chart = worksheet.Charts.Add();
                    //chart.ChartType = ExcelChartType.Column_Clustered;
                    //chart.DataRange = worksheet.Range["A:A"];
                    //chart.DataRange = worksheet.Range["E:E"];




                }

                ds = sql.Conexion(string.Format("select [gasto],[descripcion],[fecha],[total] from Gastos where Fecha like '%{0}%'", range));
                worksheet = workbook.Worksheets[1];
                worksheet.Name = "GASTOS";
                worksheet.Range["A1:D1"].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                worksheet.Range["A1:D1"].CellStyle.Font.Color = ExcelKnownColors.White;
                worksheet.Range["A1:B1"].ColumnWidth = 50;
                worksheet.Range["C1:D1"].ColumnWidth = 25;
                worksheet.Range["A1"].Text = "GASTO";
                worksheet.Range["B1"].Text = "DESCRIPCION DE GASTO";
                worksheet.Range["C1"].Text = "FECHA";
                worksheet.Range["D1"].Text = "TOTAL PAGADO";
                foreach (DataTable table in ds.Tables)
                {
                    int count = table.Rows.Count;
                    for (int j = 0; j < count; j++)
                    {
                        worksheet.Range[string.Format("A{0}", j + 2)].Text = table.Rows[j].ItemArray[0].ToString();
                        worksheet.Range[string.Format("B{0}", j + 2)].Text = table.Rows[j].ItemArray[1].ToString();
                        worksheet.Range[string.Format("C{0}", j + 2)].Text = table.Rows[j].ItemArray[2].ToString();
                        worksheet.Range[string.Format("D{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[3].ToString());

                        //Cuento el total de filas
                        int totalfilas = worksheet.Rows.Count();

                        //les da formato a la fila de "Totales" y las que contendran la sumas del dinero y del isv

                        worksheet.Range[string.Format("C{0}", totalfilas + 1)].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                        worksheet.Range[string.Format("C{0}", totalfilas + 1)].CellStyle.Font.Color = ExcelKnownColors.White;
                        worksheet.Range[string.Format("D{0}", totalfilas + 1)].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                        worksheet.Range[string.Format("D{0}", totalfilas + 1)].CellStyle.Font.Color = ExcelKnownColors.White;

                        //establece los valores en las filas (el dinero, isv y la palabra totales)
                        worksheet.Range[string.Format("C{0}", totalfilas + 1)].Text = "Totales";
                        worksheet.Range[string.Format("D{0}", totalfilas + 1)].Formula = "SUM(D2:D" + totalfilas + ")";

                    }
                }

                ds = sql.Conexion(string.Format("select [perdida],[descripcion],[fecha],[total] from Perdidas where Fecha like '%{0}%'", range));
                worksheet = workbook.Worksheets[2];
                worksheet.Name = "PERDIDA";
                worksheet.Range["A1:D1"].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                worksheet.Range["A1:D1"].CellStyle.Font.Color = ExcelKnownColors.White;
                worksheet.Range["A1:C1"].ColumnWidth = 50;
                worksheet.Range["D1"].ColumnWidth = 25;
                worksheet.Range["A1"].Text = "PERDIDA";
                worksheet.Range["B1"].Text = "DESCRIPCION DE GASTO";
                worksheet.Range["C1"].Text = "FECHA";
                worksheet.Range["D1"].Text = "TOTAL PAGADO";
                foreach (DataTable table in ds.Tables)
                {
                    int count = table.Rows.Count;
                    for (int j = 0; j < count; j++)
                    {
                        worksheet.Range[string.Format("A{0}", j + 2)].Text = table.Rows[j].ItemArray[0].ToString();
                        worksheet.Range[string.Format("B{0}", j + 2)].Text = table.Rows[j].ItemArray[1].ToString();
                        worksheet.Range[string.Format("C{0}", j + 2)].Text = table.Rows[j].ItemArray[2].ToString();
                        worksheet.Range[string.Format("D{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[3].ToString());


                        //Cuento el total de filas
                        int totalfilas = worksheet.Rows.Count();

                        //les da formato a la fila de "Totales" y las que contendran la sumas del dinero y del isv

                        worksheet.Range[string.Format("C{0}", totalfilas + 1)].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                        worksheet.Range[string.Format("C{0}", totalfilas + 1)].CellStyle.Font.Color = ExcelKnownColors.White;
                        worksheet.Range[string.Format("D{0}", totalfilas + 1)].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                        worksheet.Range[string.Format("D{0}", totalfilas + 1)].CellStyle.Font.Color = ExcelKnownColors.White;

                        //establece los valores en las filas (el dinero, isv y la palabra totales)
                        worksheet.Range[string.Format("C{0}", totalfilas + 1)].Text = "Totales";
                        worksheet.Range[string.Format("D{0}", totalfilas + 1)].Formula = "SUM(D2:D" + totalfilas + ")";

                    }
                }



                // Seleciona donde guardara el archivo
                StorageFile storageFile;
                if (!(Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")))
                {
                    string fechita = DateTime.Today.ToString("dd MM yy");
                    FileSavePicker savePicker = new FileSavePicker();
                    savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
                    savePicker.SuggestedFileName = "Reporte " + fechita;
                    savePicker.FileTypeChoices.Add("Excel Files", new List<string>() { ".xlsx" });
                    storageFile = await savePicker.PickSaveFileAsync();

                    if (storageFile != null)
                    {
                        await workbook.SaveAsAsync(storageFile); // GUARDA EL ARCHIVO EXCELL
                        await Windows.System.Launcher.LaunchFileAsync(storageFile); // ABRE EXCEL
                    }
                }
            }
        }



        private void RangeValue(object sender, SelectionChangedEventArgs e)
        {
            switch (e.AddedItems[0].ToString())
            {
                case "Diario":
                    mes.Visibility = Visibility.Collapsed;
                    año.Visibility = Visibility.Collapsed;
                    calcular.Visibility = Visibility.Collapsed;
                    range = DateTime.Today.ToString("dddd, dd MMMM yyyy");
                    Load();
                    ExcelButton.IsEnabled = true;
                    break;
                case "Mensual":
                    mes.Visibility = Visibility.Visible;
                    año.Visibility = Visibility.Visible;
                    año.SelectedDate = DateTime.Today;
                    calcular.Visibility = Visibility.Visible;
                    //range = DateTime.Today.ToString("MMMM yyyy");

                    ExcelButton.IsEnabled = true;
                    break;
                case "Anual":
                    mes.Visibility = Visibility.Collapsed;
                    año.Visibility = Visibility.Visible;
                    calcular.Visibility = Visibility.Visible;
                    Calculo.Visibility = Visibility.Visible;
                    mes.Visibility = Visibility.Collapsed;
                    range = DateTime.Today.ToString("yyyy");

                    ExcelButton.IsEnabled = true;
                    break;
            }
        }

        //Evento del boton calcular para Calcular los gastos, perdidas y compras en cada textbox
        private void Calcular_Click(object sender, RoutedEventArgs e)
        {

            if (Convert.ToString(r.SelectedItem) == "Mensual")
            {
                range = Convert.ToString(mes.SelectedItem + " " + año.Date.Year.ToString());
                Load();
            }

            if (Convert.ToString(r.SelectedItem) == "Anual")
            {
                range = Convert.ToString(año.Date.Year.ToString());
                Load();
            }


        }

        //Evento de validacion para habilitar boton calcular cuando se haya seleccionado un mes y un año

        private void Vcalcular()
        {
            if (Convert.ToString(r.SelectedItem) == "Mensual")
            {
                if (mes.SelectedItem != null)
                {

                    
                    calcular.IsEnabled = true;
                    


                }
            }
        }

            private void Mes_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                Vcalcular();
            }
        }
    }



