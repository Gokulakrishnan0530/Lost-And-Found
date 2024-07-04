using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Lost_And_Found.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lost_And_Found.Areas.Identity.Data;

namespace Lost_And_Found.Controllers
{
    [Authorize]
    public class ListController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly CoreProjectContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ListController(UserManager<AuthUser> userManager, CoreProjectContext context, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

    
        public async Task<IActionResult> Index(string searchString)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            var userItems = await _context.Main.Where(m => m.UserId == userId).ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                userItems = userItems.Where(n => n.Item_Name.Contains(searchString)
                || n.Additional_Note.Contains(searchString)).ToList();
            }

            return View(userItems);
        }


        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Main main, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                main.UserId = user?.Id;

                if (ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }
                    main.ImagePath = "/images/" + uniqueFileName;
                }

                _context.Add(main);
                _context.LostItem.Add(new LostItem
                {
                    ImagePath = main.ImagePath,
                    Owner_Name = main.Owner_Name,
                    UserId = main.UserId,
                    UserEmail = user.Email,
                    Item_Name = main.Item_Name,
                    Location = main.Location,
                    DateTime = main.DateTime,
                    Additional_Note = main.Additional_Note
                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(main);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LostItemList(string searchString)
        {
            var lostItems = await _context.LostItem.ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                lostItems = lostItems.Where(n => n.Item_Name.Contains(searchString)
                || n.Additional_Note.Contains(searchString)).ToList();
            }
            return View(lostItems);
        }

        [AllowAnonymous]
        public async Task<IActionResult> FindedList(string searchString)
        {
            var findedItems = await _context.Finded.ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                findedItems = findedItems.Where(n => n.Item_Name.Contains(searchString)
                || n.Additional_Note.Contains(searchString)).ToList();
            }
            return View(findedItems);
        }

        public async Task<IActionResult> Finded(int id)
        {
            var crNew = await _context.Main.FindAsync(id);
            if (crNew == null)
            {
                return NotFound();
            }

            var lostItem = await _context.LostItem.FirstOrDefaultAsync(li => li.Item_Name == crNew.Item_Name
                && li.Additional_Note == crNew.Additional_Note
                && li.Owner_Name == crNew.Owner_Name
                && li.UserId == crNew.UserId
                && li.ImagePath == crNew.ImagePath);

            if (lostItem == null)
            {
                return NotFound();
            }

            var findedCrNew = new Finded
            {
                ImagePath = crNew.ImagePath,
                Owner_Name = crNew.Owner_Name,
                Item_Name = crNew.Item_Name,
                Location = crNew.Location,
                DateTime = crNew.DateTime,
                Additional_Note = crNew.Additional_Note
            };

            _context.Main.Remove(crNew);
            _context.LostItem.Remove(lostItem);
            _context.Finded.Add(findedCrNew);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var main = await _context.Main.FindAsync(id);
            if (main == null || main.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }
            return View(main);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Main main, IFormFile ImageFile)
        {
            if (id != main.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    main.UserId = user?.Id;

                    if (ImageFile != null)
                    {
                       
                        if (!string.IsNullOrEmpty(main.ImagePath))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, main.ImagePath);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }
                        main.ImagePath = Path.Combine("images", uniqueFileName);
                    }

                    _context.Update(main);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainExists(main.Id))
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
            return View(main);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var main = await _context.Main.FirstOrDefaultAsync(m => m.Id == id);
            if (main == null || main.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            return View(main);
        }

     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var main = await _context.Main.FirstOrDefaultAsync(m => m.Id == id);
            if (main == null || main.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            return View(main);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var main = await _context.Main.FindAsync(id);
            if (main.UserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            _context.Main.Remove(main);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MainExists(int id)
        {
            return _context.Main.Any(e => e.Id == id);
        }
    }
}
