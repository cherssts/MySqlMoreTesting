using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace MySql_more_testing
{
    public class MySqlConnectionClass
    {

        public static MySqlConnection GetConnection()
        {
            string _mySqlString = "Server=localhost;Port=3306;UID=root;Password=115320162abc;database=restaurant2";
            MySqlConnection con = new MySqlConnection(_mySqlString);
            try
            {
                con.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "MySqlConnection", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return con;
        }

        public static void _RecordMenu(RecordMenuMedium a)
        {

            string sql = $"Insert into `Menu` values (null,'{a._menuID}','{a._menuName}','{a._menuPrice}')";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Your Menu item was added Successfully!", "MySql", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Your request has failed to accomplish\n" + ex.Message);
            }
            con.Close();
        }

        public static bool _ifContinueRecord = false;
        public static void _RecordDetailedSell(SellRecording sd)
        {
            //Bruh is the flag for if continueRecording things in a Receipt
            //Taking data From the Field _ifContinueRecord
            //And It will be controlled by the Clicked Event
            //From MainWIndow.xamal.cs
            bool bruh = _ifContinueRecord;
            //Getting Connection toward database 
            MySqlConnection con = GetConnection();
            MySqlCommand cmd;
            //Using Try Catch to catch any error that is gonna happen
            //Not let the program die if something is wrong
            try
            {
                //Creating an instance for the MySqlCommand
                //And Writing the first command to be taken
                cmd = new MySqlCommand($"Select `id` from `usuario` where `CodigoUsuario` = {sd.UserCode}", con);


                //Executing the first command
                cmd.ExecuteNonQuery();
                //Read the data that needed
                MySqlDataReader _reader = cmd.ExecuteReader();
                //Set the data read in a variable
                _reader.Read();
                string id = _reader.GetString(0);
                con.Close();

                if (bruh == false)
                {
                    con.Open();
                    //String where the data is added
                    string commandLine = $"Insert into `ventas` values(Null,{id},now())";
                    //Setting the cmd's text to the commandLine
                    cmd.CommandText = commandLine;
                    //Execute the Insert Command
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                //if(bruh == true)
                //{
                //    con.Open();
                //    string commandLineCheckUser = $"Select `Id_Usuario` from `ventas`";
                //    cmd.CommandText = commandLineCheckUser;
                //    cmd.ExecuteNonQuery();
                //    MySqlDataReader _readerCheck = cmd.ExecuteReader();
                //    _readerCheck.Read();
                //    if (sd.UserCode != _readerCheck.GetInt32(0).ToString())
                //    {
                //        MessageBox.Show("The UserCode Does not match with the one who is incharged.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                //        con.Clone();
                //        return;
                //    }
                //    con.Close();
                //}

                con.Open();
                //String where we Get the Sell Id in order to let the Sell detail
                //have the Data needed. Limiting the amount to 1 to get the one we need
                string commandLine2 = $"Select `id` From `ventas` order by `id` desc Limit 1";
                //Set the command text to commandLine2
                cmd.CommandText = commandLine2;
                //Execute the Line
                cmd.ExecuteNonQuery();
                //Let the DataReader get the new one and set it
                //to the Class property 
                MySqlDataReader _reader2 = cmd.ExecuteReader();
                _reader2.Read();
                sd.SellId = _reader2.GetInt32(0);
                con.Close();

                //Open Again the connection for another reader to be able to use
                con.Open();
                //This commandLine is use to get the price of the plate
                string CommandLineExtra = $"Select `Precio` from `menu` where `CodigoPlato` = {sd.MenuCode}";
                cmd.CommandText = CommandLineExtra;
                cmd.ExecuteNonQuery();
                //Execute the Command and read the data
                MySqlDataReader _reader3 = cmd.ExecuteReader();
                _reader3.Read();
                //Having the amount's input multiplying it to the price
                double _totalprice = (_reader3.GetInt64(0)) * (double.Parse(sd.Amount));
                con.Close();

                con.Open();
                //The text where we Insert the Data into the Sell Detail
                string commandLine3 = $"Insert into `ventasdetalle` values(null,{sd.SellId.ToString()},{sd.MenuCode},{sd.Amount},{_totalprice})";
                //Set the command text to commandLine3
                cmd.CommandText = commandLine3;
                //Execute it
                cmd.ExecuteNonQuery();

                //Finally Finish the Circuit
                con.Close();
                sd.MenuCode = string.Empty;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error!" + ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
