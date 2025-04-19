using Microsoft.Extensions.DependencyInjection;
using WpfAppTemplate.ViewModels;
using WpfAppTemplate.Views;
using WpfAppTemplate.Configs;
using WpfAppTemplate.Helpers;
using WpfAppTemplate.Repositories;
using WpfAppTemplate.Services;


namespace WpfAppTemplate.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Register database
            services.AddSingleton<DatabaseConfig>();
            // Register repositories and services
            services.AddScoped<IDaiLyService, DaiLyRepository>();
            services.AddScoped<ILoaiDaiLyService, LoaiDaiLyRepository>();
            services.AddScoped<IQuanService, QuanRepository>();
            services.AddScoped<IPhieuThuService, PhieuThuRepository>();
            services.AddScoped<IPhieuXuatService, PhieuXuatRepository>();
            services.AddScoped<IMatHangService, MatHangRepository>();
            services.AddScoped<IDonViTinhService, DonViTinhRepository>();
            services.AddScoped<IThamSoService, ThamSoRepository>();
            services.AddScoped<IChiTietPhieuXuatService, ChiTietPhieuXuatRepository>();

            // Register helpers
            services.AddSingleton<ComboBoxItemConverter>();

            // Register ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<HoSoDaiLyViewModel>();
            services.AddTransient<Func<int, ChinhSuaDaiLyViewModel>>(sp => dailyId =>
            new ChinhSuaDaiLyViewModel(
                sp.GetRequiredService<IDaiLyService>(),
                sp.GetRequiredService<IQuanService>(),
                sp.GetRequiredService<ILoaiDaiLyService>(),
                dailyId
            )
        );

            // Register View
            services.AddTransient<MainWindow>();
            services.AddTransient<HoSoDaiLyWinDow>();
            services.AddTransient<CapNhatDaiLyWindow>();
            return services;
        }
    }
}
