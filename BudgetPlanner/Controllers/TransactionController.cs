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

namespace BudgetPlanner.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public static ActionResult ReturnUrl { get; set; }

        public TransactionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        //public TransactionController()
        //{
        //    //solely for the purpose of unit testing, this will crash the program
        //}


        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var user = await _unitOfWork.Users.GetCurrentUserAsync(User);
            if (user != null)
            {

                var trans = await _unitOfWork.Transactions
                    .WhereDescDateInclCompanyUserAsync(t => t.UserId == user.Id);
                SubscriptionController.ReturnUrl = RedirectToAction(nameof(Index));
                CompanyController.ReturnUrl = RedirectToAction(nameof(Index));
                ReturnUrl = RedirectToAction(nameof(Index));

                ViewData["balance"] = user.Balance;
                return View(trans);
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
                var model = await _unitOfWork.Transactions
                    .WhereDescDateInclCompanyUserAsync(t => t.UserId == user.Id && t.TransactionName.Contains(search));


                SubscriptionController.ReturnUrl = RedirectToAction(nameof(Index));
                CompanyController.ReturnUrl = RedirectToAction(nameof(Index));
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

        // GET: Transaction/Details/5
        [HttpGet("/Transaction/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _unitOfWork.Transactions.FirstInclSubUserAsync(m => m.TransId == id);
            if (transaction == null)
            {
                return NotFound();
            }
            SubscriptionController.ReturnUrl = RedirectToAction(nameof(Details));
            ReturnUrl = RedirectToAction("Details","Transaction");
            return View(transaction);
        }

        // GET: Transaction/Create
        [HttpGet("/Transaction/Create/{subId}/{deposit}"), ActionName("Create")]
        public async Task<IActionResult> Create(int subId, bool deposit)
        {
            var model = await InitCreate(subId, deposit);

            return View(model);
        }

        public async Task<TransCreate> InitCreate(int subId, bool deposit)
        {
            var companies = await _unitOfWork.Companies.GetAllAsync();
            TransCreate output = new TransCreate
            {
                CompanyIds = new SelectList(companies, "CompanyId", "CompanyName"),
                InOut = deposit,
                SubId = subId
            };
            return output;
        }
        
        public Transaction PostCreate(TransCreate trans, User user, Company company)
        {
            decimal amount = trans.Amount;
            if ((trans.InOut && trans.Amount < 0) || (!trans.InOut && trans.Amount > 0))
            {
                amount = (-1) * amount;
            }

            var output = new Transaction
            {
                UserId = user.Id.ToString(),
                CompanyId = trans.CompanyId,
                Amount = amount,
                DateTime = trans.DateTime,
                Company = company,
                User = user,
                TransactionName = trans.TransName
            };

            output.User.Balance += output.Amount;
            try
            {
                var sub = _unitOfWork.Subscriptions.Get(trans.SubId);
                if (sub != null)
                {
                    output.SubId = trans.SubId;
                    output.Subscription = sub;
                }
            }
            catch
            {
            }

            return output;
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransCreate trans)
        {
            if (ModelState.IsValid)
            {
            var user = await _unitOfWork.Users.GetCurrentUserAsync(User);
                var company = await _unitOfWork.Companies.GetAsync(trans.CompanyId);
                var transaction = PostCreate(trans, user, company);
                _unitOfWork.Transactions.AddAsync(transaction);
                company.Transactions.Add(transaction);
                user.Transactions.Add(transaction);
                await _unitOfWork.CompleteAsync();
                return ReturnUrl;
            }
            var users = await _unitOfWork.Users.GetAllAsync();
            ViewData["CompanyId"] = new SelectList(users, "CompanyId", "CompanyName", trans.CompanyId);
            return View(trans);
        }

        // GET: Transaction/Edit/5
        [HttpGet("/Transaction/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var transaction = await _unitOfWork.Transactions.GetAsync(id.Value);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewBag.Amount = transaction.Amount;
            var users = await _unitOfWork.Users.GetAllAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "Id", transaction.UserId);
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        [HttpPost("/Transaction/Edit/{id}/{at}"), ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, decimal at, [Bind("DateTime,Amount,UserId,SubId,TransId,CompanyId,TransactionName")] Transaction transaction)
        {
            if (id != transaction.TransId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Subscription sub = new Subscription { };
                try
                { sub = await _unitOfWork.Subscriptions.GetAsync(transaction.SubId.Value); }
                catch { }
                var user = await _unitOfWork.Users.GetCurrentUserAsync(User);
                if (at != transaction.Amount)
                {
                    var difference = transaction.Amount - at;
                    if (sub != null)
                    {
                        sub.Progress += difference;
                    }
                    user.Balance += difference;
                }
                try
                {
                    _unitOfWork.Transactions.Update(transaction);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransId))
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
            var users = await _unitOfWork.Users.GetAllAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "Id", transaction.UserId);
            return View(transaction);
        }

        // GET: Transaction/Delete/5
        [HttpGet("/Transaction/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _unitOfWork.Transactions.FirstInclUserAsync(m => m.TransId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetAsync(id);
            var user = await _unitOfWork.Users.FirstAsync(u => u.Id == transaction.UserId);
            try
            {
                var sub = await _unitOfWork.Subscriptions.GetAsync(transaction.SubId.Value);
                sub.Progress -= transaction.Amount;
                user.Balance -= transaction.Amount;
            }
            catch
            {
            }
            _unitOfWork.Transactions.Remove(transaction);
            await _unitOfWork.CompleteAsync();
            return ReturnUrl;
        }

        private bool TransactionExists(int id)
        {
            return _unitOfWork.Transactions.Any(e => e.SubId == id);
        }
    }
}
