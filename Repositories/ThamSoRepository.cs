﻿using Microsoft.EntityFrameworkCore;
using WpfAppTemplate.Models;
using WpfAppTemplate.Configs;
using WpfAppTemplate.Data;
using WpfAppTemplate.Services;

namespace WpfAppTemplate.Repositories
{
    public class ThamSoRepository : IThamSoService
    {
        private readonly DataContext _context;

        public ThamSoRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        public async Task<ThamSo> GetThamSo()
        {
            ThamSo? thamSo = await _context.DsThamSo.FirstOrDefaultAsync();
            return thamSo ?? throw new Exception("ThamSo not found!");
        }

        public async Task UpdateThamSo(ThamSo thamSo)
        {
            _context.Entry(thamSo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task InitializeThamSo(ThamSo thamSo)
        {
            if (!await _context.DsThamSo.AnyAsync())
            {
                _context.DsThamSo.Add(thamSo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GenerateAvailableId()
        {
            int maxId = await _context.DsThamSo.MaxAsync(d => d.Id);
            return maxId + 1;
        }
    }
}