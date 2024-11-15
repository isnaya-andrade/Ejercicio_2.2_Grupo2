

namespace Ejercicio_2._2_Grupo2.Models
{
    public class Firma
    {
        public int Id { get; set; }

        public byte[]? FirmaDigital { get; set; }

        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }


        public Firma(byte[]? firmaDigital, string? nombre, string? descripcion)
        {
            FirmaDigital = firmaDigital;
            Descripcion = descripcion;

            Nombre = nombre;
        }

        
        public Firma() { }
    }
}
