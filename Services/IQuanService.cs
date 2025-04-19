using WpfAppTemplate.Models;

namespace WpfAppTemplate.Services
{
    public interface IQuanService
    {
        Task<Quan> GetQuanById(int id);
        Task<IEnumerable<Quan>> GetAllQuan();
        Task AddQuan(Quan quan);
        Task UpdateQuan(Quan quan);
        Task DeleteQuan(int id);
        Task<int> GetSoLuongDaiLyTrongQuan(int id);
        Task<int> GenerateAvailableId();
    }
}