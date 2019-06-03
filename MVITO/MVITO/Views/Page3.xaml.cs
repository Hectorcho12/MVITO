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
    public sealed partial class Page3 : Page
    {
        public Page3()
        {
            this.InitializeComponent();
        }
        private void Agregar(object sender, RoutedEventArgs e)
        {
            //execute Perdida 'tipo','Nombre de perdida','Descripcion','Fecha',100        
            Connection sql = new Connection();
            sql.EXECUTE(string.Format("EXECUTE Perdida '{0}','{1}','{2}',{3}", PerdidaTxt(), DescriptionTxt(), Fecha(), TotalTxt()));
            Perdida.Text = "";
            Descripcion.Text = "";
            Total.Text = "";
        }
        private string PerdidaTxt()
        {
            //Nombre de Compra
            if (Perdida.Text.Length > 0) return Perdida.Text;
            else return "Perdida sin Especificar";
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
    }
}
