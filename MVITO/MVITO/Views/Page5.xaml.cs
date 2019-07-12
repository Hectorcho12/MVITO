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
using MVITO.Extensiones;

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
        public DataSet ds = new DataSet();
        DataSet Tabla = new DataSet();
        private string fch;
        public string fchinicio;
        string desde;
        string fchfinal;






        public Page5()
        {
            this.InitializeComponent();

            Tabla = sql.Conexion(Convert.ToString("select IDemp, NombreEmp, SalarioEmp, PuestoEmp from Empleados"));
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



        private async void RangeValue(object sender, SelectionChangedEventArgs e)
        {
            switch (e.AddedItems[0].ToString())
            {
                case "Diario":
                    mes.Visibility = Visibility.Collapsed;
                    año.Visibility = Visibility.Collapsed;
                    calcular.Visibility = Visibility.Collapsed;
                    range = DateTime.Today.ToString("dddd, dd MMMM yyyy");
                    Load();

                    await Calculo.AnimateHeightAsync(0, 300, null);
                    ExcelButton.IsEnabled = true;
                    break;
                case "Mensual":
                    mes.Visibility = Visibility.Visible;
                    año.Visibility = Visibility.Visible;
                    año.SelectedDate = DateTime.Today;
                    mes.Visibility = Visibility.Visible;
                    calcular.Visibility = Visibility.Visible;
                    //range = DateTime.Today.ToString("MMMM yyyy");
                    await Calculo.AnimateHeightAsync(200, 300, null);

                    ExcelButton.IsEnabled = true;
                    break;
                case "Anual":

                    

                    mes.Visibility = Visibility.Collapsed;
                    año.Visibility = Visibility.Visible;
                    calcular.Visibility = Visibility.Visible;

                    await Calculo.AnimateHeightAsync(200, 300, null);

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


      


        private async void planillaMesAsync(DataSet ds)
        {

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Set the default application version as Excel 2016.
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2016;

                //Create a workbook with a worksheet.
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                //Access first worksheet from the workbook instance.
                IWorksheet worksheet = workbook.Worksheets[0];

                //Insert sample text into cell “A1”.
                worksheet.Name = "Planilla_Mensual";
                worksheet.Range["A1:J1"].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                worksheet.Range["A1:J1"].CellStyle.Font.Color = ExcelKnownColors.White;
                worksheet.Range["A1"].ColumnWidth = 50;
                worksheet.Range["B1:I1"].ColumnWidth = 16;
                worksheet.Range["J1"].ColumnWidth = 30;
                worksheet.Range["A1"].Text = "Empleado";
                worksheet.Range["B1"].Text = "Salario Mes";
                worksheet.Range["C1"].Text = "Hrs Extra";
                worksheet.Range["D1"].Text = "ingreso extra";
                worksheet.Range["E1"].Text = "IHSS";
                worksheet.Range["F1"].Text = "RAP";
                worksheet.Range["G1"].Text = "ISR";
                worksheet.Range["H1"].Text = "Deducciones extra";
                worksheet.Range["I1"].Text = "Total";
                worksheet.Range["J1"].Text = "Fecha";


                foreach (DataTable table in ds.Tables)
                {
                    int count = table.Rows.Count;
                    for (int j = 0; j < count; j++)
                    {
                        worksheet.Range[string.Format("A{0}", j + 2)].Text = table.Rows[j].ItemArray[0].ToString();
                        worksheet.Range[string.Format("B{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[1].ToString());
                        worksheet.Range[string.Format("C{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[2].ToString());
                        worksheet.Range[string.Format("D{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[3].ToString());
                        worksheet.Range[string.Format("E{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[4].ToString());
                        worksheet.Range[string.Format("F{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[5].ToString());
                        worksheet.Range[string.Format("G{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[6].ToString());
                        worksheet.Range[string.Format("H{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[7].ToString());
                        worksheet.Range[string.Format("I{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[8].ToString());
                        worksheet.Range[string.Format("J{0}", j + 2)].DateTime = DateTime.Parse(table.Rows[j].ItemArray[9].ToString());
                    }


                    StorageFile storageFile;

                    
                    FileSavePicker savePicker = new FileSavePicker();
                    savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
                    savePicker.SuggestedFileName = "Planilla Mensual " + fchinicio;
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


        private async void planillasemAsync(DataSet ds)
        {

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Set the default application version as Excel 2016.
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2016;

                //Create a workbook with a worksheet.
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                //Access first worksheet from the workbook instance.
                IWorksheet worksheet = workbook.Worksheets[0];

                //Insert sample text into cell “A1”.
                worksheet.Name = "Planilla_Semanal";
                worksheet.Range["A1:M1"].CellStyle.Color = ColorHelper.FromArgb(0, 42, 118, 189);
                worksheet.Range["A1:M1"].CellStyle.Font.Color = ExcelKnownColors.White;
                worksheet.Range["A1"].ColumnWidth = 50;
                worksheet.Range["B1:L1"].ColumnWidth = 18;
                worksheet.Range["M1"].ColumnWidth = 30;
                worksheet.Range["A1"].Text = "Empleado";
                worksheet.Range["B1"].Text = "Horas";
                worksheet.Range["C1"].Text = "Salario Hora";
                worksheet.Range["D1"].Text = "Devengado x 1.0909";
                worksheet.Range["E1"].Text = "Septimo Dia";
                worksheet.Range["F1"].Text = "Horas extra";
                worksheet.Range["G1"].Text = "Otros ingresos";
                worksheet.Range["H1"].Text = "Seguro";
                worksheet.Range["I1"].Text = "Rap";
                worksheet.Range["J1"].Text = "Isr";
                worksheet.Range["K1"].Text = "Deducciones extra";
                worksheet.Range["L1"].Text = "Total";
                worksheet.Range["M1"].Text = "Fecha";


                foreach (DataTable table in ds.Tables)
                {
                    int count = table.Rows.Count;
                    for (int j = 0; j < count; j++)
                    {
                        worksheet.Range[string.Format("A{0}", j + 2)].Text = table.Rows[j].ItemArray[0].ToString();
                        worksheet.Range[string.Format("B{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[1].ToString());
                        worksheet.Range[string.Format("C{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[2].ToString());
                        worksheet.Range[string.Format("D{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[3].ToString());
                        worksheet.Range[string.Format("E{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[4].ToString());
                        worksheet.Range[string.Format("F{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[5].ToString());
                        worksheet.Range[string.Format("G{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[6].ToString());
                        worksheet.Range[string.Format("H{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[7].ToString());
                        worksheet.Range[string.Format("I{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[8].ToString());
                        worksheet.Range[string.Format("J{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[9].ToString());
                        worksheet.Range[string.Format("K{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[10].ToString());
                        worksheet.Range[string.Format("L{0}", j + 2)].Number = double.Parse(table.Rows[j].ItemArray[11].ToString());
                        worksheet.Range[string.Format("M{0}", j + 2)].DateTime = DateTime.Parse(table.Rows[j].ItemArray[12].ToString());
                    }


                    StorageFile storageFile;


                    FileSavePicker savePicker = new FileSavePicker();
                    savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
                    savePicker.SuggestedFileName = "Planilla Semanal " + desde;
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

        private async void Tipopla_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Convert.ToString(tipopla.SelectedItem) == "Mensual")
            {
                await plasem.AnimateHeightAsync(0, 300, null);
                await plames.AnimateHeightAsync(80, 300, null);
            }

            if (Convert.ToString(tipopla.SelectedItem) == "Semanal")
            {
                await plames.AnimateHeightAsync(0, 300, null);
                await plasem.AnimateHeightAsync(80, 300, null);

            }

        }

        private async void Genplanilla_Click(object sender, RoutedEventArgs e)
        {

            if (Convert.ToString(tipopla.SelectedItem) == "Mensual")
            {

                if (fchplamen.SelectedDate != null)

                { 


                int mesActual = Convert.ToInt32(fchplamen.SelectedDate.Value.Month);
                int year = Convert.ToInt32(fchplamen.SelectedDate.Value.Year);
                int mesSiguiente = mesActual + 1;


                fchinicio = Convert.ToString("01/" + mesActual + "/" + year);

                if(mesActual == 12)
                {
                    fchfinal = Convert.ToString("31/" + mesActual + "/" + year);

                }
                else
                {

                    DateTime mes = Convert.ToDateTime("01/" + mesSiguiente + "/" + year).AddDays(-1);
                    fchfinal = Convert.ToString(mes).Substring(0, 10);

                }

                


                int filas = Tabla.Tables[0].Rows.Count;



                DataSet listemp = new DataSet();

               
                   
                    for (int i = 0; i < filas; i++)
                    {

                        string cta;
                        cta = Convert.ToString(Tabla.Tables[0].Rows[i][0]);
                        sql.EXECUTE("Exec InPlanillaMes '" + cta + "', '" + fchinicio + "', '" + fchfinal + "'");




                    }

                    listemp = sql.Conexion("Exec ShowPlanillames '" + (Convert.ToDateTime(fchinicio)).ToString().Substring(0, 10) + "' ");
                    planillaMesAsync(listemp);

                }
                else
                {

                    string mensaje = "Error, Selecione la fecha de la cual desea generar la planilla";
                    MessageDialog ms = new MessageDialog(mensaje, "No se ingreso fecha");
                    await ms.ShowAsync();

                }

            }
            
            
            

             

            if (Convert.ToString(tipopla.SelectedItem) == "Semanal")
            {

                if(fchsem.Date != null)
                {

                    if (fchsem.Date.Value.DayOfWeek.ToString() == "Monday" || fchsem.Date.Value.DayOfWeek.ToString() == "Lunes")
                    {
                        int mesActual = Convert.ToInt32(fchsem.Date.Value.Month);
                        int year = Convert.ToInt32(fchsem.Date.Value.Year);
                        int mesSiguiente = mesActual + 1;


                        fchinicio = Convert.ToString("01/" + mesActual + "/" + year);
                        if (mesActual == 12)
                        {
                            fchfinal = Convert.ToString("31/" + mesActual + "/" + year);

                        }
                        else
                        {

                            DateTime mes = Convert.ToDateTime("01/" + mesSiguiente + "/" + year).AddDays(-1);
                            fchfinal = Convert.ToString(mes).Substring(0, 10);

                        }


                        desde = fchsem.Date.Value.Date.ToString().Substring(0, 10);

                        string hasta = fchsem.Date.Value.AddDays(6).ToString().Substring(0, 10);

                        string cta;

                        int filas = Tabla.Tables[0].Rows.Count;



                        for (int i = 0; i < filas; i++)
                        {

                            cta = Convert.ToString(Tabla.Tables[0].Rows[i][0]);

                            sql.EXECUTE("Exec InPlanillaSem '" + cta + "', '" + desde + "', '" + hasta + "', '" + fchinicio + "', '" + fchfinal + "'");


                        }

                        DataSet listemp = sql.Conexion("Exec ShowPlanillaSem '" + desde + "',  '" + hasta + "' ");
                        planillasemAsync(listemp);



                    }
                    else
                    {
                        string mensaje = "Error, Selecione la fecha del dia lunes para calcular esa semana";
                        MessageDialog ms = new MessageDialog(mensaje, "La fecha ingresada no corresponde a un Lunes");
                        await ms.ShowAsync();
                    }

                }
                else
                {

                    string mensaje = "Error, Selecione la fecha de la cual desea generar la planilla";
                    MessageDialog ms = new MessageDialog(mensaje, "No se ingreso fecha");
                    await ms.ShowAsync();

                }
            }


            if (tipopla.SelectedItem == null)
            {
                string mensaje = "Error, Seleccione un tipo de planilla para continuar";
                MessageDialog ms = new MessageDialog(mensaje, "No se seleccion ningun tipo de planilla");
                await ms.ShowAsync();

            }


            }
    }
    }



