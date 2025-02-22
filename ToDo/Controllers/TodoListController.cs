using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.DataAccess;
using ToDo.Models;

namespace ToDo.Controllers
{
    public class TodoListController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult TodoList()
        {
            var TodoLists = dbContext.TodoLists;
            return View(TodoLists.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TodoList());
        }

        [HttpPost]
        public ActionResult Create(TodoList TodoLists, IFormFile file, IFormFile pdfFile)
        {
            if (ModelState.IsValid)
            {
                // Handle Image Upload
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");

                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    var filePath = Path.Combine(directoryPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    TodoLists.Img = fileName;
                }

                // Handle PDF Upload
                if (pdfFile != null && pdfFile.Length > 0)
                {
                    var pdfName = Guid.NewGuid().ToString() + Path.GetExtension(pdfFile.FileName);
                    var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");

                    if (!Directory.Exists(pdfPath))
                        Directory.CreateDirectory(pdfPath);

                    var pdfFilePath = Path.Combine(pdfPath, pdfName);

                    using (var stream = System.IO.File.Create(pdfFilePath))
                    {
                        pdfFile.CopyTo(stream);
                    }

                    TodoLists.Pdf = "/uploads/" + pdfName; // Save relative path
                }

                dbContext.TodoLists.Add(TodoLists);
                dbContext.SaveChanges();
                return RedirectToAction("TodoList");
            }

            return View(TodoLists);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var Todolist = dbContext.TodoLists.FirstOrDefault(e => e.Id == Id);
            if (Todolist != null)
                return View(Todolist);

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(TodoList TodoLists, IFormFile file, IFormFile pdfFile)
        {
            var TodoList = dbContext.TodoLists.AsNoTracking().FirstOrDefault(e => e.Id == TodoLists.Id);

            if (ModelState.IsValid)
            {
                // Handle Image Upload
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");

                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    var filePath = Path.Combine(directoryPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    TodoLists.Img = fileName;
                }
                else
                {
                    TodoLists.Img = TodoList?.Img;
                }

                // Handle PDF Upload
                if (pdfFile != null && pdfFile.Length > 0)
                {
                    var pdfName = Guid.NewGuid().ToString() + Path.GetExtension(pdfFile.FileName);
                    var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");

                    if (!Directory.Exists(pdfPath))
                        Directory.CreateDirectory(pdfPath);

                    var pdfFilePath = Path.Combine(pdfPath, pdfName);

                    using (var stream = System.IO.File.Create(pdfFilePath))
                    {
                        pdfFile.CopyTo(stream);
                    }

                    TodoLists.Pdf = "/uploads/" + pdfName;
                }
                else
                {
                    TodoLists.Pdf = TodoList?.Pdf;
                }

                dbContext.TodoLists.Update(TodoLists);
                dbContext.SaveChanges();
                return RedirectToAction("TodoList");
            }

            return View(TodoLists);
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            var Todolist = dbContext.TodoLists.FirstOrDefault(e => e.Id == Id);
            if (Todolist != null)
            {
                // Delete Image
                if (Todolist.Img != null)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", Todolist.Img);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                if (Todolist.Pdf != null)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", Path.GetFileName(Todolist.Pdf));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                dbContext.TodoLists.Remove(Todolist);
                dbContext.SaveChanges();

                return RedirectToAction("TodoList");
            }
            return NotFound();
        }

    }
}




