using JobOverview.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.Model
{
    static public class DAL
    {
        #region Méthodes privées

        #endregion

        #region Méthodes publiques
        static public List<Employee> GetListEmployee()
        {
            List<Employee> listEmployee = new List<Employee>();

            using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringDefault))
            {

            }

                return listEmployee;
        } 
        #endregion
    }
}
