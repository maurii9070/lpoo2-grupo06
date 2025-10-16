using System.ComponentModel;
using ClasesBase.Repositories;

namespace ClasesBase.Entidades
{
    public class Usuario : INotifyPropertyChanged
    {
        public int Usu_ID { get; set; }
        public string Usu_NombreUsuario { get; set; }
        public string Usu_Contrasenia { get; set; }
        public string Usu_ApellidoNombre { get; set; }
        public int Rol_ID { get; set; }

        public string RolNombre
        {
            get
            {
                var rol = new RolRepository().Obtener(Rol_ID);
                return rol != null ? rol.Rol_Descripcion : "Sin rol";
            }
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}