using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;

namespace MVITO.Clases
{
    class Validacion
    {
        

        public static int definicion(char letra)
        {
            char[] especiales = { '!','"','·','$','%','&','/','(',')','=','|','@','#','~','€','¬','^','*' };

            int conteo = 1;

            for(int i = 0; i < 18; i++)
            {
                if(letra == especiales[i])
                {
                    conteo = 0;
                    return conteo;
                }
                else
                {
                    return conteo;
                }
            }


            return conteo;

        }
       

        public static void letras(KeyRoutedEventArgs caracter)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(caracter.Key));

            if ( (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122 || ascci == 8 || ascci == 32) && ascci != 15  )
            {

                    caracter.Handled = false;
                
          
            }

            else
            { 
                caracter.Handled = true;

            }
        }

        public static void letrasynumeros(KeyRoutedEventArgs caracter)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(caracter.Key));

            if ((ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122 || ascci == 8 || ascci == 32 || ascci >= 48 && ascci <= 57) && ascci != 15)
            {


                
                
                    caracter.Handled = false;
                

            }

            else
            {
                caracter.Handled = true;

            }
        }

        public static void numeros(KeyRoutedEventArgs caracter)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(caracter.Key));

            if ((ascci >= 48 && ascci <= 57 || ascci == 8 ) && ascci != 15)
            {




                caracter.Handled = false;
            }

            else
            {
                caracter.Handled = true;

            }
    
        }
    }
}


/*
  
 
 
 */
