using WpfAppTemplate.Models;

namespace WpfAppTemplate.Services
{
    public interface IThamSoService
    {
        Task<ThamSo> GetThamSo();
        Task UpdateThamSo(ThamSo thamSo);
        Task<int> GenerateAvailableId();
    }
}