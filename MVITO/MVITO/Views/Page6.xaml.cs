using MVITO.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Data;


// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace MVITO.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Page6 : Page
    {

        //Conexion
        Connection cnn = new Connection();
        //tabla para cargar empleados
        DataSet Tabla = new DataSet();
        //Otra conexion xD revisa para que es antes de entregar
        Connection con = new Connection();
        //variable para guardar identidad
        string identidad;
        //variable para guardar salario
        decimal dinero;

        ObservableCollection<Empleados> emp = new ObservableCollection<Empleados>(); 

        public Page6()
        {
            this.InitializeComponent();

            

           



        }


        ////////////////////////// Animaciones //////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////




        //////////////////////////Evento del pivote gastos///////////////////////////////////


        // Evento Click en boton AgregarGasto
        private async void Agregargasto(object sender, RoutedEventArgs e)
        {
            string strMensaje = string.Format("Tipo de Gasto : {1}{0}Descripcion del gasto : {2}{0}Total de : {3}{0}", Environment.NewLine, Gasto.SelectedValue , Descripciong.Text, Totalg.Text);



            if (Gasto.SelectedItem != null && Totalg.Text != "")
            {

                var dialog = new Windows.UI.Popups.MessageDialog(
                        strMensaje + "¿Deseas ingresar estos datos?",
                        "Esta a punto de guardar la siguiente informacion: ");

                dialog.Commands.Add(new UICommand("SI") { Id = 0 });
                dialog.Commands.Add(new UICommand("No") { Id = 1 });

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                var result = await dialog.ShowAsync();

                if ((int)result.Id == 0)
                {
                    Connection sql = new Connection();
                    sql.EXECUTE(string.Format("EXECUTE Gasto '{0}','{1}','{2}',{3}", Gasto.SelectedItem , Descripciong.Text , Fecha(), Totalg.Text));
                    Gasto.SelectedItem = null;
                    Descripciong.Text = "";
                    Totalg.Text = "";
                    aviso.Text = "Gasto ingresado exitosamente";
                    

                }
                else
                {
                    aviso.Text = "Ingreso Cancelado";
                    aviso.Focus(FocusState.Programmatic);
                    
                }

            }

            else
            {
                string mensaje = "Error, Hay campos que necesitan ser llenados";
                MessageDialog ms = new MessageDialog(mensaje, "Error al ingresar Compra");
                await ms.ShowAsync();
            }
            
        }

        // Funciones recopiladoras de infomarcion
       
       
        private string Fecha()
        {
            //Fecha de la compra
            return DateTime.Today.ToString("dddd, dd MMMM yyyy") + " " + DateTime.Now.ToString("HH:MM tt");
        }
     
        


        //validacion del campo descripcion
        private void Descripciong_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            Validacion.letrasynumeros(e);
        }

        private void Totalg_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            Validacion.numeros(e);
        }


        //////////////////////////////////////////////////////////////////////////////////////





        //////////////////////////Evento del pivote compras///////////////////////////////////

        private void Interruptor_Toggled(object sender, RoutedEventArgs e)
        {
            if (panelcalcular.Visibility == Visibility.Visible)
            {
                panelcalcular.Visibility = Visibility.Collapsed;
                splitpanel.Height = 30;
            }
            else
            {
                panelcalcular.Visibility = Visibility.Visible;
                splitpanel.Height = 120;
            }
        }

        private async void Agregarcompra(object sender, RoutedEventArgs e)
        {
            if (Compras.SelectedItem != null && Totalc.Text != "" && cantidad.Text != "" && Selection.SelectedItem != null)
            {

                string strMensaje = string.Format("Compra de : {1}{0}Descripcion de la compra : {2}{0}Cantidad de : {3}{0}total de : {4}{0} Total ISV : {5}{0}", Environment.NewLine, Comprastxt(), Descripcionc.Text, cantidad.Text, Totalc.Text, ISVTxt());



                    var dialog = new Windows.UI.Popups.MessageDialog(
                            strMensaje + "¿Deseas ingresar estos datos?",
                            "Esta a punto de guardar la siguiente informacion: ");

                    dialog.Commands.Add(new UICommand("SI") { Id = 0 });
                    dialog.Commands.Add(new UICommand("No") { Id = 1 });

                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;

                    var result = await dialog.ShowAsync();

                if ((int)result.Id == 0)
                {

                    Connection sql = new Connection();
                    sql.EXECUTE(string.Format("EXECUTE Compra '{0}','{1}',{2},'{3}',{4}, {5}", Comprastxt(), Descripcionc.Text, cantidad.Text, Fechac(), Totalc.Text, ISVTxt()));
                    Compras.SelectedItem = null;
                    op1.IsSelected = false;
                    op2.IsSelected = false;
                    op3.IsSelected = false;
                    Descripcionc.Text = "";
                    Totalc.Text = "";
                    Selection.SelectedItem = null;
                    ISV.IsSelected = false;
                    NoISV.IsSelected = false;


                    avisoc.Visibility = Visibility.Visible;
                    avisoc.Text = "Compra ingresada exitosamente";

                }
                else
                {
                    avisoc.Visibility = Visibility.Visible;
                    avisoc.Text = "Compra cancelada";

                }


                  
            }

            else
            {
                string mensaje = "Error, Hay campos que necesitan ser llenados";
                MessageDialog ms = new MessageDialog(mensaje, "Error al ingresar Compra");
                await ms.ShowAsync();
            }
        }



        private void Calcular_Click(object sender, RoutedEventArgs e)
        {
            Totalc.Text = Convert.ToString(Convert.ToDecimal(cantidad.Text) * Convert.ToDecimal(precio.Text));
        }

        private void Cantidad_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (cantidad.Text != "" && precio.Text != "")
            {
                calcular.IsEnabled = true;
            }


        }

        private void Precio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cantidad.Text != "" && precio.Text != "")
            {
                calcular.IsEnabled = true;
            }
        }

        private string Fechac()
        {
            //Fecha de la compra
            return DateTime.Today.ToString("dddd, dd MMMM yyyy") + " " + DateTime.Now.ToString("HH:MM tt");
        }

        private string ISVTxt()
        {
            //Impuesto de la compra
            if (Totalc.Text.Length > 0 && ISV.IsSelected == true)
            {
                return (Convert.ToDecimal(Totalc.Text) * 0.15m).ToString("##.00");
            }
            else return "0.00";
        }

        private string Comprastxt()
        {


            if (op1.IsSelected == true)
            {
                return "Insumos(Materias Primas)";
            }
            if (op2.IsSelected == true)
            {
                return "Materiales/Equipo de trabajo";
            }

            if (op3.IsSelected == true)
            {
                return "Moviliario/Material de oficina";
            }

            if (op4.IsSelected == true)
            {
                return "Otros(Especifique en descripcion)";
            }

            return null;

        }

        //Validaciones de campos
        private void Descripcionc_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            Validacion.letrasynumeros(e);
        }
        private void Cantidad_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            Validacion.numeros(e);
        }
        private void Precio_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            Validacion.numeros(e);
        }
        private void Totalc_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            Validacion.numeros(e);
        }






        //////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////Evento del pivote perdidas///////////////////////////////////


        private async void Agregarperdida(object sender, RoutedEventArgs e)
        {

            if (Perdidas.SelectedItem != null && Descripcionp.Text != "" && Totalp.Text != "")
            {

                string strMensaje = string.Format("Tipo de perdida : {1}{0}Descripcion de la perdida : {2}{0}Total de : {3}{0}", Environment.NewLine, Perdidas.SelectedItem , Descripcionp.Text, Totalp.Text);


                    var dialog = new Windows.UI.Popups.MessageDialog(
                    strMensaje + "¿Deseas ingresar estos datos?",
                    "Esta a punto de guardar la siguiente informacion: ");

                    dialog.Commands.Add(new UICommand("SI") { Id = 0 });
                    dialog.Commands.Add(new UICommand("No") { Id = 1 });

                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;

                    var result = await dialog.ShowAsync();

                if ((int)result.Id == 0)
                {

                    Connection sql = new Connection();
                    sql.EXECUTE(string.Format("EXECUTE Perdida '{0}','{1}','{2}',{3}", Perdidas.SelectedItem, Descripcionp.Text, Fecha(), Totalp.Text));
                    Perdidas.SelectedItem = null;
                    Descripcionp.Text = "";
                    Totalp.Text = "";

                    avisop.Text = "Perdida registrada exitosamente";

                }
                else
                {
                    avisop.Text = "Perdida Cancelada";
                }

            }
            else
            {
                string mensaje = "Error, Hay campos que necesitan ser llenados";
                MessageDialog ms = new MessageDialog(mensaje, "Error al ingresar Compra");
                await ms.ShowAsync();
            }

        }

        private void Descripcionp_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            Validacion.letrasynumeros(e);
        }

        private void Totalp_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            Validacion.numeros(e);
        }




        //////////////////////////////////////////////////////////////////////////////////////


        //////////////////////////Evento del pivote repartidores///////////////////////////////////



        private void AgregarRepartidor(object sender, RoutedEventArgs e)
        {

        }


        private void RepartidorSelection(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CantidadSelection(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MovimientoSelection(object sender, SelectionChangedEventArgs e)
        {

        }


        //////////////////////////////////////////////////////////////////////////////////////


       

        private void Descripcion_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
       
        private void Compras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

      
    }
}
