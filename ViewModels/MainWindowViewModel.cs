using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfAppTemplate.Models;
using WpfAppTemplate.Services;
using WpfAppTemplate.Commands;
using WpfAppTemplate.Views;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System;


namespace WpfAppTemplate.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //To chua hieu cai nay ne
        private readonly IDaiLyService _dailyService;
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<int, ChinhSuaDaiLyViewModel> _chinhSuaDaiLyFactory;
        public MainWindowViewModel(
            IDaiLyService dailyService,
            IServiceProvider serviceProvider,
            Func<int, ChinhSuaDaiLyViewModel> chinhSuaDaiLyFactory)
        {
            _dailyService = dailyService;
            _ = LoadData();

            DeleteDaiLyCommand = new RelayCommand(OpenDeleteDaiLyWindow);
            EditDaiLyCommand = new RelayCommand(OpenChinhSuaDaiLyWindow);
            OpenHoSoDaiLyCommand = new RelayCommand(OpenHoSoDaiLyWindow);
            LoadDataCommand = new RelayCommand(async () => await LoadDataExecute());
            _serviceProvider = serviceProvider;
            _chinhSuaDaiLyFactory = chinhSuaDaiLyFactory;
        }

        private ObservableCollection<DaiLy> _danhSachDaiLy = [];
        public ObservableCollection<DaiLy> DanhSachDaiLy
        {
            get => _danhSachDaiLy;
            set
            {
                _danhSachDaiLy = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadData()
        {
            var list = await _dailyService.GetAllDaiLy();
            DanhSachDaiLy = [.. list];
            SelectedDaiLy = null!;
        }

        public ICommand OpenHoSoDaiLyCommand { get; }
        public ICommand EditDaiLyCommand { get; }
        public ICommand DeleteDaiLyCommand { get; }
        //public ICommand SearchDaiLyCommand { get; }
        public ICommand LoadDataCommand { get; }

        private void OpenHoSoDaiLyWindow()
        {
            SelectedDaiLy = null!;

            var hoSoDaiLyWindow = _serviceProvider.GetRequiredService<HoSoDaiLyWinDow>();

            if (hoSoDaiLyWindow.DataContext is HoSoDaiLyViewModel viewModel)
            {
                viewModel.DataChanged += async (sender, e) => await LoadData();
            }

            hoSoDaiLyWindow.Show();
        }

        private async Task LoadDataExecute()
        {
            await LoadData();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void OpenDeleteDaiLyWindow()
        {
            if (string.IsNullOrEmpty(SelectedDaiLy.TenDaiLy))
            {
                MessageBox.Show("Vui lòng chọn đại lý để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa đại lý '{SelectedDaiLy.TenDaiLy}'?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _dailyService.DeleteDaiLy(SelectedDaiLy.MaDaiLy);
                    await LoadData();
                    MessageBox.Show("Đã xóa đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể xóa đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*private void OpenSearchDaiLyWindow()
        {
            SelectedDaiLy = null!;

            var traCuuDaiLyWindow = _serviceProvider.GetRequiredService<TraCuuDaiLyWindow>();

            if (traCuuDaiLyWindow.DataContext is TraCuuDaiLyViewModel viewModel)
            {
                viewModel.SearchCompleted += (sender, searchResults) =>
                {
                    if (searchResults.Count > 0)
                    {
                        DanhSachDaiLy = searchResults;
                    }
                };
            }

            traCuuDaiLyWindow.Show();
        }*/

        private DaiLy _selectedDaiLy = new();
        public DaiLy SelectedDaiLy
        {
            get => _selectedDaiLy;
            set
            {
                _selectedDaiLy = value;
                OnPropertyChanged();
            }
        }

        private void OpenChinhSuaDaiLyWindow()
        {
            if (SelectedDaiLy == null || string.IsNullOrEmpty(SelectedDaiLy.TenDaiLy))
            {
                MessageBox.Show("Vui lòng chọn đại lý để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var viewModel = _chinhSuaDaiLyFactory(SelectedDaiLy.MaDaiLy);
                viewModel.DataChanged += async (sender, e) => await LoadData();

                var window = new CapNhatDaiLyWindow(viewModel);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening edit window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}