using System;
using System.Collections.Generic;
using System.Linq;
using Almacen.Models.ViewModels;
using Almacen.Models;
using Almacen.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace Almacen.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public ProductController(IProductRepository rep)
        {
            repository = rep;
        }

        public IActionResult Index(string search, string sortOrder, int? page)
        {
            int productsPerPage = 10;
            ProductsViewModel productsViewModel = new ProductsViewModel();
            IQueryable<Product> products = repository.Products;

            if (page == null)
                page = 1;

            ViewData["NameSortParam"] = sortOrder == "name" ? "name_des" : "name";
            ViewData["QuantSortParam"] = sortOrder == "quant" ? "quant_des" : "quant";
            ViewData["BrandSortParam"] = sortOrder == "brand" ? "brand_des" : "brand";
            ViewData["CatSortParam"] = sortOrder == "cat" ? "cat_des" : "cat";
            ViewData["CostPriceSortParam"] = sortOrder == "costPrice" ? "costPrice_des" : "costPrice";
            ViewData["PriceSortParam"] = sortOrder == "price" ? "price_des" : "price";
            ViewData["CurrentFilter"] = search;


            if (!string.IsNullOrEmpty(search))
            {
                products =
                    products.Where(p => p.Name.Contains(search) || 
                    p.Brand.Contains(search) || 
                    p.Description.Contains(search) ||
                    p.Category.Contains(search));
            }

            if(!string.IsNullOrEmpty(sortOrder))
            {
                switch(sortOrder)
                {
                    case "name":
                        products = products.OrderBy(p => p.Name);
                        break;
                    case "name_des":
                        products = products.OrderByDescending(p => p.Name);
                        break;
                    case "quant":
                        products = products.OrderBy(p => p.Quantity);
                        break;
                    case "quant_des":
                        products = products.OrderByDescending(p => p.Quantity);
                        break;
                    case "brand":
                        products = products.OrderBy(p => p.Brand);
                        break;
                    case "brand_des":
                        products = products.OrderByDescending(p => p.Brand);
                        break;
                    case "cat":
                        products = products.OrderBy(p => p.Category);
                        break;
                    case "cat_des":
                        products = products.OrderByDescending(p => p.Category);
                        break;
                    case "costPrice":
                        products = products.OrderBy(p => p.CostPrice);
                        break;
                    case "costPrice_des":
                        products = products.OrderByDescending(p => p.CostPrice);
                        break;
                    case "price":
                        products = products.OrderBy(p => p.Price);
                        break;
                    case "price_des":
                        products = products.OrderByDescending(p => p.Price);
                        break;
                    default:
                        products = products.OrderBy(p => p.Id);
                        break;
                }   
            }

            productsViewModel.LastSortParam = sortOrder;
            productsViewModel.CurrentPage = (int)page;
            if ((products.Count() % productsPerPage) == 0)
                productsViewModel.MaxPage = (products.Count() / productsPerPage);
            else
                productsViewModel.MaxPage = (products.Count() / productsPerPage) + 1;
            productsViewModel.Products = products.Skip(productsPerPage * ((int)page - 1))
                                                 .Take(productsPerPage);

            return View(productsViewModel);
            //return View(products);
        }

        public ViewResult Edit(string id)
        {
            return View(repository.Products.FirstOrDefault(p => p.Id == id));
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if(ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = $"¡Operación realizada con éxito!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }
    }
}