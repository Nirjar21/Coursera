using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET: api/users
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userRepository.GetAll();
        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userRepository.GetById(id);
        if (user == null)
            return NotFound(new { error = "User not found" });
        
        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public IActionResult Create([FromBody] User user)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(user.Name))
            return BadRequest(new { error = "Name is required" });
        
        if (string.IsNullOrWhiteSpace(user.Email))
            return BadRequest(new { error = "Email is required" });
        
        if (!user.Email.Contains("@"))
            return BadRequest(new { error = "Invalid email format" });

        var created = _userRepository.Add(user);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] User user)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(user.Name))
            return BadRequest(new { error = "Name is required" });
        
        if (string.IsNullOrWhiteSpace(user.Email))
            return BadRequest(new { error = "Email is required" });

        var updated = _userRepository.Update(id, user);
        if (updated == null)
            return NotFound(new { error = "User not found" });
        
        return Ok(updated);
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _userRepository.Delete(id);
        if (!deleted)
            return NotFound(new { error = "User not found" });
        
        return NoContent();
    }
}