using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alameen_CustomerAndDonglesMangment.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alameen_CustomerAndDonglesMangment.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public APIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Home")]
        public JsonResult Home(string text)
        {
            var data = _context.Dongles.Include(c => c.Customer)
                .Where(i=>i.SerialNum.Contains(text) || i.Customer.Name.Contains(text) || i.Customer.CommercialName.Contains(text))
                .Select(o => new {
                    DongleSerialNumber =  o.SerialNum,
                    CustomerName = o.Customer.Name,
                    CustomerCommercialName = o.Customer.CommercialName,
                    ExpirationDate = o.ExpirationDate.HasValue ? o.ExpirationDate.Value.ToString("yyyy/MM/dd") : "لايوجد تاريخ مضاف"
                });
            return new JsonResult(data);
        }
    }
}