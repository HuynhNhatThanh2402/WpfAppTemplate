﻿using Microsoft.EntityFrameworkCore;
using WpfAppTemplate.Configs;
using WpfAppTemplate.Data;
using WpfAppTemplate.Models;
using WpfAppTemplate.Services;

namespace WpfAppTemplate.Repositories
{
    public class DonViTinhRepository : IDonViTinhService
    {
        private readonly DataContext _context;

        public DonViTinhRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task AddDonViTinh(DonViTinh donViTinh)
        {
            _context.DsDonViTinh.Add(donViTinh);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDonViTinh(int id)
        {
            var donViTinh = await _context.DsDonViTinh.FindAsync(id);
            if (donViTinh != null)
            {
                _context.DsDonViTinh.Remove(donViTinh);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DonViTinh>> GetAllDonViTinh()
        {
            return await _context.DsDonViTinh
                .Include(d => d.DsMatHang)
                .ToListAsync();
        }

        public async Task<DonViTinh> GetDonViTinhById(int id)
        {
            DonViTinh? donViTinh = await _context.DsDonViTinh
                .Include(d => d.DsMatHang)
                .FirstOrDefaultAsync(d => d.MaDonViTinh == id);
            return donViTinh ?? throw new Exception("DonViTinh not found!");
        }

        public async Task<DonViTinh> GetDonViTinhByTenDonViTinh(string tenDonViTinh)
        {
            DonViTinh? donViTinh = await _context.DsDonViTinh
                .Include(d => d.DsMatHang)
                .FirstOrDefaultAsync(d => d.TenDonViTinh == tenDonViTinh);
            return donViTinh ?? throw new Exception("DonViTinh not found!");
        }

        public async Task UpdateDonViTinh(DonViTinh donViTinh)
        {
            _context.Entry(donViTinh).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> GenerateAvailableId()
        {
            int maxId = await _context.DsDonViTinh.MaxAsync(d => d.MaDonViTinh);
            return maxId + 1;
        }
    }
}