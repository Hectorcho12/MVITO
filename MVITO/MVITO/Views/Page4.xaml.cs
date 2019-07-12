using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MVITO.Clases;
using MVITO.Views;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using System.Numerics;
using MVITO.Extensiones;
using System.Diagnostics;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace MVITO.Views
{

    

    public sealed partial class Page4 : Page
    {
        DataSet EmpleadoCheck = new DataSet();
        DataSet Tabla = new DataSet();

        Connection con = new Connection();

        string identidad;
        
        decimal dinero;

        ObservableCollection<Empleados> emp = new ObservableCollection<Empleados>();

        public Page4()
        {
            this.InitializeComponent();

            Tabla = con.Conexion(Convert.ToString("Exec ShowEmpleados"));

            llenarlista();
            
        }

        //////////////////////////Eventos del pivot Empleados///////////////////////////////

        private async void IngresoEmpleado_Click(object sender, RoutedEventArgs e)
        {


            string desde = cbxentrada.SelectedTime.ToString().Substring(0, 2);
            string hasta = Cbxsalida.SelectedTime.ToString().Substring(0, 2);


            int totalhoras = (Convert.ToInt32(hasta) - Convert.ToInt32(desde));

            string gen = "";


            if (Convert.ToString(genempleado.SelectedItem) == "Masculino")
            {
                gen = "M";
            }

            if (Convert.ToString(genempleado.SelectedItem) == "Femenino")
            {
                gen = "F";
            }

            if (Indefinido.IsChecked == true)
            {
                con.EXECUTE(Convert.ToString("Exec InModEmpleados '" + IDempleado.Text + "', '" + nomempleado.Text + "', '" + nacempleado.SelectedDate + "', '" + gen + "', '" + iniempleado.SelectedDate
               + "', '" + Convert.ToBoolean(1) + "', '" + domempleado.Text + "' , '" + Salarioempleado.Text + "', '" + cbxentrada.SelectedTime + "', '" + Cbxsalida.SelectedTime + "', '" + puestoempleado.Text + "','"
               + comentarioempleado.Text + "',NULL , '" + totalhoras + "'"));


                string mensaje = "El empleado " + nomempleado.Text + " ha sido ingresado exitosamente";
                MessageDialog ms = new MessageDialog(mensaje, "Empleado ingresado exitosamente");
                await ms.ShowAsync();


                IDempleado.Text = "";
                nomempleado.Text = "";
                nacempleado.SelectedDate = null;
                genempleado.SelectedItem = null;
                iniempleado.SelectedDate = null;
                domempleado.Text = "";
                Salarioempleado.Text = "";
                cbxentrada.SelectedTime = null;
                Cbxsalida.SelectedTime = null;
                puestoempleado.Text = "";
                comentarioempleado.Text = "";
                durampleado.SelectedDate = null;
                Indefinido.IsChecked = false;

                Tabla = con.Conexion(Convert.ToString("Exec ShowEmpleados"));
                llenarlista();

                

            }
            else
            {

                


                con.EXECUTE(Convert.ToString("Exec InModEmpleados '" + IDempleado.Text + "', '" + nomempleado.Text + "', '" + nacempleado.SelectedDate + "', '" + gen + "', '" + iniempleado.SelectedDate
               + "', '" + Convert.ToBoolean(1) + "', '" + domempleado.Text + "' , '" + Salarioempleado.Text + "', '" + cbxentrada.SelectedTime + "', '" + Cbxsalida.SelectedTime + "', '" + puestoempleado.Text + "','"
               + comentarioempleado.Text + "','" + durampleado.SelectedDate + "' , '" + totalhoras + "'"));


                string mensaje = "El empleado " + nomempleado.Text + " ha sido ingresado exitosamente";
                MessageDialog ms = new MessageDialog(mensaje, "Empleado ingresado exitosamente");
                await ms.ShowAsync();


                IDempleado.Text = "";
                nomempleado.Text = "";
                nacempleado.SelectedDate = null;
                genempleado.SelectedItem = null;
                iniempleado.SelectedDate = null;
                domempleado.Text = "";
                Salarioempleado.Text = "";
                cbxentrada.SelectedTime = null;
                Cbxsalida.SelectedTime = null;
                puestoempleado.Text = "";
                comentarioempleado.Text = "";
                durampleado.SelectedDate = null;
                Indefinido.IsChecked = false;

                Tabla = con.Conexion(Convert.ToString("Exec ShowEmpleados"));
                llenarlista();

            }




        }



        private void Btnsearchempleados_Click(object sender, RoutedEventArgs e)
        {


            string cta;
            string nom;
            string pst;
            decimal sal;
            int filas = Tabla.Tables[0].Rows.Count;
            emp.Clear();



            for (int i = 0; i < filas; i++)
            {



                cta = Convert.ToString(Tabla.Tables[0].Rows[i][0]);
                nom = Convert.ToString(Tabla.Tables[0].Rows[i][1]);
                sal = Convert.ToDecimal(Tabla.Tables[0].Rows[i][2]);
                pst = Convert.ToString(Tabla.Tables[0].Rows[i][3]);


                if (nom.Contains(Buscadorempleado.Text))
                {
                    Empleados Empledatos = new Empleados() { IDemp = cta, NmEmp = nom, PstEmp = pst, SalEmp = sal };
                    emp.Add(Empledatos);


                }


            }

            listaempleados.ItemsSource = emp;


        }


        private void llenarlista()
        {
            string cta;
            string nom;
            string pst;
            decimal sal;
            int filas = Tabla.Tables[0].Rows.Count;
            emp.Clear();


            for (int i = 0; i < filas; i++)
            {

                cta = Convert.ToString(Tabla.Tables[0].Rows[i][0]);
                nom = Convert.ToString(Tabla.Tables[0].Rows[i][1]);
                sal = Convert.ToDecimal(Tabla.Tables[0].Rows[i][2]);
                pst = Convert.ToString(Tabla.Tables[0].Rows[i][3]);

                Empleados Empledatos = new Empleados() { IDemp = cta, NmEmp = nom, PstEmp = pst, SalEmp = sal };
                emp.Add(Empledatos);


            }

            listaempleados.ItemsSource = emp;
        }




        //////////////////////////////////////////////////////////////////////////////////////






        ////////////////////Eventos pivot extras//////////////////////////////////////////////


        private async void Tipoextra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Convert.ToString(Tipoextra.SelectedItem) == "Horas Extras")
            {            
                await stackin.AnimateWidthAsync(0, 300, null);

                await stackeg.AnimateWidthAsync(0, 300, null);

                await stackhx.AnimateWidthAsync(400, 300, null);
            }

            if (Convert.ToString(Tipoextra.SelectedItem) == "Ingresos extras")
            {

                await stackhx.AnimateWidthAsync(0, 300, null);

                await stackeg.AnimateWidthAsync(0, 300, null);

                await stackin.AnimateWidthAsync(400, 300, null);

                

            }

            if (Convert.ToString(Tipoextra.SelectedItem) == "Egresos Extras")
            {
                await stackhx.AnimateWidthAsync(0, 300, null);

                await stackin.AnimateWidthAsync(0, 300, null);

                await stackeg.AnimateWidthAsync(400, 300, null);

            }

        }

        private async void Botonhx_Click(object sender, RoutedEventArgs e)
        {



            if (listaempleados.SelectedItem != null && hx1.SelectedTime != null && hx2.SelectedTime != null && hxfch.SelectedDate != null)
            {

                string h1 = Convert.ToString(hx1.SelectedTime).Substring(0, 2);
                string h2 = Convert.ToString(hx2.SelectedTime).Substring(0, 2);
                int totalhx = 0;
                int tipo = 0;


                totalhx = calculohoras(h1, h2);

                if ((Convert.ToInt32(h1) >= 5 && Convert.ToInt32(h1) <= 19) && (Convert.ToInt32(h2) >= 5 && Convert.ToInt32(h2) <= 19))
                {
                    //las horas tipo 1 son diurnas
                    tipo = 1;
                }
                else
                {
                    //las horas tipo 2 son nocturnas
                    tipo = 2;
                }


                if (totalhx <= 12 && totalhx != 0)
                {




                    con.EXECUTE(Convert.ToString("Exec InHrsX  '" + identidad + "', '" + hxfch.SelectedDate + "', '" + totalhx + "',  '" + tipo + "'"));


                    string mensaje = "Ingreso correcto de las horas extras";
                    MessageDialog ms = new MessageDialog(mensaje,"Registro Exitoso");
                    await ms.ShowAsync();


                   






                }
                else
                {
                    string mensaje = "Error, Ingrese un rango de horas valido";
                    MessageDialog ms = new MessageDialog(mensaje, "Las horas extras no pueden exceder a 12 horas ni pueden ser 0");
                    await ms.ShowAsync();


                }

            }
            else
            {
                string mensaje = "Error, Hay campos que necesitan ser llenados";
                MessageDialog ms = new MessageDialog(mensaje, "Se han dejado campos vacios o no se selecciono un empleado");
                await ms.ShowAsync();
            }



        }

        private void Listaempleados_ItemClick(object sender, ItemClickEventArgs e)
        {
            Empleados emple = (Empleados)e.ClickedItem;

            diaperdido.IsEnabled = true;
            diaextra.IsEnabled = true;

            identidad = Convert.ToString(emple.IDemp);
            dinero = Convert.ToDecimal(emple.SalEmp);


            if (diaextra.IsOn == true)
            {
                ixdescripcion.Text = "Dia de trabajo extra";
                ixtotal.Text = Convert.ToString(Math.Round((dinero / 30), 2));
                

            }
            if (diaperdido.IsOn == true)
            {
                exdescripcion.Text = "Dia de trabajo perdido";
                extotal.Text = Convert.ToString(Math.Round((dinero / 30), 2));


            }









        }

        private async void Ixingreso_Click(object sender, RoutedEventArgs e)
        {

            if (listaempleados.SelectedItem != null && ixdescripcion.Text != "" && ixtotal.Text != "" && ixfch.SelectedDate != null)
            {

                con.EXECUTE(Convert.ToString("Exec InOtrosInEg  '" + identidad + "', '" + ixdescripcion.Text + "', '" + 1 + "',  '" + ixtotal.Text + "' ,  '" + ixfch.SelectedDate + "'"));

                string mensaje = "Registro correcto del ingreso extra";
                MessageDialog ms = new MessageDialog(mensaje, "Registro Exitoso");
                await ms.ShowAsync();

                diaextra.IsOn = false;

                ixdescripcion.Text = "";
                ixtotal.Text = "";




            }
            else
            {

                string mensaje = "Error, Hay campos que necesitan ser llenados";
                MessageDialog ms = new MessageDialog(mensaje, "Se han dejado campos vacios o no se selecciono un empleado");
                await ms.ShowAsync();

            }

        }

        private async void Exingreso_Click(object sender, RoutedEventArgs e)
        {
            if (listaempleados.SelectedItem != null && exdescripcion.Text != "" && extotal.Text != "" && exfch.SelectedDate != null)
            {

                con.EXECUTE(Convert.ToString("Exec InOtrosInEg  '" + identidad + "', '" + exdescripcion.Text + "', '" + 0 + "',  '" + extotal.Text + "' ,  '" + exfch.SelectedDate + "'"));

                string mensaje = "Registro correcto del egreso extra";
                MessageDialog ms = new MessageDialog(mensaje, "Registro Exitoso");
                await ms.ShowAsync();

                diaperdido.IsOn = false;

                exdescripcion.Text = "";
                extotal.Text = "";




            }
            else
            {

                string mensaje = "Error, Hay campos que necesitan ser llenados";
                MessageDialog ms = new MessageDialog(mensaje, "Se han dejado campos vacios o no se selecciono un empleado");
                await ms.ShowAsync();

            }

        }

        private void Diaperdido_Toggled(object sender, RoutedEventArgs e)
        {

            if (diaperdido.IsOn == true)
            {
                exdescripcion.Text = "Dia de trabajo perdido";
                extotal.Text = Convert.ToString(Math.Round((dinero / 30), 2));

                exdescripcion.IsEnabled = false;
                extotal.IsEnabled = false;

            }
            else
            {

                exdescripcion.Text = "";
                extotal.Text = "";

                exdescripcion.IsEnabled = true;
                extotal.IsEnabled = true;

            }


        }


        private void ValidarNumero(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            
            if (!System.Text.RegularExpressions.Regex.IsMatch(sender.Text, "^\\d*\\.?\\d*$") && sender.Text !="")
            {
                int pos = sender.SelectionStart - 1;
                if (pos < 0)
                {
                    pos = 0;
                    sender.Text = sender.Text.Remove(pos, 1);
                    sender.SelectionStart = pos;
                }
                else
                {
                    sender.Text = sender.Text.Remove(pos, 1);
                    sender.SelectionStart = 1;
                }
            }
        }

        
           
        private void Salarioempleado_LostFocus(object sender, RoutedEventArgs e)
        {

            mensaje.Text = "Recuerda que el salario minimo de tus empleados debe ser de 8920.80 \nsegun el rubro de comercios";

            FlyoutMss.ShowAt(Salarioempleado);

        }


        private int calculohoras(string h1, string h2)
        {
            int totalhx = 0;
            


            if (Convert.ToInt32(h2) > Convert.ToInt32(h1))
            {
                totalhx = Convert.ToInt32(h2) - Convert.ToInt32(h1);
                return totalhx;
            }

            if (Convert.ToInt32(h2) < Convert.ToInt32(h1))
            {
                totalhx = (24 - Convert.ToInt32(h1)) + Convert.ToInt32(h2);
                return totalhx;
            }


            return 0;

        }

        private async void Cbxentrada_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
        {
            if (cbxentrada.SelectedTime != null && Cbxsalida.SelectedTime != null)
            {



                string h1 = Convert.ToString(cbxentrada.SelectedTime).Substring(0, 2);
                string h2 = Convert.ToString(Cbxsalida.SelectedTime).Substring(0, 2);
                int totalhx = 0;


                if (h1 != h2)
                {



                    totalhx = calculohoras(h1, h2);


                    if (totalhx > 8)
                    {
                        mensaje.Text = "La jornada laboral no debe exceder de 8 horas \nEsto implica un aumento en el salario del empleado";
                        FlyoutMss.ShowAt(Cbxsalida);
                       
                    }

                }
                else
                {

                    string mensaje = "Error, Las horas de trabajo no pueden ser 0";
                    MessageDialog ms = new MessageDialog(mensaje, "Los horarios asignados presentan un error");
                    await ms.ShowAsync();
                    cbxentrada.SelectedTime = null;
                    Cbxsalida.SelectedTime = null;

                }


            }

        }

        private async void Cbxsalida_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
        {
            if (cbxentrada.SelectedTime != null && Cbxsalida.SelectedTime != null)
            {

                

                string h1 = Convert.ToString(cbxentrada.SelectedTime).Substring(0, 2);
            string h2 = Convert.ToString(Cbxsalida.SelectedTime).Substring(0, 2);
            int totalhx = 0;


                if(h1 != h2) { 



            totalhx = calculohoras(h1, h2);


                if (totalhx > 8)
                {
                    mensaje.Text = "La jornada laboral no debe exceder de 8 horas \nEsto implica un aumento en el salario del empleado";
                    FlyoutMss.ShowAt(Cbxsalida);
                    
                }

                }
                else
                {

                    string mensaje = "Error, Las horas de trabajo no pueden ser 0";
                    MessageDialog ms = new MessageDialog(mensaje, "Los horarios asignados presentan un error");
                    await ms.ShowAsync();
                    cbxentrada.SelectedTime = null;
                    Cbxsalida.SelectedTime = null;

                }


            }

        }

        private void Durampleado_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            DateTime date = DateTime.Now;

            if(durampleado.SelectedDate != null) { 

            if (durampleado.SelectedDate.Value.CompareTo(date) < 1)
            {

                mensaje.Text = "La fecha del contrato debe ser despues del " + Convert.ToString(date).Substring(0,10);
                FlyoutMss.ShowAt(durampleado);
                durampleado.SelectedDate = null;
            }
            }
        }

        private void Diaextra_Toggled(object sender, RoutedEventArgs e)
        {
            if (diaextra.IsOn == true)
            {
                ixdescripcion.Text = "Dia de trabajo Extra";
                ixtotal.Text = Convert.ToString(Math.Round((dinero / 30), 2));

                ixdescripcion.IsEnabled = false;
                ixtotal.IsEnabled = false;

            }
            else
            {

                ixdescripcion.Text = "";
                ixtotal.Text = "";

                ixdescripcion.IsEnabled = true;
                ixtotal.IsEnabled = true;

            }

        }

        private async void IDempleado_LostFocus(object sender, RoutedEventArgs e)
        {


            EmpleadoCheck = con.Conexion("select * from empleados where idemp = '" + IDempleado.Text + "'");

            int estado;
            string nombre;
            DateTime fchnaci;
            string genero;
            DateTime fchinicio;
            string domicilio;
            string salario;

            
            TimeSpan hrentrada;
            TimeSpan hrsalida;
            string puesto;
            string comentario;
            DateTime duracion;


            if (EmpleadoCheck.Tables[0].Rows.Count == 1)
            {

                int horaentrada = Convert.ToInt32(Convert.ToString(EmpleadoCheck.Tables[0].Rows[0][8]).Substring(0,2));
                int horasalida = Convert.ToInt32(Convert.ToString(EmpleadoCheck.Tables[0].Rows[0][9]).Substring(0, 2));


                estado = Convert.ToInt32(EmpleadoCheck.Tables[0].Rows[0][5]);
                nombre = Convert.ToString(EmpleadoCheck.Tables[0].Rows[0][1]);
                fchnaci = Convert.ToDateTime(EmpleadoCheck.Tables[0].Rows[0][2]);
                if (Convert.ToString(EmpleadoCheck.Tables[0].Rows[0][3]) == "M")
                {
                    genero = "Masculino";
                }
                else
                {
                    genero = "Femenino";
                }
                fchinicio = Convert.ToDateTime(EmpleadoCheck.Tables[0].Rows[0][4]);
                domicilio = Convert.ToString(EmpleadoCheck.Tables[0].Rows[0][6]);
                salario = Convert.ToString(EmpleadoCheck.Tables[0].Rows[0][7]);
                hrentrada = new TimeSpan(horaentrada, 0, 0);
                hrsalida = new TimeSpan(horasalida, 0, 0);
                puesto = Convert.ToString(EmpleadoCheck.Tables[0].Rows[0][10]);
                comentario = Convert.ToString(EmpleadoCheck.Tables[0].Rows[0][11]);

               
                



                if (estado == 1)
                {
                    var dialog = new Windows.UI.Popups.MessageDialog(
                            "Desea cambiar el contrato del empleado " + nombre + " o finalizarlo? ",
                            "Ya hay un empleado registrado con la identidad ingresada");

                    dialog.Commands.Add(new UICommand("Modificar contrato") { Id = 0 });
                    dialog.Commands.Add(new UICommand("Terminar contrato") { Id = 1 });

                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;

                    var result = await dialog.ShowAsync();

                    if ((int)result.Id == 0)
                    {
                        mensaje.Text = "Cambie las opciones del contrato y presione guardar \npara la modificacion del contrato";
                        FlyoutMss.ShowAt(IDempleado);

                        nomempleado.Text = nombre;
                        nacempleado.SelectedDate = fchnaci;
                        genempleado.SelectedItem = genero;
                        iniempleado.SelectedDate = fchinicio;
                        domempleado.Text = domicilio;
                        Salarioempleado.Text = salario;
                        cbxentrada.Time =  hrentrada;
                        Cbxsalida.Time = hrsalida;
                        puestoempleado.Text = puesto;
                        comentarioempleado.Text = comentario;
                        if (Convert.ToString(EmpleadoCheck.Tables[0].Rows[0][12]) == "")
                        {
                            Indefinido.IsChecked = true;
                        }
                        else
                        {
                            duracion = Convert.ToDateTime(EmpleadoCheck.Tables[0].Rows[0][12]);
                            durampleado.SelectedDate = duracion;
                        }

                        


                    }
                    else
                    {

                        con.EXECUTE("update empleados set EstadoEmp = 0 where idEmp = '" + IDempleado.Text + "'");

                        Tabla = con.Conexion(Convert.ToString("Exec ShowEmpleados"));

                        llenarlista();

                        IDempleado.Text = "";

                        string mensaje = "El contrato del empleado " + nombre + " ha sido eliminado.\nSi desea generar un nuevo contrato para el empleado digite el numero de identidad de este";
                        MessageDialog ms = new MessageDialog(mensaje, "Empleado eliminado exitosamente");
                        await ms.ShowAsync();

                    }

                }
                else
                {

                    var dialog = new Windows.UI.Popups.MessageDialog(
                            "Deseas recontratar a " + nombre + " y crear un nuevo contrato?\nPresione cancelar para ingresar un nuevo numero de identidad ",
                            "El numero de identidad coincide con un empleado que trabajo aqui");

                    dialog.Commands.Add(new UICommand("Nuevo contrato") { Id = 0 });
                    dialog.Commands.Add(new UICommand("Cancelar") { Id = 1 });

                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;

                    var result = await dialog.ShowAsync();

                    if ((int)result.Id == 0)
                    {

                        mensaje.Text = "la informacion mostrada es del contrato anterior del empleado\nRealize los cambios que vea necesarios y guarde el nuevo contrato";
                        FlyoutMss.ShowAt(IDempleado);

                        nomempleado.Text = nombre;
                        nacempleado.SelectedDate = fchnaci;
                        genempleado.SelectedItem = genero;
                        iniempleado.SelectedDate = fchinicio;
                        domempleado.Text = domicilio;
                        Salarioempleado.Text = salario;
                        cbxentrada.Time = hrentrada;
                        Cbxsalida.Time = hrsalida;
                        puestoempleado.Text = puesto;
                        comentarioempleado.Text = comentario;
                        if (Convert.ToDateTime(EmpleadoCheck.Tables[0].Rows[0][12]) == null)
                        {
                            Indefinido.IsChecked = true;
                        }
                        else
                        {
                            duracion = Convert.ToDateTime(EmpleadoCheck.Tables[0].Rows[0][12]);
                            durampleado.SelectedDate = duracion;
                        }

                    }
                    else
                    {

                        IDempleado.Text = "";

                    }

                }

            }


        }








        //////////////////////////////////////////////////////////////////////////////////////
    }
}
