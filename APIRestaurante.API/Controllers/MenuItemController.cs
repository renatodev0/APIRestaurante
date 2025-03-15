using APIRestaurante.Domain.Interfaces;
using APIRestaurante.Domain.Entities;
using APIRestaurante.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRestaurante.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var menuItems = await _menuItemService.GetAllAsync();
            return Ok(new { items = menuItems });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var menuItem = await _menuItemService.GetByIdAsync(id);
            if (menuItem == null)
                return NotFound();

            return Ok(menuItem);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] MenuItemCreateDto menuItemDto)
        {
            if (menuItemDto == null)
                return BadRequest();

            var menuItem = await _menuItemService.CreateAsync(menuItemDto);
            return CreatedAtAction(nameof(GetById), new { id = menuItem.Id }, menuItem);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePartial(int id, [FromBody] MenuItemUpdateDto menuItem)
        {
            if (menuItem == null)
                return BadRequest();

            var updatedMenuItem = await _menuItemService.UpdatePartialAsync(id, menuItem);
            if (updatedMenuItem == null)
                return NotFound();

            return NoContent();
        }

        // DELETE: /MenuItem/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _menuItemService.GetByIdAsync(id);
            if (menuItem == null)
                return NotFound();

            await _menuItemService.DeleteAsync(menuItem);
            return NoContent();
        }
    }
}
