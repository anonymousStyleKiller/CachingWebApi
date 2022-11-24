﻿using CachingWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CachingWebApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Driver> Drivers { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }
}