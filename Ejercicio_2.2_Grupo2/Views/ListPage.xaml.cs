using Ejercicio_2._2_Grupo2.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Ejercicio_2._2_Grupo2.Views
{
    public partial class ListPage : ContentPage
    {
        public ObservableCollection<Firma> SignaturesList { get; private set; }

        // Datos de prueba de una imagen simple en formato PNG
        private readonly byte[] sampleSignatureBytes = new byte[] {
            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D,
            0x49, 0x48, 0x44, 0x52, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
            0x08, 0x06, 0x00, 0x00, 0x00, 0x1F, 0x15, 0xC4, 0x89, 0x00, 0x00, 0x00,
            0x0D, 0x49, 0x44, 0x41, 0x54, 0x08, 0xD7, 0x63, 0x60, 0x60, 0x60, 0x60,
            0x00, 0x00, 0x00, 0x05, 0x00, 0x01, 0x0D, 0x0A, 0x2D, 0xB4, 0x00, 0x00,
            0x00, 0x00, 0x49, 0x45, 0x4E, 0x44, 0xAE, 0x42, 0x60, 0x82
        };

        public ListPage()
        {
            InitializeComponent();
            InitializeSignatures();
        }

        private void InitializeSignatures()
        {
            try
            {
                SignaturesList = new ObservableCollection<Firma>();
                var firmasDao = new FirmaDao();

                firmasDao.ver(firmas =>
                {
                    if (firmas != null)
                    {
                        foreach (var firma in firmas)
                        {
                            if (firma != null)
                            {
                            
                                SignaturesList.Add(new Firma(
                                    firma.FirmaDigital ?? sampleSignatureBytes,  
                                    firma.Nombre ?? "Firma de prueba",
                                    firma.Descripcion ?? "Descripción de prueba"
                                ));
                            }
                        }
                    }
                });

                if (SignaturesList.Count == 0)
                {
                    SignaturesList.Add(new Firma(
                        sampleSignatureBytes,
                        "Firma de prueba 1",
                        "Esta es una firma de prueba"
                    ));
                }

                this.BindingContext = this;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar las firmas: {ex.Message}");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Error", "No se pudieron cargar las firmas", "OK");
                });
            }
        }
    }
}