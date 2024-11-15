using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Diagnostics;
using Ejercicio_2._2_Grupo2.Models;


namespace Ejercicio_2._2_Grupo2
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            if (Validar())
            {
                await SaveDataAsync();
            }
            else
            {
                await DisplayAlert("Advertencia", "Escriba una descripción y dibuje una firma.", "Cerrar");
            }
        }

        private async System.Threading.Tasks.Task SaveDataAsync()
        {
            try
            {
                var nombre = txtNombre.Text;
                var description = txtDescripcion.Text;
                var signatureImage = await signaturePad.GetImageStreamAsync();
                byte[] signatureData;

                using (var memoryStream = new MemoryStream())
                {
                    await signatureImage.CopyToAsync(memoryStream);
                    signatureData = memoryStream.ToArray();
                }

                (new FirmaDao()).guardar(new Firma(signatureData, nombre,description));

                txtDescripcion.Text = string.Empty;
                txtNombre.Text = string.Empty;
                signaturePad.Clear();

                await DisplayAlert("Registro Exitoso", "¡La firma se creó con éxito!", "Aceptar");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex}");
                await DisplayAlert("Error", "¡Error al guardar firma!", "Aceptar");
            }
        }

        private bool Validar()
        {
            return !string.IsNullOrWhiteSpace(txtDescripcion.Text) && !signaturePad.IsBlank();
        }

        private async void BtnMostrar_Clicked(object sender, EventArgs e)
        {
            try
            {
                (new FirmaDao()).ver(firmas => {
                    if(firmas != null && firmas.Count > 0)
                        DisplayAlert("Info", $"Firmas: {firmas.Last().Nombre}, {firmas.Last().Descripcion}", "OK");
                });
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar las firmas: {ex.Message}", "OK");
            }
        }
    }
}
