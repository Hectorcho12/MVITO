using MVITO.Clases;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Monitoreo.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Page4 : Page
    {
        private string repartidor = "Sin Seleccionar";
        private string movimiento = "Sin Seleccionar";
        private string cantidad = "0";
        public Page4()
        {
            this.InitializeComponent();
        }
        private void Agregar(object sender, RoutedEventArgs e)
        {
            //execute Envio 'Repartidor','Salida',12,'12-02-2019',250      
            Connection sql = new Connection();
            sql.EXECUTE(string.Format("EXECUTE Envio '{0}','{1}',{2},'{3}',{4}", repartidor, movimiento, cantidad, Fecha(), Dinero()));
            repartidor = "Sin Seleccionar";
            movimiento = "Sin Seleccionar";
            cantidad = "0";
            //Repartidor.SelectedItem = null;
            //Cantidad.SelectedItem = null;
            //Movimiento.SelectedItem = null;
            Total.Text = "";
        }
        private string Dinero()
        {
            if (Total.Text.Length > 0)
                return Total.Text.ToString();
            else return "0.00";
        }
        private string Fecha()
        {
            return DateTime.Today.ToString("dddd, dd MMMM yyyy") + " " + DateTime.Now.ToString("HH:MM tt");
        }

        private void RepartidorSelection(object sender, SelectionChangedEventArgs e)
        {
            repartidor = e.AddedItems[0].ToString();
        }
        private void CantidadSelection(object sender, SelectionChangedEventArgs e)
        {
            cantidad = e.AddedItems[0].ToString().Replace(" Pizza", "");
        }
        private void MovimientoSelection(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0].ToString() == "Repartidor Saliendo")
            {
                Total.PlaceholderText = "Ingrese el total del pedido que lleva";
                movimiento = "Salida con Pedido";
            }
            else
            {
                Total.PlaceholderText = "Ingrese el dinero entregado";
                movimiento = "Regresa con Entrega";
            }
        }
    }
}
