using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosApi.DataModels;
using PosApi.DataModels.DataModels;
using PosApi.DataModels.Dtos;
using PosApi.Extensions;

namespace PosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly PosDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(PosDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET : api/Product/getall/product-list

        [Authorize]
        [HttpGet]
        [Route("getall/product-list")]
        public async Task<IActionResult> GetAllProductList()
        {
            List<ProductListDto> productList = new List<ProductListDto>();
            try
            {
                var query = from p in _context.Products
                            join pt in _context.ProductTypes on p.ProductTypesId equals pt.Id
                            where (p.IsDeleted == false)
                            select new ProductListDto()
                            {
                                ProductsId = p.Id,
                                ProductTypesId = pt.Id,
                                ProductTypeName = pt.ProductTypeName,
                                ProductName = p.ProductName,
                                ProductSalePrice = p.ProductSalePrice,
                                UnitName = p.UnitName,
                                ProductTypeImagePath = pt.ProductTypeImagePath,
                                ProductImagePath = p.ProductImagePath
                            };
                
                productList = await query.AsNoTracking().ToListAsync();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = ex.ToString() });
            }

            return Ok(productList);
        }

        // GET: api/Product/getall
        [Authorize]
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllProduct()
        {
            List<ProductDto> products = new List<ProductDto>();
            try
            {
                products = await _context.Products.Where(e => e.IsDeleted == false)
                .AsNoTracking().OrderBy(e => e.ProductCode).ThenBy(e => e.ProductName)
                .Select(e => Mapping.Map<Products, ProductDto>(e))
                .ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = ex.ToString() });
            }

            return Ok(products);
        }

        // GET: api/Product/getby/{id}
        [Authorize]
        [HttpGet]
        [Route("getby/{id}")]
        public async Task<IActionResult> GetAllProductById(string id)
        {
            ProductDto product = new ProductDto();
            try
            {
                product = await _context.Products.Where(e => e.Id == id && e.IsDeleted == false)
                .AsNoTracking().Select(e => Mapping.Map<Products, ProductDto>(e))
                .FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = ex.ToString() });
            }

            return Ok(product);
        }

        // POST: api/Product/create
        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateUpdateProductDto request)
        {
            try
            {
                if (!String.IsNullOrEmpty(request.Id))
                {
                    var product = await _context.Products
                        .Where(e => e.Id == request.Id && e.IsDeleted == false)
                        .AsNoTracking().FirstOrDefaultAsync();
                    if (product != null) throw new Exception("มีรายการสินค้านี้อยู่แล้ว");
                }

                if (String.IsNullOrEmpty(request.ProductTypesId))
                {
                    throw new Exception("กรุณาระบุหมายเลขประเภทสินค้า");
                }
                else
                {
                    var productTypes = await _context.ProductTypes
                        .Where(e => e.Id == request.ProductTypesId && e.IsDeleted == false)
                        .AsNoTracking().FirstOrDefaultAsync();
                    if (productTypes == null) throw new Exception("ไม่มีข้อมูลประเภทสินค้า โปรดเพิ่มประเภทสินค้าก่อน");
                }

                var map = Mapping.Map<CreateUpdateProductDto, Products>(request);
                map.Id = Guid.NewGuid().ToString();
                map.CreateBy = GetUserId();
                map.CreateDate = DateTime.Now;

                await _context.Products.AddAsync(map);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = ex.ToString() });
            }

            return Ok(true);
        }

        // PUT : api/Product/update
        [Authorize]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateProduct([FromForm] CreateUpdateProductDto request)
        {
            try
            {
                if (String.IsNullOrEmpty(request.Id))
                {
                    throw new Exception("ระบุข้อมูลไม่ถูกต้อง โปรดตรวจสอบอีกครั้ง");
                }
                else
                {
                    var product = await _context.Products
                        .Where(e => e.Id == request.Id && e.IsDeleted == false)
                        .AsNoTracking().FirstOrDefaultAsync();

                    if (product == null) throw new Exception("ไม่มีรายการสินค้านี้ในระบบ");

                    product.ProductTypesId = request.ProductTypesId;
                    product.ProductCode = request.ProductCode;
                    product.ProductName = request.ProductName;
                    product.ProductBarcode = request.ProductBarcode;
                    product.ProductSalePrice = request.ProductSalePrice;
                    product.UnitName = request.UnitName;
                    product.UpdateBy = GetUserId();
                    product.UpdateDate = DateTime.Now;

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = ex.ToString() });
            }

            return Ok(true);
        }

        // DELETE : api/Product/delete/{productsId}
        [Authorize]
        [HttpDelete]
        [Route("delete/{productsId}")]
        public async Task<IActionResult> DeleteProduct(string productsId)
        {
            try
            {
                var query = await _context.Products
                    .Where(e => e.Id == productsId && e.IsDeleted == false)
                    .AsNoTracking().FirstOrDefaultAsync();

                // _context.Products.Remove(query);
                // await _context.SaveChangesAsync();

                if (query != null)
                {
                    query.IsDeleted = true;
                    query.UpdateBy = GetUserId();
                    query.UpdateDate = DateTime.Now;

                    _context.Products.Update(query);
                    await _context.SaveChangesAsync();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = ex.ToString() });
            }

            return Ok(true);
        }

        private string GetUserId()
        {
            var userIdentity = User.Identity;
            ClaimsIdentity claimsIdentity = userIdentity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst("sid");
            var userId = claim?.Value ?? "system";
            return userId;
        }
    }
}
