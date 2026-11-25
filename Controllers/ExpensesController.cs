using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketDashboard.Data;
using SupermarketDashboard.Models;

namespace SupermarketDashboard.Controllers;

public class ExpensesController : Controller
{
    private readonly SupermarketContext _context;

    public ExpensesController(SupermarketContext context)
    {
        _context = context;
    }

    // GET: /Expenses  (صفحة المصروفات)
    public async Task<IActionResult> Index()
    {
        var expenses = await _context.Expenses
            .OrderByDescending(e => e.Date)
            .ToListAsync();
        return View(expenses);
    }

    // POST: /Expenses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Expense expense, bool fromExpensesPage = false)
    {
        if (string.IsNullOrWhiteSpace(expense.Title))
        {
            TempData["Error"] = "اكتب اسم المصروف";
        }
        else if (expense.Amount <= 0)
        {
            TempData["Error"] = "المبلغ لازم يكون أكبر من صفر";
        }
        else
        {
            if (expense.Date == default)
                expense.Date = DateTime.Now;

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        if (fromExpensesPage)
            return RedirectToAction(nameof(Index));

        return RedirectToAction("Create", "Sales");
    }

    // POST: /Expenses/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense != null)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}