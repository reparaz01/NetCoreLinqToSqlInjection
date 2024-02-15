using NetCoreLinqToSqlInjection.Models;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;

#region
/*

create procedure SP_DELETE_DOCTOR
(@iddoctor int)
as
	delete from DOCTOR where DOCTOR_NO=@iddoctor
go



create procedure SP_UPDATE_DOCTOR
(@iddoctor INT, @idhospital INT, @apellido NVARCHAR(50), @especialidad NVARCHAR(50), @salario INT)
AS
	UPDATE Doctor
    SET 
        HOSPITAL_COD = @idhospital,
        APELLIDO = @apellido,
        ESPECIALIDAD = @especialidad,
        SALARIO = @salario
    WHERE 
        DOCTOR_NO = @iddoctor;
GO




*/
#endregion



namespace NetCoreLinqToSqlInjection.Repositories
{

    public class RepositoryDoctoresSqlServer : IRepositoryDoctores
    {
        private DataTable tablaDoctores;
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public RepositoryDoctoresSqlServer()

        {

            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITALES;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.tablaDoctores = new DataTable();
            string sql = "select * from DOCTOR";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tablaDoctores);
        }

 

        public List<Doctor> GetDoctores()

        {

            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();

            foreach (var row in consulta)

            {
                Doctor doc = new Doctor
                {

                    IdDoctor = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO"),
                    IdHospital = row.Field<int>("HOSPITAL_COD")

                };

                doctores.Add(doc);

            }

            return doctores;

        }

        public List<Doctor> GetDoctoresEspecialidad(string especialidad)
        {
            throw new NotImplementedException();
        }

        public void InsertDoctor

            (int id, string apellido, string especialidad
            , int salario, int idHospital)

        {
            string sql = "insert into DOCTOR values (@idhospital, @iddoctor, @apellido "
                + ", @especialidad, @salario)";

            this.com.Parameters.AddWithValue("@idhospital", idHospital);
            this.com.Parameters.AddWithValue("@iddoctor", id);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

        }

        public void DeleteDoctor(int idDoctor)
        {
            this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_DOCTOR";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }


        public void UpdateDoctor(int idDoctor)
        {

            this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_UPDATE_DOCTOR";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

        }

        public Doctor GetDoctor(int idDoctor)
        {
            
                string sql = "select * from DEPT where DEPT_NO = @id";
                this.com.Parameters.AddWithValue("@id", idDoctor);
                this.com.CommandType = CommandType.Text;
                this.com.CommandText = sql;
                this.cn.Open();
                this.reader = this.com.ExecuteReader();

                Doctor doc = null;
                if (this.reader.Read())
                {

                doc = new Doctor();
                doc.IdDoctor = int.Parse(this.reader["DOCT"].ToString());
                doc.IdHospital = this.reader["HOSPITAL_COD"].ToString();
                doc.Apellido = this.reader["APELLIDO"].ToString();
                doc.Especialidad = this.reader["ESPECIALIDAD"].ToString();
                doc.Salario = int.Parse(this.reader["SALARIO"].ToString());


            }
                else
                {
                    //NO HAY DATOS
                }

                await this.reader.CloseAsync();
                await this.cn.CloseAsync();
                this.com.Parameters.Clear();
                return dept;

            
        }
    }

}


