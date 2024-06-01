﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SalesOrderDetails2011_ConsultaPersonalizada1Controller : Controller
    {
        private readonly AdventureWorks2016Context _context;

        public SalesOrderDetails2011_ConsultaPersonalizada1Controller(AdventureWorks2016Context context)
        {
            _context = context;
        }

        // GET: SalesOrderDetails2011_ConsultaPersonalizada1
        public async Task<IActionResult> Index()
        {
            var adventureWorks2016Context = _context.Product2011_Consulta1
                .Where(s => s.Product.Color == "RED")
                .Select(s => new ProductViewModel
                {
                    ProductID = s.ProductID,
                    Name = s.Product.Name,
                    Color = s.Product.Color
                });

            return View(await adventureWorks2016Context.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrderDetail = await _context.SalesOrderDetail
                .Include(s => s.SalesOrder)
                .FirstOrDefaultAsync(m => m.SalesOrderID == id);
            if (salesOrderDetail == null)
            {
                return NotFound();
            }

            return View(salesOrderDetail);
        }

        // GET: SalesOrderDetails2011_ConsultaPersonalizada1/Create
        public IActionResult Create()
        {
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrderHeader, "SalesOrderID", "SalesOrderID");
            return View();
        }

        // POST: SalesOrderDetails2011_ConsultaPersonalizada1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalesOrderID,SalesOrderDetailID,CarrierTrackingNumber,OrderQty,ProductID,SpecialOfferID,UnitPrice,UnitPriceDiscount,LineTotal,rowguid,ModifiedDate")] SalesOrderDetail salesOrderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesOrderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrderHeader, "SalesOrderID", "SalesOrderID", salesOrderDetail.SalesOrderID);
            return View(salesOrderDetail);
        }

        // GET: SalesOrderDetails2011_ConsultaPersonalizada1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrderDetail = await _context.SalesOrderDetail.FindAsync(id);
            if (salesOrderDetail == null)
            {
                return NotFound();
            }
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrderHeader, "SalesOrderID", "SalesOrderID", salesOrderDetail.SalesOrderID);
            return View(salesOrderDetail);
        }

        // POST: SalesOrderDetails2011_ConsultaPersonalizada1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalesOrderID,SalesOrderDetailID,CarrierTrackingNumber,OrderQty,ProductID,SpecialOfferID,UnitPrice,UnitPriceDiscount,LineTotal,rowguid,ModifiedDate")] SalesOrderDetail salesOrderDetail)
        {
            if (id != salesOrderDetail.SalesOrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesOrderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderDetailExists(salesOrderDetail.SalesOrderID))
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
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrderHeader, "SalesOrderID", "SalesOrderID", salesOrderDetail.SalesOrderID);
            return View(salesOrderDetail);
        }

        // GET: SalesOrderDetails2011_ConsultaPersonalizada1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrderDetail = await _context.SalesOrderDetail
                .Include(s => s.SalesOrder)
                .FirstOrDefaultAsync(m => m.SalesOrderID == id);
            if (salesOrderDetail == null)
            {
                return NotFound();
            }

            return View(salesOrderDetail);
        }

        // POST: SalesOrderDetails2011_ConsultaPersonalizada1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesOrderDetail = await _context.SalesOrderDetail.FindAsync(id);
            if (salesOrderDetail != null)
            {
                _context.SalesOrderDetail.Remove(salesOrderDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesOrderDetailExists(int id)
        {
            return _context.SalesOrderDetail.Any(e => e.SalesOrderID == id);
        }
    }
}