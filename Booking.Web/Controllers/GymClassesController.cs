﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking.Core.Entities;
using Booking.Web.Data;
using Booking.Data.Data;
using Booking.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Booking.Web.Extensions;
using Booking.Web.Filters;
using Booking.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Booking.Web.Controllers
{
   // [Authorize(Policy ="Test")]
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;

        // private readonly GymClassRepository gymClassRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public GymClassesController(IUnitOfWork uow,ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {

            _context = context ?? throw new ArgumentNullException(nameof(context));
            // gymClassRepository = new GymClassRepository(context);
            this.uow = uow;

            this.userManager = userManager;
        }

        // GET: GymClasses
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<GymClass> model = await uow.GymClassRepository.GetAsync();

            return View(model);
        }

       

        //[Authorize]
        public async Task<IActionResult> BookingToggle(int? id)
        {
            if (id is null) return BadRequest();

            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = userManager.GetUserId(User);

            if (userId == null) return NotFound();
            ApplicationUserGymClass? attending = await uow.ApplicationUserGymClassRepository.FindAsync(userId, (int)id);

            if (attending == null)
            {
                var booking = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymClassId = (int)id
                };

               // _context.AppUserGymClass.Add(booking);
               uow.ApplicationUserGymClassRepository.Add(booking);
            }
            else
            {
               // _context.AppUserGymClass.Remove(attending);
               uow.ApplicationUserGymClassRepository.Remove(attending);
            }

            await uow.CompleteAsync();

            return RedirectToAction("Index");

        }


        // GET: GymClasses/Details/5
        [RequiredParameterRequiredModel("id")]
        public async Task<IActionResult> Details(int? id)
        {
            return View(await uow.GymClassRepository.GetAsync((int)id!));
        }

        // GET: GymClasses/Create
        public IActionResult Create()
        {
            return Request.IsAjax() ? PartialView("CreatePartial")  : View();
        }

        public IActionResult FetchForm()
        {
            return PartialView("CreatePartial");
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return Request.IsAjax() ? PartialView("GymClassPartial", gymClass) : RedirectToAction(nameof(Index));
            }

            if(Request.IsAjax())
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("CreatePartial", gymClass);
            }

            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GymClasses == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
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
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GymClasses == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GymClasses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GymClasses'  is null.");
            }
            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass != null)
            {
                _context.GymClasses.Remove(gymClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
          return (_context.GymClasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
