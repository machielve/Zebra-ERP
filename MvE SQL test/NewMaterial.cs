using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MvE_SQL_test
{
    public partial class NewMaterial : Form
    {
        private void ClearForm()
        {
            txtMaterialName.Clear();
            txtMaterialID.Clear();
            this.parsedMaterialID = 0;
        }

        private int parsedMaterialID;

        private bool IsUnitNameValid()
        {
            if (txtMaterialName.Text == "")
            {
                MessageBox.Show("Please enter a name.");
                return false;
            }
            else
            {
                return true;
            }
        }

        public NewMaterial()
        {
            InitializeComponent();
        }

        private void btnFinnish_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNewMaterial_Click(object sender, EventArgs e)
        {
            if (IsUnitNameValid())
            {
                // Create the connection.
                string connectionstring = Properties.Settings.Default.connString;
                using (MySqlConnection connection = new MySqlConnection(connectionstring))
                {
                    using (MySqlCommand msqlcommand = new MySqlCommand("uspNewMaterial", connection))
                    {
                        msqlcommand.CommandType = CommandType.StoredProcedure;

                        msqlcommand.Parameters.Add(new MySqlParameter("MaterialName", MySqlDbType.Text));
                        msqlcommand.Parameters["MaterialName"].Value = txtMaterialName.Text;

                        msqlcommand.Parameters.Add(new MySqlParameter("MaterialID", MySqlDbType.Int32));
                        msqlcommand.Parameters["MaterialID"].Direction = ParameterDirection.Output;

                        try
                        {
                            connection.Open();

                            msqlcommand.ExecuteNonQuery();

                            this.parsedMaterialID = (int)msqlcommand.Parameters["MaterialID"].Value;
                            this.txtMaterialID.Text = Convert.ToString(parsedMaterialID);

                        }

                        catch (MySqlException ex)
                        {
                            MessageBox.Show("error " + ex.Number + " has occurd " + ex.Message);

                            MessageBox.Show("Material ID was not returned. Material could not be created.");
                        }

                        finally
                        {
                            connection.Close();
                        }

                    }
                }

            }
        }

        private void btnAddAnotherUnit_Click(object sender, EventArgs e)
        {
            this.ClearForm();
        }
    }
}
