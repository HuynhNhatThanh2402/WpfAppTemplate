using WpfAppTemplate.Models;

namespace WpfAppTemplate.Services
{
    public interface IDonViTinhService
    {
        Task<DonViTinh> GetDonViTinhById(int id);
        Task<IEnumerable<DonViTinh>> GetAllDonViTinh();
        Task AddDonViTinh(DonViTinh donViTinh);
        Task UpdateDonViTinh(DonViTinh donViTinh);
        Task DeleteDonViTinh(int id);
        Task<int> GenerateAvailableId();
    }
}