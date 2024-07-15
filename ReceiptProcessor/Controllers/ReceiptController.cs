using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReceiptProcessor.Models;
using ReceiptProcessor.Services.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReceiptProcessor.Controllers
{
    [Route("Receipts")]
    [ApiController]
    public class ReceiptProcessorController : ControllerBase
    {
        private IReceiptPointService _receiptPointService;
        private IReceiptService _receiptService;
        private IMapper _mapper;

        public ReceiptProcessorController(IReceiptService receiptService, IReceiptPointService receiptPointService, IMapper mapper)
        {
            _receiptService = receiptService;
            _receiptPointService = receiptPointService;
            _mapper = mapper;
        }

        // GET: Receipts
        [HttpGet]
        public ActionResult<List<Receipt>> GetAll()
        {

                var receipts = _receiptService.GetAll();
                return Ok(receipts);

        }

        // GET Receipts/{id}
        [HttpGet("{id}")]
        public ActionResult<int> Get(Guid id)
        {

                var receipt = _receiptPointService.Get(id);
                if (receipt == null)
                {
                    return NotFound($"Receipt with ID {id} not found.");
                }
                return Ok(receipt);

        }

        // GET Receipts/{id}/points
        [HttpGet("{id}/points")]
        public ActionResult<int> GetPoints(Guid id)
        {
                var receipt = _receiptPointService.Get(id);
                if (receipt == null)
                {
                    return NotFound($"Receipt with ID {id} not found.");
                }
                return Ok(new { receipt.Points });
        }

        // POST Receipts/process
        [HttpPost("process")]
        public ActionResult<object> Post([FromBody] ReceiptDTO receiptDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var receipt = _mapper.Map<Receipt>(receiptDTO);
                var points = _receiptService.CalculateReceiptPoints(receipt);
                _receiptService.Add(receipt);
                var receiptPoint = _receiptPointService.ProcessReceiptPoint(receipt.Id, points);

                return Ok(new { receiptPoint.Id });
            }
            catch (AutoMapperMappingException ex)
            {
                return BadRequest("Invalid receipt data provided.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        // DELETE Receipts/{ID}/delete
        [HttpDelete("{id}/delete")]
        public ActionResult Delete(Guid id)
        {
            _receiptService.Remove(id);

            return Ok();
        }
    }
}
