using NetCoreLinqToSqlInjection.Models;

namespace NetCoreLinqToSqlInjection.Repositories
{
    public interface IRepositoryDoctores
    {

        List<Doctor> GetDoctores();

        Doctor GetDoctor(int idDoctor);

        void InsertDoctor(int id, string apellido, string especialidad
           , int salario, int idHospital);

        List<Doctor> GetDoctoresEspecialidad(string especialidad);


        void DeleteDoctor(int idDoctor);

        void UpdateDoctor(int idDoctor);
    }

}
