using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Kolos1.Services;

namespace Kolos1.DAL {

    public class SqlServerPatientsDbService : IPatientDbService{
        string connectionString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s18968;User ID=inzs18968;Password=admin123";

        public SqlServerPatientsDbService() {
        }

        public int deletePatient(int id) {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand()) {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try {
                    //Check if there are any prescriptions
                    List<string> prescriptionList = new List<string>();
                    command.CommandText = "select * from Prescription where IdPatient = " + id;
                    var dr = command.ExecuteReader();
                    while (dr.Read()) {
                        prescriptionList.Add(dr["IdPrescription"].ToString());
                    }

                    dr.Close();
                    foreach (string prescriptionId in prescriptionList) {
                        command.CommandText = "delete from Prescription_Medicament where IdPrescription = " + prescriptionId;
                        command.ExecuteNonQuery();
                    }

                    command.CommandText = "delete from Prescription where IdPatient = " + id;
                    command.ExecuteNonQuery();

                    command.CommandText = "delete from Patient where IdPatient = " + id;
                    command.ExecuteNonQuery();

                    dr.Close();
                    transaction.Commit();
                    return 200;
                } catch (SqlException sqlException) {
                    transaction.Rollback();
                    return 401;
                } catch (Exception exception) {
                    transaction.Rollback();
                    return 400;
                }
            }
        }
    }
}
