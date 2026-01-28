using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers;

[Route("api/schedules")]
[ApiController]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    /// <summary>
    /// Constructor de la clase <see cref="ScheduleController"/>.
    /// </summary>
    /// <param name="scheduleService">Servicio para manejar la lógica de negocio de los eventos programados.</param>
    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    /// <summary>
    /// Obtiene el detalle de un evento programado por su identificador.
    /// </summary>
    /// <param name="id">Identificador del horario.</param>
    /// <returns>
    /// Una respuesta HTTP que contiene el detalle del evento programado y un código de estado.
    /// </returns>
    [HttpGet("detail/{id}")] // GET api/schedules/detail/{id}
    public async Task<IActionResult> GetScheduleById(int id)
    {
        var scheduleResponse = await _scheduleService.GetScheduleById(id);
        return StatusCode(scheduleResponse.StatusCode, scheduleResponse);
    }

    /// <summary>
    /// Obtiene una lista de eventos programados filtrados por el Id del evento.
    /// </summary>
    /// <param name="eventId">Id del evento por el cual filtrar.</param>
    /// <returns>
    /// Una respuesta HTTP que contiene una lista de eventos programados y un código de estado.
    /// </returns>
    [HttpGet("{eventId}")] // GET api/schedules/{eventId}
    public async Task<IActionResult> GetSchedulesByEventId(int eventId)
    {
        var schedulesResponse = await _scheduleService.GetSchedulesByEventId(eventId);
        return StatusCode(schedulesResponse.StatusCode, schedulesResponse);
    }
}