﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnBoardFlight_Backend.Data.IRepository;
using OnBoardFlight_Backend.Model;

namespace OnBoardFlight_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepository)
        {
            _productRepo = productRepository;
        }

        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepo.GetAllProducts();
        }
        [HttpGet]
        [Route("Product/{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            return _productRepo.GetProductById(id);
        }
    }
}