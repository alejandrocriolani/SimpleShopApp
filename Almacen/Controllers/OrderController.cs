using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Almacen.Models.Data;
using Almacen.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Almacen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Almacen.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private UserManager<User> userManager;
        private IOrderRepository repository;
        private Cart cartService;

        public OrderController(UserManager<User> usrMgr, Cart cart, IOrderRepository repoService)
        {
            userManager = usrMgr;
            cartService = cart;
            repository = repoService;
        }

        public async Task<IActionResult> Checkout(string username)
        {
            OrdersViewModel ordersView;
            if (!string.IsNullOrEmpty(username))
            {
                ordersView = new OrdersViewModel();
                User user = await userManager.FindByNameAsync(username);
                if (user != null)
                {
                    ordersView.Cart = cartService;
                    ordersView.User = user;
                }
                return View(ordersView);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(User user)
        {
            User clientUser = await userManager.FindByIdAsync(user.Id);
            if(cartService.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "El carro de compras está vacío");
            }
            if(ModelState.IsValid && clientUser != null)
            {
                Order order = new Order();
                order.Lines = cartService.Lines.ToArray();
                order.Client = clientUser;
                order.OrderDateTime = DateTime.Now;
                order.Delivered = false;
                order.Rejected = false;
                repository.SaveOrder(order);
            }
            cartService.Clear();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles ="Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            List<Order> orders = repository.Orders.Include(u => u.Client).Include(e => e.Employee)
                .OrderBy(o => o.OrderDateTime).ToList();
            List<BasicOrderData> basicOrderDatas = new List<BasicOrderData>();


            foreach(Order order in orders)
            {
                if (string.IsNullOrEmpty(order.Client.Id))
                    break;
                BasicOrderData orderDetails = await GetBasicOrderData(order);
                basicOrderDatas.Add(orderDetails);
            }

            return View(basicOrderDatas);
        }

        public async Task<IActionResult> Details(string orderid)
        {
            Order order = repository.Orders.Include(u => u.Client)
                .Include(e => e.Employee).FirstOrDefault(o => o.Id == orderid);
            return View(await GetBasicOrderData(order));
        }

        private async Task<BasicOrderData> GetBasicOrderData(Order order)
        {
            if (order == null)
                return null;
            if (order.Client == null)
                return null;
            BasicOrderData data = new BasicOrderData();
            User client = await userManager.FindByIdAsync(order.Client.Id);
            User employee = null;

            data.Client = order.Client;

            if (order.Employee != null)
            {
                employee = await userManager.FindByIdAsync(order.Employee.Id);
                data.Employee = employee;
            }

            data.TotalPrice = order.Lines.Sum(p => p.Product.Price * p.Quantity);
            data.Order = order;
            
            return data;
        }

    }
}