using System.ComponentModel.DataAnnotations;

namespace WpfAppTemplate.Models
{
    public class ThamSo
    {
        [Key]
        public int Id { get; set; } = 0;
        public int SoLuongDaiLyToiDa { get; set; } = 0;
        public bool QuyDinhTienThuTienNo { get; set; } = true;
    }
}
