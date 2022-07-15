using EFCore.Contacts.Application.Commands.Contact;
using EFCore.Contacts.Application.Commands.Contact.Responses;
using EFCore.Contacts.Application.Queries;
using EFCore.Contacts.Application.Queries.Models;
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
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);            
    }

    [HttpPut("personalinformation")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateContactPersonalInformationCommandResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePersonalInformations([FromBody]UpdateContactPersonalInformationCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);        
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContactDto))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var contact = await _contactQueryService.GetByIdAsync(id);
        return Ok(contact);        
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ContactDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _contactQueryService.GetAllasync());        
    }
}
