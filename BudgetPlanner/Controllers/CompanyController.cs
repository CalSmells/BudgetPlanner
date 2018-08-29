using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetPlanner.Data;
using BudgetPlanner.Models;
using Microsoft.AspNetCore.Identity;
using BudgetPlanner.Persistence.Interfaces;
using Autofac;
using BudgetPlanner.Persistence;
using BudgetPlanner.Persistence.Repositories;

namespace BudgetPlanner.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public static ActionResult ReturnUrl { get; set; }
        private static IContainer container { get; set; }

        public CompanyController(IUnitOfWork unitOfWork)
        {
            //var builder = new ContainerBuilder();
            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            //builder.Register(c => new UnitOfWork()).As<IUnitOfWork>();
            //container = builder.Build();
            //using (var scope = container.BeginLifetimeScope())
            //    var blah = scope.Resolve<IUnitOfWork>();
            _unitOfWork = unitOfWork;
        }

        // GET: Company
        public async Task<IActionResult> Index()
        {
            var user = await _unitOfWork.Users.GetCurrentUserAsync(User);
            if (user != null)
            {
                var model = await _unitOfWork.Companies.GetAllAsync();
                TransactionController.ReturnUrl = RedirectToAction(nameof(Index));
                SubscriptionController.ReturnUrl = RedirectToAction(nameof(Index));
                ReturnUrl = RedirectToAction(nameof(Index));
                ViewData["balance"] = user.Balance;
                return View(model);
            } 
            return RedirectToAction("Login", "Identity/Account");
        }


        public async Task<IActionResult> IndexSearch(string search)
        {
            var user = await _unitOfWork.Users.GetCurrentUserAsync(User);
            if (search == null)
            {
                search = "";
            }
            if (user != null)
            {
                var model = await _unitOfWork.Companies.FirstAsync(c => c.CompanyName.Contains(search));
                TransactionController.ReturnUrl = RedirectToAction(nameof(Index));
                SubscriptionController.ReturnUrl = RedirectToAction(nameof(Index));
                ReturnUrl = RedirectToAction(nameof(Index));

                ViewData["balance"] = user.Balance;
                return View("Index", model);
            }
            return RedirectToAction("Login", "Identity/Account");
        }


        public IActionResult Return()
        {
            return ReturnUrl;
        }

        // GET: Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.FirstInclTransSubAsync(c => c.CompanyId == id.Value);
            if (company == null)
            {
                return NotFound();
            }
            TransactionController.ReturnUrl = RedirectToAction(nameof(Index));
            SubscriptionController.ReturnUrl = RedirectToAction(nameof(Index));
            ReturnUrl = RedirectToAction(nameof(Index));
            return View(company);
        }

        // GET: Company/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyName")] Company company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Companies.AddAsync(company);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Company/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.GetAsync(id.Value);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,CompanyName")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Companies.AddAsync(company);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
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
            return View(company);
        }

        // GET: Company/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.GetAsync(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _unitOfWork.Companies.GetAsync(id);
            _unitOfWork.Companies.Remove(company);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _unitOfWork.Companies.Any(e => e.CompanyId == id);
        }
    }
}
