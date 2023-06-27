using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace DU_Summer_2023_Capstone.Controllers
{
    public partial class RegistrationController : System.Web.UI.Page
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySQL.Data.MySqlClient.MySqlCommand cmd;
        String queryStr;

        protected void Page_load(object sender, EventArgs e)
        {

        }

        protected void registerEventMethod(object sender, EventArgs e)
        {
            registerUser();

        }

        private void registerUser()
        {
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["PizzaTPSDBConnString"].ToString();

            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            queryStr = "";

            queryStr = "INSERT INTO pizzeria_tps_db.user_info (User_Email, User_Password)";
                "VALUES('" + emailTextBox.Text + "', '" + passwordTextBox.Text + "')";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);

            cmd.ExecuteReader();
        }
    }
}

