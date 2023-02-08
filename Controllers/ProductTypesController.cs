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

namespace PosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly PosDbContext _context;
        private readonly ILogger<ProductTypesController> _logger;

        public ProductTypesController(PosDbContext context, ILogger<ProductTypesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ProductTypes/getall
        [Authorize]
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllProductType()
        {
            List<ProductTypeDto> products = new List<ProductTypeDto>();
            try
            {
                var product = await _context.ProductTypes.Where(e => e.IsDeleted == false)
                .AsNoTracking().Select(e => new ProductTypeDto()
                {
                    Id = e.Id,
                    ProductTypeName = e.ProductTypeName,
                    ProductTypeImagePath = e.ProductTypeImagePath
                }).ToListAsync();

                products = product;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = ex.ToString() });
            }

            return Ok(products);
        }

         // GET: api/ProductTypes/getall
        [Authorize]
        [HttpGet]
        [Route("getby/{id}")]
        public async Task<IActionResult> GetAllProductTypeById(string id)
        {
            ProductTypeDto product = new ProductTypeDto();
            try
            {
                product = await _context.ProductTypes.Where(e => e.Id == id && e.IsDeleted == false)
                .AsNoTracking().Select(e => new ProductTypeDto()
                {
                    Id = e.Id,
                    ProductTypeName = e.ProductTypeName,
                    ProductTypeImagePath = e.ProductTypeImagePath
                }).FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = ex.ToString() });
            }

            return Ok(product);
        }

        // POST: api/ProductTypes/create
        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProductType([FromForm] CreateUpdateProductTypeDto request)
        {
            try
            {
                var model = new ProductTypes()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductTypeName = request.ProductTypeName,
                    CreateBy = GetUserId(),
                    CreateDate = DateTime.Now
                };

                await _context.ProductTypes.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = ex.ToString() });
            }

            return Ok(true);
        }

        [Authorize]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateProductType([FromForm] CreateUpdateProductTypeDto request)
        {
            try
            {
                var query = await _context.ProductTypes
                    .Where(e => e.Id == request.Id)
                    .AsNoTracking().FirstOrDefaultAsync();
                if (query != null)
                {
                    query.ProductTypeName = request.ProductTypeName;
                    query.UpdateBy = GetUserId();
                    query.UpdateDate = DateTime.Now;

                    _context.ProductTypes.Update(query);
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

        [Authorize]
        [HttpDelete]
        [Route("delete/{productTypesId}")]
        public async Task<IActionResult> DeleteProductType(string productTypesId)
        {
            try
            {
                var query = await _context.ProductTypes
                    .Where(e => e.Id == productTypesId && e.IsDeleted == false)
                    .AsNoTracking().FirstOrDefaultAsync();

                if (query == null) 
                {
                    throw new Exception("Cannot delete");
                }

                // _context.ProductTypes.Remove(query);
                // await _context.SaveChangesAsync();

                if (query != null)
                {
                    query.IsDeleted = true;
                    query.UpdateBy = GetUserId();
                    query.UpdateDate = DateTime.Now;

                    _context.ProductTypes.Update(query);
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
