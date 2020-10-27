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
        private void ShowSelectedZooInTextbox()
        {
            try
            {
                string query = "select location from Zoo where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ZooList.SelectedValue);
                    DataTable zooDataTable = new DataTable();
                    sqlDataAdapter.Fill(zooDataTable);
                    zooEntryTextBox.Text = zooDataTable.Rows[0]["Location"].ToString();
                   
                }

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }
        private void ShowSelectedAnimalInTextbox()
        {
            try
            {
                string query = "select Name from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", AllAnimals.SelectedValue);
                    DataTable animalDataTable = new DataTable();
                    sqlDataAdapter.Fill(animalDataTable);
                    zooEntryTextBox.Text = animalDataTable.Rows[0]["Name"].ToString();

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
            ShowSelectedZooInTextbox();
           
        }
        private void AllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextbox();
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

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Zoo values (@Location)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                sqlCommand.Parameters.AddWithValue("@Location", zooEntryTextBox.Text);
                sqlCommand.ExecuteScalar();
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlCon.Close();
                ShowZoos();
            }

        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into ZooAnimal values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ZooList.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", AllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlCon.Close();
                ShowAssocAnimals();
            }
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", AssocAnimList.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlCon.Close();
                ShowAssocAnimals();
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Animal values (@Name)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                sqlCommand.Parameters.AddWithValue("@Name", zooEntryTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlCon.Close();
                ShowAnimals();
            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", AllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlCon.Close();
                ShowAnimals();
            }
        }

      
    }
}
