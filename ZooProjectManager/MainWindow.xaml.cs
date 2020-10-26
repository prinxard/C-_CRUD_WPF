using System;
using System.Collections.Generic;
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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ZooProjectManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlCon;
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["ZooProjectManager.Properties.Settings.ZooProjectDbConnectionString"].ConnectionString;
            sqlCon = new SqlConnection(connectionString);
            ShowZoos();
            ShowAnimals();
        }
        private void ShowZoos()
        {
            try
            {
                string query = "select * from Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlCon);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqlDataAdapter.Fill(zooTable);
                    ZooList.DisplayMemberPath = "Location";
                    ZooList.SelectedValuePath = "Id";
                    ZooList.ItemsSource = zooTable.DefaultView;
                }

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
           
        }
        private void ShowAssocAnimals() 
        {
            try
            {
                string query = "select * from Animal a inner join ZooAnimal za on a.Id = za.AnimalId where za.ZooId = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ZooList.SelectedValue);
                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);
                    AssocAnimList.DisplayMemberPath = "Name";
                    AssocAnimList.SelectedValuePath = "Id";
                    AssocAnimList.ItemsSource = animalTable.DefaultView;
                }

            }
            catch (Exception e)
            {

                //MessageBox.Show(e.ToString());
            }

        }
        private void ShowAnimals()
        {
            try
            {
                string query = "select * from Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlCon);

                using (sqlDataAdapter)
                {
                    DataTable allAnimalsTable = new DataTable();
                    sqlDataAdapter.Fill(allAnimalsTable);
                    AllAnimals.DisplayMemberPath = "Name";
                    AllAnimals.SelectedValuePath = "Id";
                    AllAnimals.ItemsSource = allAnimalsTable.DefaultView;
                }

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        private void ZooList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssocAnimals();
        }

        private void AllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Zoo where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ZooList.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            finally {
                sqlCon.Close();
                ShowZoos();
            }
                

        }
    }
}
