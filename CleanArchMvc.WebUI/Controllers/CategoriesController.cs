﻿using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CleanArchMvc.WebUI.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return View(categories);
        }
        [HttpGet()]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    
        [HttpGet()]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) return NotFound();

            var categoryVM = await _categoryService.GetByIdAsync(id);

            if(categoryVM == null) return NotFound();

            return View(categoryVM);    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateAsync(category);
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet()]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _categoryService.GetByIdAsync(id);

            if(category == null) return NotFound();

            return View(category);
        }

        [HttpPost(), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.RemoveAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null) return NotFound();

            var category = await _categoryService.GetByIdAsync(id);

            if(category == null) return NotFound();

            return View(category);
        }
    }
}
