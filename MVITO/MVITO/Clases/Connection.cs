using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVITO.Clases
{
    class Connection
    { 
        private SqlConnection conexion = new SqlConnection((App.Current as App).ConnectionString);

        
        public Connection() { }

        // Funcion Primitiva
        public DataSet Conexion(string instruccion)
        {
            conexion.Open();
            DataSet DS = new DataSet();
            SqlDataAdapter DP = new SqlDataAdapter(instruccion, conexion);
            DP.Fill(DS);
            conexion.Close();
            return DS;
        }
        //Crea un nuevo Cliente en la agenda
        public bool EXECUTE(string instruction)
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand(instruction, conexion);
            int filasafectadas = cmd.ExecuteNonQuery();
            conexion.Close();
            if (filasafectadas > 0) return true;
            else return false;
        }

        public bool comprobar(string query)
        {
            conexion.Open();
            SqlDataAdapter  Sda = new SqlDataAdapter(query, conexion);
            DataTable table = new DataTable();

            Sda.Fill(table);
            conexion.Close();


            if (table.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

            

           


        }   


    }
}
