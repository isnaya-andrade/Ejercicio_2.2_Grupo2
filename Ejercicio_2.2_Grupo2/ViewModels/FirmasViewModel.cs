using Ejercicio_2._2_Grupo2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ejercicio_2._2_Grupo2.ViewModels
{
    public class FirmasViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseContext _database;
        private ObservableCollection<Firma> _firmas;
        private bool _isLoading;
        private string _searchText;

        public ObservableCollection<Firma> Firmas
        {
            get => _firmas;
            set
            {
                _firmas = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
      
            }
        }

        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public FirmasViewModel(DatabaseContext database)
        {
            _database = database;
            Firmas = new ObservableCollection<Firma>();
            DeleteCommand = new Command<Firma>(async (firma) => await DeleteFirma(firma));
            RefreshCommand = new Command(async () => await LoadFirmas());
        }

        public async Task LoadFirmas()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                var firmas = await _database.GetFirmasAsync();
                Firmas.Clear();

                foreach (var firma in firmas)
                {
                    var firmaVM = new Firma
                    {
                        Id = firma.Id,
                        Descripcion = firma.Descripcion,
                        
                    };
                    Firmas.Add(firmaVM);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading firmas: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteFirma(Firma firma)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar",
                "¿Está seguro de eliminar esta firma?",
                "Sí",
                "No");

            if (!confirm) return;

            try
            {
                Firmas.Remove(firma);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Error al eliminar la firma: {ex.Message}",
                    "OK");
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
