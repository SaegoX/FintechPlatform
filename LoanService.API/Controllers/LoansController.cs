using LoanService.Application.UseCases.LoanApplications;
using LoanService.Application.UseCases.LoanApplications.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LoanService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoansController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateLoanApplicationCommand command, CancellationToken cancellationToken)
        {
            var loanId = await _mediator.Send(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, loanId);
        }

        [HttpPost("{id:guid}/approve")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ApproveLoanApplicationCommand(id), cancellationToken);
            return NoContent();
        }

        [HttpGet("{id:guid}", Name = nameof(GetById))]
        [ProducesResponseType(typeof(LoanApplicationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetLoanApplicationByIdQuery(id);
                var result = await _mediator.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}