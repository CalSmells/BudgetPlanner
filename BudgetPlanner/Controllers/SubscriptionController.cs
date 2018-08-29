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
using Microsoft.AspNetCore.SignalR;
using BudgetPlanner.Persistence.Interfaces;

namespace BudgetPlanner.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public static ActionResult ReturnUrl { get; set; }

        public SubscriptionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        

        // GET: Subscription
        public async Task<IActionResult> Index()
        {
            var user = await _unitOfWork.Users.GetCurrentUserAsync(User);
            if (user != null)
            {
                var model = await _unitOfWork.Subscriptions
                    .WhereDescendingInclCompanyAsync(s => s.UserId == user.Id);
                TransactionController.ReturnUrl = RedirectToAction(nameof(Index));
                CompanyController.ReturnUrl = RedirectToAction(nameof(Index));
                ReturnUrl = RedirectToAction(nameof(Index));

                ViewBag.UserId = user.Id;
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
                var model = await _unitOfWork.Subscriptions
                    .WhereDescendingInclCompanyAsync(s => s.UserId == user.Id && s.SubscriptionName.Contains(search));
                TransactionController.ReturnUrl = RedirectToAction(nameof(Index));
                CompanyController.ReturnUrl = RedirectToAction(nameof(Index));
                ReturnUrl = RedirectToAction(nameof(Index));
                
                ViewBag.UserId = user.Id;
                ViewData["balance"] = user.Balance;
                return View("Index", model);
            }
            return RedirectToAction("Login", "Identity/Account");
        }


        public IActionResult Return()
        {
            return ReturnUrl;
        }


        public async Task<IActionResult> Update(int id)
        {
            var sub = await _unitOfWork.Subscriptions.FirstInclCompanyAsync(s => s.SubscriptionId== id);
            var user = await _unitOfWork.Users.GetCurrentUserAsync(User);

            var company = _unitOfWork.Companies.FirstInclTrans(c => c.CompanyId == sub.CompanyId);
            _unitOfWork.AddPayment(sub, user, company);
            _unitOfWork.Complete();

            return ReturnUrl;
        }


        // GET: Subscription/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _unitOfWork.Subscriptions
                .FirstInclTransCompanyAsync(m => m.SubscriptionId == id);
            if (subscription == null)
            {
                return NotFound();
            }
            decimal temp = (decimal)0.00;
            try
            {
                temp = (subscription.Progress / subscription.Target.Value)*100;
            }
            catch { }   
            ViewData["progress"] = temp;
            TransactionController.ReturnUrl = RedirectToAction(nameof(Details));
            ReturnUrl = RedirectToAction(nameof(Details));
            return View(subscription);
        }

        // GET: Subscription/Create
        [HttpGet("/Subscription/Create/deposit")]
        public IActionResult Create(bool deposit)
        {
            var model = CreateInit(deposit);
            return View(model);
        }

        public SubCreate CreateInit(bool deposit)
        {
            var companies = _unitOfWork.Companies.GetAllAsNoTracking();
            var output = new SubCreate
            {
                Companies = new SelectList(companies, "CompanyId", "CompanyName"),
                InOut = deposit

            };

            return output;
        }

        // POST: Subscription/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCreate sub)
        {
            if (ModelState.IsValid && sub.Amount != 0)
            {
            var user = await _unitOfWork.Users.GetCurrentUserAsync(User);
                var company = await _unitOfWork.Companies.GetAsync(sub.CompanyId);
                Subscription subscription = new Subscription { };
                subscription.Amount = (decimal)10.00;
                subscription = PostCreate(sub, subscription, user, company);
                
                _unitOfWork.Subscriptions.AddAsync(subscription);
                _unitOfWork.Complete();
                return ReturnUrl;
            }
            return RedirectToAction(nameof(Create));
        }

        public Subscription PostCreate(SubCreate sub, Subscription subscription, User user, Company company)
        {
            DateTime lastDue = sub.StartDate;

            decimal amount = sub.Amount;
            decimal target = (decimal)0.00;
            try { target = sub.Target.Value; }
            catch { }

            if ((sub.InOut && sub.Amount < 0) || (!sub.InOut && sub.Amount > 0))
            {
                amount = (-1)*amount;
                try { target = (-1)*target; }
                catch { }
            }

            subscription.SubscriptionName = sub.SubscriptionName;
            subscription.Company = company;
            subscription.CompanyId = company.CompanyId;
            subscription.Amount = amount;
            subscription.Target = target;
            subscription.Transactions = sub.Transactions;
            subscription.User = user;
            subscription.UserId = user.Id;
            subscription.InOut = sub.InOut;
            subscription.Interval = sub.Interval;
            subscription.NextDue = sub.StartDate;
            
            return subscription;
        }

        // GET: Subscription/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var sub = await _unitOfWork.Subscriptions.GetAsync(id.Value);

            var amount = sub.Amount;
            var target = sub.Target.Value;
            if (amount < 0)
            {
                target = target * (-1);
                amount = amount * (-1);
            }
            string interval = sub.Interval;
            SubCreate subscription = new SubCreate
            {
                Amount = amount,
                Target = target,
                SubscriptionName = sub.SubscriptionName,
                StartDate = sub.NextDue,
                CompanyId = sub.CompanyId,
                InOut = sub.InOut,
                Interval = interval
            };

            ViewData["id"] = id;
            if (subscription == null)
            {
                return NotFound();
            }
            return View(subscription);
        }

        // POST: Subscription/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCreate sub)
        {
            var subscription = await _unitOfWork.Subscriptions.GetAsync(id);
            if (ModelState.IsValid && sub.Amount != 0)
            {
                try
                {
            var user = await _unitOfWork.Users.GetCurrentUserAsync(User);
                    var company = await _unitOfWork.Companies.GetAsync(sub.CompanyId);
                    subscription = PostCreate(sub, subscription, user, company);
                    _unitOfWork.Subscriptions.Update(subscription);
                    _unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.SubscriptionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return ReturnUrl;
            }
            return View(sub);
        }

        // GET: Subscription/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _unitOfWork.Subscriptions
                .FirstAsync(m => m.SubscriptionId == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // POST: Subscription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subscription = await _unitOfWork.Subscriptions.GetAsync(id);
            var transactions = await _unitOfWork.Transactions.WhereAsync(t => t.SubId == subscription.SubscriptionId);
            foreach (var trans in transactions) 
            {
                trans.SubId = null;
                trans.Subscription = null;
            }
            _unitOfWork.Subscriptions.Remove(subscription);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(int id)
        {
            return _unitOfWork.Subscriptions.Any(e => e.SubscriptionId == id);
        }
    }
}
