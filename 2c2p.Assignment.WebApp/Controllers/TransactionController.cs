using _2c2p.Assignment.Bussiness;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace _2c2p.Assignment.WebApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionManager _transactionManager;

        public TransactionController(TransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload([FromForm] IFormFile file)
        {
            try
            {
                var result = _transactionManager.SaveTransactions(file);
                if (!result.IsSuccessful)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (Exception ex)
            {
                //TODO:add logging 
                return StatusCode(500);
            }
            return Ok();
        }

        public JsonResult GetAllTransactionsByCurrency(string currency)
        {
            var result = _transactionManager.GetAllTransGetAllTransactionsByCurrency(currency);
            return Json(result);
        }

        public JsonResult GetAllTransactionsByDateRange(DateTime begin,DateTime end)
        {
            var result = _transactionManager.GetAllTransactionsByDateRange(begin, end);
            return Json(result);
        }
        public JsonResult GetAllTransactionsByStatus(string status)
        {
            var result = _transactionManager.GetAllTransactionsByStatus(status);
            return Json(result);
        }
    }
}
