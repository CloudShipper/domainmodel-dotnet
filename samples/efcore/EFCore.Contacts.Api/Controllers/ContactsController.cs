using EFCore.Contacts.Application.Commands.Contact;
using EFCore.Contacts.Application.Commands.Contact.Responses;
using EFCore.Contacts.Application.Queries;
using EFCore.Contacts.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EFCore.Contacts.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IContactQueryService _contactQueryService;

    public ContactsController(IMediator mediator, IContactQueryService contactQueryService)
    {
        _mediator = mediator;
        _contactQueryService = contactQueryService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateContactCommandResponse))]
    public async Task<IActionResult> CreateContac([FromBody]CreateContactCommand command)
    {
        try
        {
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (Exception)
        {
            // Todo, error handling
            return BadRequest();
        }       
    }

    [HttpPut("personalinformation")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateContactPersonalInformationCommandResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePersonalInformations([FromBody]UpdateContactPersonalInformationCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception)
        {
            // Todo, error handling
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contact))]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var contact = await _contactQueryService.GetByIdAsync(id);
            return Ok(contact);
        }        
        catch (Exception)
        {
            // Todo, error handling
            return NotFound();
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contact>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _contactQueryService.GetAllasync());
        }
        catch (Exception)
        {
            // Todo, error handling
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
