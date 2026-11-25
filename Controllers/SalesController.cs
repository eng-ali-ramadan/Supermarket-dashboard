using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketDashboard.Data;
using SupermarketDashboard.Models;

namespace SupermarketDashboard.Controllers;

public class SalesController : Controller
{
    private readonly SupermarketContext _context;

    public SalesController(SupermarketContext context)
    {
        _context = context;
    }

    // GET: /Sales  (بيحوّل لصفحة التسجيل)
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Create));
    }

    // بيجيب المنتجات وكل البيعات عشان تظهر في نفس الصفحة
    private async Task LoadPageDataAsync()
{
    ViewBag.Products = await _context.Products.OrderBy(p => p.Name).ToListAsync();
    ViewBag.Expenses = await _context.Expenses
        .OrderByDescending(e => e.Date)
        .ToListAsync();
    ViewBag.Sales = await _context.Sales
        .Include(s => s.Product)
        .OrderByDescending(s => s.SoldAt)
        .ToListAsync();
}

    // GET: /Sales/Create
    public async Task<IActionResult> Create()
    {
        await LoadPageDataAsync();
        return View(new Sale { Quantity = 1 });
    }

    // POST: /Sales/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Sale sale)
    {
        var product = await _context.Products.FindAsync(sale.ProductId);
        if (product == null)
            ModelState.AddModelError(nameof(sale.ProductId), "اختار منتج");

        if (sale.UnitPrice <= 0)
            ModelState.AddModelError(nameof(sale.UnitPrice), "السعر لازم يكون أكبر من صفر");

        if (sale.Quantity <= 0)
            ModelState.AddModelError(nameof(sale.Quantity), "الكمية لازم تكون 1 أو أكتر");

        if (!ModelState.IsValid)
        {
            await LoadPageDataAsync();
            return View(sale);
        }

        sale.SoldAt = DateTime.Now;
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Create));
    }

    // GET: /Sales/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var sale = await _context.Sales.FindAsync(id);
        if (sale == null) return NotFound();

        ViewBag.Products = await _context.Products.OrderBy(p => p.Name).ToListAsync();
        return View(sale);
    }

    // POST: /Sales/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Sale sale)
    {
        if (id != sale.Id) return BadRequest();

        var product = await _context.Products.FindAsync(sale.ProductId);
        if (product == null)
            ModelState.AddModelError(nameof(sale.ProductId), "اختار منتج");

        if (sale.UnitPrice <= 0)
            ModelState.AddModelError(nameof(sale.UnitPrice), "السعر لازم يكون أكبر من صفر");

        if (sale.Quantity <= 0)
            ModelState.AddModelError(nameof(sale.Quantity), "الكمية لازم تكون 1 أو أكتر");

        if (!ModelState.IsValid)
        {
            ViewBag.Products = await _context.Products.OrderBy(p => p.Name).ToListAsync();
            return View(sale);
        }

        // نحافظ على تاريخ البيع الأصلي
        var original = await _context.Sales.AsNoTracking().FirstAsync(s => s.Id == id);
        sale.SoldAt = original.SoldAt;

        _context.Update(sale);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Create));
    }

    // POST: /Sales/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var sale = await _context.Sales.FindAsync(id);
        if (sale != null)
        {
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Create));
    }
}