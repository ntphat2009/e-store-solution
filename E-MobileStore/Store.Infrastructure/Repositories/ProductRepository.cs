using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Infrastructure.Data;
using Store.Infrastructure.DTOs;
using Store.Infrastructure.Repositories.Interfaces;
using Store.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Store.Common.Utility.ProductsUtility;
namespace Store.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteProduct(string productUrl)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductUrl == productUrl);

            if (product == null)
            {
                throw new Exception("Product not found");
            }
            else
            {
                product.IsDeleted = true;
                product.UpdatedDate = DateTime.Now;
                _context.Products.Update(product);
                _context.SaveChanges();
            }
        }
        public async Task<Product> GetProductByUrlAsync(string productUrl)
        {
            var product = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductAttributes)
                .ThenInclude(x => x.AttributeValue)
                .Include(x => x.Rates)
                .Include(x => x.Comments)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductUrl == productUrl);
            if (product != null)
            {
                product.ProductImages = product.ProductImages.Where(img => img.IsActive && !img.IsDeleted).OrderBy(x=>x.Position).ToList();
            }
            return product ?? new Product();
        }

        public async Task<string> AddOrUpdateProduct(ProductDTO product)
        {
            try
            {
                if (product.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    bool nameExists = await _context.Products
                               .AnyAsync(p => p.Name == product.Name);
                    if (nameExists)
                    {
                        throw new Exception("This Product Name is already exists.");
                    }
                    var newProduct = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = product.Name,
                        ProductUrl = ToUrl(product.Name),
                        Description = product.Description,
                        ShortDesc = product.ShortDesc,
                        CategoryId = product.CategoryId,
                        IsActive = product.IsActive,
                        IsDeleted = false,
                        Price = product.Price,
                        PriceSale = product.PriceSale,
                        Quantity = product.Quantity,
                        CreatedBy = product.CreatedBy,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = product.UpdatedBy,
                        UpdatedDate = DateTime.Now,
                    };
                    _context.Products.Add(newProduct);
                    await _context.SaveChangesAsync();
                    return newProduct.Id.ToString();
                }
                else
                {
                    var existingProduct = await _context.Products
                                   .Include(p => p.ProductImages)
                                   .FirstOrDefaultAsync(p => p.Id.ToString() == product.Id);
                    if (existingProduct == null)
                    {
                        throw new Exception($"Sản phẩm {product.Name} không được tìm thấy");
                    }
                    else
                    {
                        existingProduct.Name = product.Name;
                        existingProduct.ProductUrl = ToUrl(product.Name);
                        existingProduct.Description = product.Description;
                        existingProduct.Price = product.Price;
                        existingProduct.PriceSale = product.PriceSale;
                        existingProduct.Quantity = product.Quantity;
                        existingProduct.ShortDesc = product.ShortDesc;
                        existingProduct.CategoryId = product.CategoryId;
                        existingProduct.CreatedBy = existingProduct.CreatedBy;
                        existingProduct.CreatedDate = existingProduct.CreatedDate;
                        existingProduct.UpdatedBy = product.UpdatedBy;
                        existingProduct.UpdatedDate = DateTime.Now;
                        existingProduct.IsActive = product.IsActive;
                        existingProduct.IsDeleted = product.IsDeleted;
                        _context.Products.Update(existingProduct);
                        _context.SaveChanges();
                    }
                    return product.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<IEnumerable<FlashSaleProduct>> GetSaleProducts(int flashSaleId)
        {
            bool isFlashSaleExist = _context.FlashSales.Any(p => p.Id == flashSaleId &&
                    !p.IsDeleted &&
                    p.IsActive &&
                    p.DateOpen <= DateTime.Now &&
                    p.DateClose >= DateTime.Now);

            if (!isFlashSaleExist)
            {
                throw new Exception("The FlashSale was not found");
            }
            var flashSaleProductsInclude = await _context.FlashSaleProducts
                   .Include(p => p.Product)
                   .ThenInclude(p => p.Category)
                   .Include(p => p.Product)
                   .ThenInclude(p => p.ProductImages)
                   .Include(p => p.FlashSale)
                   .AsNoTracking()
                   .Where(p => p.FlashSaleId == flashSaleId &&
                               !p.Product.IsDeleted &&
                               p.Product.IsActive &&
                               !p.IsDeleted &&
                               p.IsActive)
                   .ToListAsync();
            if (flashSaleProductsInclude.Count == 0)
            {
                throw new Exception("There are no products");
            }
            //var productlist = flashSaleProductsInclude.Select(fsp => fsp.Product).ToList();
            return flashSaleProductsInclude;
        }
        public async Task<IEnumerable<Product>> GetProductListByCateUrlAsync(string cateUrl, int page, int pageSize, string? sortBy)
        {
            IQueryable<Product> query = _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.ProductImages.Where(p => !p.IsDeleted && p.IsActive))
                .Include(x => x.ProductAttributes)
                .Where(x => x.Category.CategoryUrl == cateUrl && x.IsActive && !x.IsDeleted);
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "date_desc":
                        query = query.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "date_asc":
                        query = query.OrderBy(x => x.CreatedDate);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(x => x.Name);
                        break;
                    case "name_asc":
                        query = query.OrderBy(x => x.Name);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(x => x.Price);
                        break;
                    case "price_asc":
                        query = query.OrderBy(x => x.Price);
                        break;
                }
            }
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return products ?? new List<Product>();
        }
        public async Task<IEnumerable<Product>> GetProductListAsync(int page, int pageSize, string? sortBy)
        {
            IQueryable<Product> query = _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductAttributes);
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "date_desc":
                        query = query.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "date_asc":
                        query = query.OrderBy(x => x.CreatedDate);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(x => x.Name);
                        break;
                    case "name_asc":
                        query = query.OrderBy(x => x.Name);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(x => x.Price);
                        break;
                    case "price_asc":
                        query = query.OrderBy(x => x.Price);
                        break;
                }
            }
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return products ?? new List<Product>();
        }
        public async Task<IEnumerable<Product>> GetProductSearchAsync(string search, int page, int pageSize)
        {
            IQueryable<Product> query = _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.ProductImages);
            var product = await query
                .Where(x => x.IsActive && !x.IsDeleted && (x.Name.ToLower().Contains(search.ToLower()) || x.ShortDesc.ToLower().Contains(search.ToLower()) || x.Category.Name.ToLower().Contains(search.ToLower())))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Name)
                .ToListAsync();
            return product ?? new List<Product>();
        }
        public async Task<int> TotalProductByCateAsync(string cateUrl)
        {
            int totalProduct = await _context.Products.Include(x => x.Category).Where(x => x.Category.CategoryUrl == cateUrl && !x.IsDeleted && x.IsActive).CountAsync();
            return totalProduct;
        }
        public async Task<int> TotalProductAsync()
        {
            int totalProduct = await _context.Products.CountAsync();
            return totalProduct;
        }
    }
}