using MVITO.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Page1 : Page
    {
        
        public Page1()
        {
            this.InitializeComponent();
            

        }

        private async void Agregar(object sender, RoutedEventArgs e)
        {

            if(Compras.SelectedItem != null && Total.Text != "")
            { 
            //EXECUTE Compra 'Bebidas','4 Coca Colas','19-Abril-2019',100,15            
            Connection sql = new Connection();
            sql.EXECUTE(string.Format("EXECUTE Compra '{0}','{1}',{2},'{3}',{4}, {5}", Compra(), DescriptionTxt(), cantidadtxt() ,Fecha(), TotalTxt(), ISVTxt()));
            Compras.SelectedItem = null;
            op1.IsSelected = false;
            op2.IsSelected = false;
            op3.IsSelected = false;
            Descripcion.Text = "";
            Total.Text = "";
            Selection.SelectedItem = null;
            ISV.IsSelected = false;
            NoISV.IsSelected = false;
            }

            else
            {
                string mensaje = "Error, Hay campos que necesitan ser llenados";
                MessageDialog ms = new MessageDialog(mensaje, "Error al ingresar Compra");
                await ms.ShowAsync();
            }

        }

        private string Compra()
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

        private string cantidadtxt()
        {
            return cantidad.Text;
        }

        private string DescriptionTxt()
        {
            //Descripcion de la compra
            if (Descripcion.Text.Length > 0) return Descripcion.Text;
            else return "Sin Descripcion";
        }
        private string Fecha()
        {
            //Fecha de la compra
            return DateTime.Today.ToString("dddd, dd MMMM yyyy") + " " + DateTime.Now.ToString("HH:MM tt");
        }
        private string TotalTxt()
        {
            //Total de la compra
            if (Total.Text.Length > 0) return Total.Text;
            else return "0.00";
        }
        private string ISVTxt()
        {
            //Impuesto de la compra
            if (Total.Text.Length > 0 && ISV.IsSelected == true)
            {
                return (Convert.ToDecimal(Total.Text) * 0.15m).ToString("##.00");
            }
            else return "0.00";
        }

        private void Cantidad_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (cantidad.Text != "" && precio.Text != "")
            {
                calcular.IsEnabled = true;
            }
            
        }

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

        private void Calcular_Click(object sender, RoutedEventArgs e)
        {
            Total.Text = Convert.ToString(Convert.ToDecimal(cantidad.Text) * Convert.ToDecimal(precio.Text));
        }

        private void Precio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cantidad.Text != "" && precio.Text != "")
            {
                calcular.IsEnabled = true;
            }
        }

        private void Descripcion_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

