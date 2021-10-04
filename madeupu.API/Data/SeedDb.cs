using madeupu.API.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Data
{
    public class SeedDb 
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckDocumentTypeAsync();
            await CheckParticipationTypeAsync();
            await CheckProjectCategoryAsync();
            await CheckCountryAsync();
            /*await CheckBrandsAsync();
            await CheckProcedureAsync();
            await CheckRolesAsync();
            await CheckUsersAsync("1010", "Luis", "Salazar", "luis@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.Admin);
            await CheckUsersAsync("2020", "Juan", "Zuluaga", "zulu@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.User);
            await CheckUsersAsync("3030", "Ledys", "Bedoya", "ledys@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.User);
            await CheckUsersAsync("4040", "Sandra", "Salazar", "sandra@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.Admin);
            await CheckVehicleTypeAsync();*/
        }

        private async Task CheckCountryAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country { Name = "Colombia" });
                _context.Countries.Add(new Country { Name = "Brazil" });
                _context.Countries.Add(new Country { Name = "Perú" });
                _context.Countries.Add(new Country { Name = "Bolivia" });
                _context.Countries.Add(new Country { Name = "Venezuela" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckProjectCategoryAsync()
        {
            if (!_context.ProjectCategories.Any())
            {
                _context.ProjectCategories.Add(new ProjectCategory { Description = "Social" });
                _context.ProjectCategories.Add(new ProjectCategory { Description = "Privado" });
                _context.ProjectCategories.Add(new ProjectCategory { Description = "Tecnológico" });
                _context.ProjectCategories.Add(new ProjectCategory { Description = "Público" });
                _context.ProjectCategories.Add(new ProjectCategory { Description = "Productivo" });
                _context.ProjectCategories.Add(new ProjectCategory { Description = "Manufactura" });
                _context.ProjectCategories.Add(new ProjectCategory { Description = "Artistico" });
                _context.ProjectCategories.Add(new ProjectCategory { Description = "Cultural" });
                _context.ProjectCategories.Add(new ProjectCategory { Description = "Otro" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckParticipationTypeAsync()
        {
            if (!_context.ParticipationTypes.Any())
            {
                _context.ParticipationTypes.Add(new ParticipationType { Description = "Colaborador" });
                _context.ParticipationTypes.Add(new ParticipationType { Description = "Patrocinador" });
                _context.ParticipationTypes.Add(new ParticipationType { Description = "Inversor" });
                _context.ParticipationTypes.Add(new ParticipationType { Description = "Interesado" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckDocumentTypeAsync()
        {
            if (!_context.DocumentTypes.Any())
            {
                _context.DocumentTypes.Add(new DocumentType { Description = "Cédula" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Tarjeta de Identidad" });
                _context.DocumentTypes.Add(new DocumentType { Description = "NIT" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Pasaporte" });
                await _context.SaveChangesAsync();
            }
        }

    }
}
