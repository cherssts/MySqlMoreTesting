using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MySql_more_testing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            DisplayDataMenu();
            DisplaySells();
            DisplayUser();
        }
        #endregion

        #region functions
        public void DisplayDataMenu()
        {
            try
            {
                MySqlConnection con = MySqlConnectionClass.GetConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `menu`", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataMenu");
                _displayDataMenu.DataContext = ds;
                con.Close();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        public void DisplaySells()
        {
            try
            {
                MySqlConnection con = MySqlConnectionClass.GetConnection();
                MySqlCommand cmd = new MySqlCommand("Select `ventasdetalle`.`id`,`Id_Menu`,`Id_Venta`,`ventasdetalle`.`Precio`,`NombrePlato`,`cantidad`  From `ventasdetalle` left join `menu` on `Id_Menu` = `menu`.`CodigoPlato` order by `ventasdetalle`.`id` Asc", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet d = new DataSet();
                adp.Fill(d, "LoadDataBindingSells");
                _displayDataSells.DataContext = d;
                con.Close();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public void DisplayUser()
        {
            try
            {
                MySqlConnection con = MySqlConnectionClass.GetConnection();
                MySqlCommand cmd = new MySqlCommand("Select * From `usuario` left join `cargo` on `cargo_id` = `cargo`.`id`", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet d = new DataSet();
                adp.Fill(d, "UserBinding");
                _userDisplayData.DataContext = d;
                con.Close();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Events
        private void _mainWin_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }



        #endregion

        private void _addSell_Click(object sender, RoutedEventArgs e)
        {
            if (_code == null || _code.Text.Length < 2)
            {
                MessageBox.Show("Error");
                _code.Text = String.Empty;
                return;
            }
            if (_userCode == null || _userCode.Text.Length < 0)
            {
                MessageBox.Show("Error");
                _code.Text = String.Empty;
                return;
            }
            if (_amount.Text == String.Empty)
            {
                _amount.Text = "1";
            }
            if (_amount.Text.Length == 0)
            {
                MessageBox.Show("Error");
                _amount.Text = String.Empty;
                return;
            }
            
            try
            {
                SellRecording sr = new SellRecording(_code.Text, _userCode.Text, _amount.Text);
                MySqlConnectionClass._RecordDetailedSell(sr);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            _code.Text = String.Empty;
            _userCode.Text = String.Empty;
            _amount.Text = String.Empty;
            DisplayDataMenu();
            DisplaySells();
            DisplayUser();
            MySqlConnectionClass._ifContinueRecord = true;
        }

        private void _userCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemPlus)
            {
                e.Handled = true;
                MySqlConnectionClass._ifContinueRecord = false;
                MessageBox.Show("Receipt Finished.");
            }
        }

        private void _addPlateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_menuCode.Text.Length < 3)
            {
                MessageBox.Show("Error!\n"+"Foramt:\n"+"'1**' or '*01'", "Warning!!", MessageBoxButton.OK,MessageBoxImage.Warning);
                _menuCode.Text = String.Empty;
                return;
            }
            if (_plateName.Text.Length <= 0)
            {
                MessageBox.Show("Error!\n" + "The Plate Name Cannot be Empty!", "Warning!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                _plateName.Text = String.Empty;
                return;
            }
            if (_price.Text.Length <= 3)
            {
                MessageBox.Show("Wrong Format!\n" + "example:\n" + "1.99 or 10.00 or 20.25", "Warning!!",MessageBoxButton.OK,MessageBoxImage.Warning);
                _price.Text = String.Empty;
                return;
            }
            try
            {
                var recordMenu = new RecordMenuMedium(_menuCode.Text, _plateName.Text, _price.Text);
                MySqlConnectionClass._RecordMenu(recordMenu);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _menuCode.Text = String.Empty;
            _plateName.Text = String.Empty;
            _price.Text = String.Empty;
            DisplayDataMenu();
        }
    }
}
