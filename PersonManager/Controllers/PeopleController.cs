using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PersonManager.Data;
using PersonManager.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersonManager.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration configuration;

        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;

        [Obsolete]
        public PeopleController(ApplicationDbContext context, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this.configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.Person.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create([Bind("Person,File")] PersonViewModel personViewModel)
        {
            if (ModelState.IsValid)
            {
                if (null != personViewModel.File)
                {
                    var relativePath = GetRelativePath(personViewModel.File.Image.FileName);
                    await CreateAsync(personViewModel.File.Image, GetAbsolutePath(relativePath));

                    personViewModel.Person.IdCardImgUrl = relativePath;
                }

                _context.Add(personViewModel.Person);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(personViewModel);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            var personViewModel = new PersonViewModel()
            {
                Person = person
            };
            return View(personViewModel);
        }

        // POST: People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Edit(int id, [Bind("Person,File")] PersonViewModel personViewModel)
        {
            if (id != personViewModel.Person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (null != personViewModel.File)
                    {
                        Delete(GetAbsolutePath(personViewModel.Person.IdCardImgUrl));

                        var relativePath = GetRelativePath(personViewModel.File.Image.FileName);
                        await CreateAsync(personViewModel.File.Image, GetAbsolutePath(relativePath));

                        personViewModel.Person.IdCardImgUrl = relativePath;
                    }

                    _context.Update(personViewModel.Person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(personViewModel.Person.PersonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(personViewModel);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.FindAsync(id);

            Delete(GetAbsolutePath(person.IdCardImgUrl));

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }

        private string GetRelativePath(string fileName) => string.Format("{0}/{1}", configuration.GetValue<string>("StoredFilesPath"), Path.GetRandomFileName() + Path.GetExtension(fileName));

        [Obsolete]
        private string GetAbsolutePath(string relativePath) => hostingEnvironment.WebRootPath + relativePath;

        private void Delete(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        private async Task CreateAsync(IFormFile formFile, string path)
        {
            using (var fileStream = System.IO.File.Create(path))
            {
                await formFile.CopyToAsync(fileStream);
            }
        }
    }
}