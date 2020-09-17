using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSCRUDelicious.Models;
using Microsoft.EntityFrameworkCore;

//using Microsoft.AspNetCore.Http;
// HttpContext.Session.SetString("key","")/.GetString("key")/.GetInt32("key",)/.GetInt32("key")
// HttpContext.Session.Clear();
// TempData["var"] = "";  //persist across one redirect only


namespace CSCRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private dbCRUDeliciousContext db;
        public HomeController(dbCRUDeliciousContext dbContext)
        {
            db = dbContext;
        }


        public IActionResult Index()
        {
            // DbSet<Dish> dishes = db.Dishes;
            // List<Dish> dishes = db.Dishes.ToList();
            // List<Dish> dishes = db.Dishes.Reverse().ToList();
            List<Dish> dishes = db.Dishes.OrderByDescending(d => d.Name).ToList();

            return View(dishes);
        }


        public IActionResult New()
        {
            return View();
        }

        public IActionResult Create(Dish newDish)
        {
            if (ModelState.IsValid == false)
            {
                return View("New");
            }
            // ModalState.IsValid...
            // newDish.CreatedAt = DateTime.Now;        //works, but moved to Model's instantiation
            db.Dishes.Add(newDish);                     //DB Insert
            db.SaveChanges();
            return RedirectToAction("Details", newDish.DishId);  //"Details", new {id = newDish.DishId}
        }

        //defaultMVCRoute: /Home/Details/4
        //exploring "id" as route param
        public IActionResult Details(int id)
        {
            Dish selectedDish = db.Dishes.FirstOrDefault(d => d.DishId == id);
            if (selectedDish == null)
            {
                return RedirectToAction("Index");
            }
            return View(selectedDish);
        }

        //defaultMVCRoute: /Home/Edit/1
        //HttpRoute takes precedence: /Home/1/Edit
        //exploring "id" as route param
        //exploring HttpRoute - apply this route when avail, without this use default MVCRoute
        [HttpGet("/Home/{id}/Edit")]
        public IActionResult Edit(int id)
        {
            Dish selectedDish = db.Dishes.FirstOrDefault(d => d.DishId == id);
            if (selectedDish == null)
            {
                return RedirectToAction("Index");
            }
            return View(selectedDish);
        }


        // RESTful route: /Home/1/Update
        // exploring "id" as route param
        // [HttpPost("/Home/{dishId}/Update")]     //dishId  //id = "This page isn't working"
        public IActionResult Update(Dish updatedDish, int dishId)       
        {
            if (ModelState.IsValid == false)
            {
                return View("Edit", updatedDish);  //correct way - get the new updated model with errors displayed
             // return RedirectToAction("Edit", dishId);        //??? why wouldnt this work - breakpoint shows updatedDish.DishId has a valid id
                                                                // param needs anonymous obj new { paramId = xxx}
            }

            // updatedDish.DishId only works if asp-route- on <form> use asp-route-dishId, 
            // frmwk knows to match it using asp-route-dishId withOUT the (... , int dishId) input param
            //Dish dbDish = db.Dishes.FirstOrDefault(d => d.DishId == updatedDish.DishId);    
            Dish dbDish = db.Dishes.FirstOrDefault(d => d.DishId == dishId);
            if (dbDish == null)
            {
                return RedirectToAction("Index");
            }
            dbDish.Name = updatedDish.Name;
            dbDish.Chef = updatedDish.Chef;
            dbDish.Description = updatedDish.Description;
            dbDish.Calories = updatedDish.Calories;
            dbDish.Tastiness = updatedDish.Tastiness;
            dbDish.UpdatedAt = DateTime.Now;
            
            db.Dishes.Update(dbDish);           // NOT Update(updatedDish) - it would override CreatedAt timestamp as well.
            db.SaveChanges();
            return RedirectToAction("Details",new{id = dbDish.DishId});     //RedirectToAction MUST HAVE object route param format in dict format if included, hence new{ x = xxx}   
            // return RedirectToAction("/Home/Details/{dishId}", dishId);   //- Err: No route matches the supplied values
            // return View("Details",dbDish);               //works
            // return RedirectToAction("Index");            //works, not good enough
            // return View("test", updatedDish.DishId);     //works
        }

    //defaultMVCRoute: /Home/Delete/4  
        //exploring "id" as route param
        public IActionResult Delete(int id)
        {
            // not checking null here - not needed - Default would be null if not found, will send to Index anyway
            db.Dishes.Remove(db.Dishes.FirstOrDefault(d => d.DishId == id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
