﻿using Microsoft.EntityFrameworkCore;
using WpfAppTemplate.Models;
using WpfAppTemplate.Configs;
using WpfAppTemplate.Data;
using WpfAppTemplate.Services;

namespace WpfAppTemplate.Repositories
{
    public class PhieuXuatRepository : IPhieuXuatService
    {
        private readonly DataContext _context;

        public PhieuXuatRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task AddPhieuXuat(PhieuXuat phieuXuat)
        {
            _context.DsPhieuXuat.Add(phieuXuat);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePhieuXuat(int id)
        {
            var phieuXuat = await _context.DsPhieuXuat.FindAsync(id);
            if (phieuXuat != null)
            {
                _context.DsPhieuXuat.Remove(phieuXuat);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PhieuXuat>> GetAllPhieuXuat()
        {
            return await _context.DsPhieuXuat
                .Include(p => p.DaiLy)
                .Include(p => p.DsChiTietPhieuXuat)
                    .ThenInclude(c => c.MatHang)
                        .ThenInclude(m => m.DonViTinh)
                .ToListAsync();
        }

        public async Task<PhieuXuat> GetPhieuXuatById(int id)
        {
            PhieuXuat? phieuXuat = await _context.DsPhieuXuat
                .Include(p => p.DaiLy)
                .Include(p => p.DsChiTietPhieuXuat)
                    .ThenInclude(c => c.MatHang)
                        .ThenInclude(m => m.DonViTinh)
                .FirstOrDefaultAsync(p => p.MaPhieuXuat == id);
            return phieuXuat ?? throw new Exception("PhieuXuat not found!");
        }

        public async Task UpdatePhieuXuat(PhieuXuat phieuXuat)
        {
            _context.Entry(phieuXuat).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PhieuXuat>> GetPhieuXuatByDaiLyId(int maDaiLy)
        {
            return await _context.DsPhieuXuat
                .Include(p => p.DaiLy)
                .Include(p => p.DsChiTietPhieuXuat)
                    .ThenInclude(c => c.MatHang)
                        .ThenInclude(m => m.DonViTinh)
                .Where(p => p.MaDaiLy == maDaiLy)
                .ToListAsync();
        }

        public async Task<IEnumerable<PhieuXuat>> GetPhieuXuatByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.DsPhieuXuat
                .Include(p => p.DaiLy)
                .Include(p => p.DsChiTietPhieuXuat)
                    .ThenInclude(c => c.MatHang)
                        .ThenInclude(m => m.DonViTinh)
                .Where(p => p.NgayLapPhieu >= startDate && p.NgayLapPhieu <= endDate)
                .ToListAsync();
        }

        public async Task<int> GenerateAvailableId()
        {
            int maxId = await _context.DsPhieuXuat.MaxAsync(d => d.MaPhieuXuat);
            return maxId + 1;
        }
    }
}