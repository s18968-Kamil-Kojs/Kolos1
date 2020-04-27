using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Kolos1.Services;

namespace Kolos1.DAL {

    public class SqlServerMedicamentsDbService : IMedicamentDbService{
        string connectionString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s18968;User ID=inzs18968;Password=admin123";

        public SqlServerMedicamentsDbService() {
        }

        public List<string> getMedicament(int id) {
            List<string> list = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand()) {
                command.Connection = connection;
                command.CommandText = "select * from Medicament where IdMedicament = "+id;

                connection.Open();
                var dr = command.ExecuteReader();

                if (!dr.Read()) {
                    return null;
                }

                string medicamentInfo = "";
                medicamentInfo += dr["IdMedicament"].ToString() + " ";
                medicamentInfo += dr["Name"].ToString() + " ";
                medicamentInfo += dr["Description"].ToString() + " ";
                medicamentInfo += dr["Type"].ToString() + " ";
                list.Add(medicamentInfo);
                list.Add("Dane o receptach:");

                dr.Close();
                command.CommandText = "select Prescription.IdPrescription, Prescription_Medicament.Dose, Prescription_Medicament.Details, Prescription.Date, Prescription.DueDate from Prescription inner join Prescription_Medicament on Prescription_Medicament.IdPrescription = Prescription.IdPrescription where Prescription_Medicament.IdMedicament = "+ id +" order by Date desc";
                dr = command.ExecuteReader();

                if (!dr.Read()) {
                    list.Add("Brak recept");
                } else {
                    //pierwszy juz wczytany w dr wiec go odczytujemy
                    string idPrescription = dr["IdPrescription"].ToString();
                    string dose = dr["Dose"].ToString();
                    string details = dr["Details"].ToString();
                    string date = dr["Date"].ToString();
                    string dueDate = dr["DueDate"].ToString();

                    list.Add(idPrescription + " " + dose + " " + details + " " + date + " " + dueDate);

                    //czytamy reszte
                    while (dr.Read()) {
                        idPrescription = dr["IdPrescription"].ToString();
                        dose = dr["Dose"].ToString();
                        details = dr["Details"].ToString();
                        date = dr["Date"].ToString();
                        dueDate = dr["DueDate"].ToString();

                        list.Add(idPrescription + " " + dose + " " + details + " " + date + " " + dueDate);
                    }
                }

                return list;
            }
        }

    }
}
