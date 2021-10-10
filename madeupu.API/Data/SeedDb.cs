using madeupu.API.Data.Entities;
using madeupu.API.Enums;
using madeupu.API.Helpers;
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
        private readonly IUserHelper _iuserHelper;

        public SeedDb(DataContext context, IUserHelper iuserHelper)
        {
            _context = context;
            _iuserHelper = iuserHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckDocumentTypeAsync();
            await CheckParticipationTypeAsync();
            await CheckProjectCategoryAsync();
            await CheckCountryAsync();
            await CheckRegionAsync();
            await CheckCityAsync();
            await CheckRolesAsync();
            await CheckUsersAsync("1010", "Luis", "Salazar", "luis@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.Admin);
            await CheckUsersAsync("2020", "Juan", "Zuluaga", "zulu@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.User);
            await CheckUsersAsync("3030", "Ledys", "Bedoya", "ledys@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.User);
            await CheckUsersAsync("4040", "Sandra", "Salazar", "sandra@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.Admin);
            /*await CheckBrandsAsync();
            await CheckProcedureAsync();
            await CheckVehicleTypeAsync();*/
        }

        

        private async Task CheckUsersAsync(string document, string firstName, string lastName, string email, string phoneNumber, string address, UserType userType)
        {
            User user = await _iuserHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Address = address,
                    Document = document,
                    DocumentType = _context.DocumentTypes.FirstOrDefault(x => x.Description == "Cédula"),
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    UserName = email,
                    UserType = userType
                };
                await _iuserHelper.AddUserAsync(user, "123456");
                await _iuserHelper.AddUserToRoleAsync(user, userType.ToString());

                //string token = await _iuserHelper.GenerateEmailConfirmationTokenAsync(user);
                //await _iuserHelper.ConfirmEmailAsync(user, token);
            }
        }

        private async Task CheckRolesAsync()
        {
            await _iuserHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _iuserHelper.CheckRoleAsync(UserType.User.ToString());
        }
        private async Task CheckCountryAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country { Name = "Colombia" });
                _context.Countries.Add(new Country { Name = "Brasil" });
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

        private async Task CheckRegionAsync()
        {
            if (!_context.Regions.Any())
            {
                _context.Regions.Add(new Region { Name = "Antioquia", Country = _context.Countries.FirstOrDefault(x => x.Name == "Colombia") });
                _context.Regions.Add(new Region { Name = "Carabobo", Country = _context.Countries.FirstOrDefault(x => x.Name == "Venezuela") });
                _context.Regions.Add(new Region { Name = "Cundinamarca", Country = _context.Countries.FirstOrDefault(x => x.Name == "Colombia") });
                _context.Regions.Add(new Region { Name = "Alagoas", Country = _context.Countries.FirstOrDefault(x => x.Name == "Brasil") });
                _context.Regions.Add(new Region { Name = "Cusco", Country = _context.Countries.FirstOrDefault(x => x.Name == "Perú") });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCityAsync()
        {
            if (!_context.Cities.Any())
            {
                _context.Cities.Add(new City { Name = "Medellín", Region = _context.Regions.FirstOrDefault(x => x.Name == "Antioquia") });
                _context.Cities.Add(new City { Name = "Valencia", Region = _context.Regions.FirstOrDefault(x => x.Name == "Carabobo") });
                _context.Cities.Add(new City { Name = "Bogotá", Region = _context.Regions.FirstOrDefault(x => x.Name == "Cundinamarca") });
                _context.Cities.Add(new City { Name = "Penedo", Region = _context.Regions.FirstOrDefault(x => x.Name == "Alagoas") });
                _context.Cities.Add(new City { Name = "Paruro", Region = _context.Regions.FirstOrDefault(x => x.Name == "Cusco") });
                await _context.SaveChangesAsync();
            }
        }
    }
}
