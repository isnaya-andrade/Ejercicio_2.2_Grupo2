using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ejercicio_2._2_Grupo2.Models
{
    public class Firma : INotifyPropertyChanged
    {
        private int _id;
        private byte[]? _firmaDigital;
        private string? _nombre;
        private string? _descripcion;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public byte[]? FirmaDigital
        {
            get => _firmaDigital;
            set
            {
                if (_firmaDigital != value)
                {
                    _firmaDigital = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Nombre
        {
            get => _nombre;
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Descripcion
        {
            get => _descripcion;
            set
            {
                if (_descripcion != value)
                {
                    _descripcion = value;
                    OnPropertyChanged();
                }
            }
        }

        public Firma(byte[]? firmaDigital, string? nombre, string? descripcion)
        {
            FirmaDigital = firmaDigital;
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public Firma() { }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool TieneFirmaValida()
        {
            return FirmaDigital != null && FirmaDigital.Length > 0;
        }
    }
}