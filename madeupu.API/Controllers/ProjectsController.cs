using madeupu.API.Data;
using madeupu.API.Data.Entities;
using madeupu.API.Helpers;
using madeupu.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Controllers
{

    public class ProjectsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IUserHelper _userHelper;

        public ProjectsController(DataContext context, ICombosHelper combosHelper, IConverterHelper converterHelper, IBlobHelper blobHelper, IUserHelper userHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            _userHelper = userHelper;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects
                .Include(x => x.ProjectCategory)
                .Include(x => x.City)
                .ThenInclude(x => x.Region)
                .ThenInclude(x => x.Country)
                .Include(x => x.City)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .Include(x => x.Participations)
                .ThenInclude(x => x.ParticipationType)
                .Include(x => x.Participations)
                .ThenInclude(x => x.User)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ProjectViewModel model = new ProjectViewModel
            {
                ProjectCategories = _combosHelper.GetComboProyectCategories(),
                Cities = _combosHelper.GetComboCities()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "projects");
                    }

                    Project project = await _converterHelper.ToProjectAsync(model, imageId, true);

                    User user = await _userHelper.GetUserAsync(User.Identity.Name);
                    ParticipationType participationType = await _context.ParticipationTypes
                        .FirstOrDefaultAsync(x => x.Description == "Creador");

                    if ((project == null) ^ (user == null) ^ (participationType == null))
                    {
                        return NotFound();
                    }

                    Participation participation = new Participation
                    {
                        User = user,
                        ParticipationType = participationType,
                        Project = project,
                        Message = "Creacion del proyecto"
                    };


                    if (project.Participations == null)
                    {
                        project.Participations = new List<Participation>();
                    }

                    //if (user.Participations == null)
                    //{
                    //    user.Participations = new List<Participation>();
                    //}

                    //user.Participations.Add(participation);
                    project.Participations.Add(participation);

                    _context.Participations.Add(participation);
                    _context.Projects.Add(project);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(user.UserType != Enums.UserType.User? nameof(Index) : nameof(MyProjects));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un proyecto con este nombre, intenta con uno nuevo.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exeption)
                {
                    ModelState.AddModelError(string.Empty, exeption.InnerException.Message);
                }
            }

            model.ProjectCategories = _combosHelper.GetComboProyectCategories();
            model.Cities = _combosHelper.GetComboCities();

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project project = await _context.Projects
                .Include(x => x.City)
                .Include(x => x.ProjectCategory)
                .FirstOrDefaultAsync(x => x.Id == id);


            if (project == null)
            {
                return NotFound();
            }

            ProjectViewModel model = _converterHelper.ToProjectViewModel(project);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectViewModel projectViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    Guid imageId = projectViewModel.ImageId;
                    if (projectViewModel.ImageFile != null)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(projectViewModel.ImageFile, "projects");
                    }
                    Project project = await _converterHelper.ToProjectAsync(projectViewModel, imageId, false);
                    _context.Projects.Update(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = projectViewModel.CityId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe esta region.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exeption)
                {
                    ModelState.AddModelError(string.Empty, exeption.InnerException.Message);
                }
            }

            projectViewModel.ProjectCategories = _combosHelper.GetComboProyectCategories();
            projectViewModel.Cities = _combosHelper.GetComboCities();
            return View(projectViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> SingleProject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project project = await _context.Projects
                .Include(x => x.ProjectCategory)
                .Include(x => x.City)
                .ThenInclude(x => x.Region)
                .ThenInclude(x => x.Country)
                .Include(x=> x.City)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x=>x.Comments)
                .ThenInclude(x => x.User)
                .Include(x=>x.Ratings)
                .ThenInclude(x => x.User)
                .Include(x=>x.Participations)
                .ThenInclude(x=>x.ParticipationType)
                .Include(x=>x.Participations)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (project == null)
            {
                return NotFound();
            }


            return View(project);
        }

        public async Task<IActionResult> AddComment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            CommentViewModel model = new CommentViewModel
            {
                ProjectId = project.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Project project = await _context.Projects
                    .Include(x => x.Comments)
                    .ThenInclude(x=> x.User)
                    .FirstOrDefaultAsync(x => x.Id == model.ProjectId);
                if (project == null)
                {
                    return NotFound();
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                if (user == null)
                {
                    return NotFound();
                }

                Comment comment = new Comment
                {
                    Project = project,
                    Date = DateTime.UtcNow,
                    Message = model.Message,
                    User = user
                };

                if (project.Comments == null)
                {
                    project.Comments = new List<Comment>();
                }

                //if (user.Comments == null)
                //{
                //    user.Comments = new List<Comment>();
                //}

                project.Comments.Add(comment);
                //user.Comments.Add(comment);
                _context.Comments.Add(comment);
                _context.Projects.Update(project);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SingleProject), new { id = project.Id });
            }

            return View(model);
        }


        public async Task<IActionResult> AddRate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            RatingViewModel model = new RatingViewModel
            {
                ProjectId = project.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRate(RatingViewModel model)
        {
            if (ModelState.IsValid)
            {
                Project project = await _context.Projects
                    .Include(x => x.Ratings)
                    .ThenInclude(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == model.ProjectId);
                if (project == null)
                {
                    return NotFound();
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);
                Rating rating = new Rating
                {
                    Project = project,
                    Date = DateTime.UtcNow,
                    Rate = model.Rate,
                    User = user
                };

                if (project.Ratings == null)
                {
                    project.Ratings = new List<Rating>();
                }

                //if (user.Ratings == null)
                //{
                //    user.Ratings = new List<Rating>();
                //}

                project.Ratings.Add(rating);
                //user.Ratings.Add(rating);
                _context.Ratings.Add(rating);
                _context.Projects.Update(project);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SingleProject), new { id = project.Id });
            }

            return View(model);
        }

        public async Task<IActionResult> MyProjects()
        {
            return View(await _context.Participations
                .Include(x=>x.ParticipationType)
                .Include(x=>x.Project)
                .ThenInclude(x=>x.City)
                .ThenInclude(x=>x.Region)
                .ThenInclude(x => x.Country)
                .Include(x=>x.User)
                .Where(x=>x.User.Email == User.Identity.Name)
                .ToListAsync());
        }

    }
}
