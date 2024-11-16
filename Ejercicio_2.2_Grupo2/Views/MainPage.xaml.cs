using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Diagnostics;
using Ejercicio_2._2_Grupo2.Models;
using Ejercicio_2._2_Grupo2.Views;


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

        private async Task SaveDataAsync()
        {
            if (signaturePad == null || !signaturePad.IsBlank())
            {
                try
                {

                    if (string.IsNullOrWhiteSpace(txtNombre.Text))
                    {
                        await DisplayAlert("Error", "El nombre es requerido", "OK");
                        return;
                    }

                    var nombre = txtNombre.Text.Trim();
                    var description = txtDescripcion.Text?.Trim() ?? string.Empty;

                    using var signatureImage = await signaturePad.GetImageStreamAsync();

                    if (signatureImage == null)
                    {
                        await DisplayAlert("Error", "No se pudo obtener la imagen de la firma", "OK");
                        return;
                    }

                    byte[] signatureData;
                    using (var memoryStream = new MemoryStream())
                    {
                        await signatureImage.CopyToAsync(memoryStream);
                        signatureData = memoryStream.ToArray();
                    }

                    if (signatureData == null || signatureData.Length == 0)
                    {
                        await DisplayAlert("Error", "La firma está vacía", "OK");
                        return;
                    }

                    var firma = new Firma(signatureData, nombre, description);
                    await Task.Run(() => new FirmaDao().guardar(firma));

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        txtDescripcion.Text = string.Empty;
                        txtNombre.Text = string.Empty;
                        signaturePad.Clear();
                    });

                    await DisplayAlert("Registro Exitoso", "¡La firma se creó con éxito!", "Aceptar");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al guardar la firma: {ex.Message}");
                    Debug.WriteLine($"StackTrace: {ex.StackTrace}");

                    string errorMessage = "Error al guardar la firma";
                    if (Debugger.IsAttached)
                    {
                        errorMessage += $"\n{ex.Message}";
                    }

                    await DisplayAlert("Error", errorMessage, "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("Error", "Por favor, dibuje una firma antes de guardar", "OK");
            }
        }

        private bool Validar()
        {
            return !string.IsNullOrWhiteSpace(txtDescripcion.Text) && !signaturePad.IsBlank();
        }

        private async void BtnMostrar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListPage());
           
        }
    }
}
