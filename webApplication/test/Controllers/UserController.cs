using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using webApplication.Contracts.user;
using webApplication.requests;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Get user
        /// </summary>
        /// <returns></returns>
        // Get api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAll());
        }
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <remarks>
        /// Enter id
        /// </remarks>
        /// <param name="id">User</param>
        /// <returns></returns>
        // Get api/<UsersController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetById(id);
            var response = result.Adapt<GetUserResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Get user by password and email
        /// </summary>
        /// <remarks>
        /// Enter id
        /// </remarks>
        /// <param name="request">User</param>
        /// <returns></returns>
        // POST api/<UsersController>
        [HttpPost("login")]
        public async Task<IActionResult> GetByNameAndPassword([FromBody] GetUserByPasswordAndEmailRequest request)
        {

            var result = await _userService.GetByNameAndPassword(request.password,request.email);
            var response = result.Adapt<GetUserResponse>();
            return Ok(response);
        }
        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "name" : "Мax",
        ///        "surname" : "Morgan",
        ///        "avatar" : "pathToImage/",
        ///        "patronymic" : "___" nullable,
        ///        "email" : "example@mail.ru",
        ///        "telephonnumber" : "+79042391023"      
        ///        "passwordhash" : "jhdsDHJD-12jda",
        ///        "role" : "Student/Teacher/Admin" one of them,
        ///        "isactive" : true/false filled in by itself, usually true,
        ///        "createdat" : date.now() filled in by itself,        
        ///     }
        ///
        /// </remarks>
        /// <param name="request">User</param>
        /// <returns></returns>

        // POST api/<UsersController>

        [HttpPost]
        public async Task<IActionResult> Add(CreateUserRequest request)
        {

            var userDto = request.Adapt<user>();
            await _userService.Create(userDto);
            return Ok();
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     PUT /Todo
        ///     {
        ///        "userid" : 4
        ///        "name" : "Мax",
        ///        "surname" : "Morgan",
        ///        "avatar" : "pathToImage/",
        ///        "patronymic" : "___" nullable,
        ///        "email" : "example@mail.ru",
        ///        "telephonnumber" : "+79042391023"      
        ///        "passwordhash" : "jhdsDHJD-12jda",
        ///        "role" : "Student/Teacher/Admin" one of them,
        ///        "isactive" : true/false filled in by itself, usually true,
        ///        "createdat" : date.now() filled in by itself,  
        ///     }
        ///
        /// </remarks>
        /// <param name="request">User</param>
        /// <returns></returns>

        // PUT api/<UsersController>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserRequest request)
        {
            var useDto = request.Adapt<user>();
            useDto.isactive = true;
            var data = await _userService.GetById(useDto.userid);
            if (data == null)
                return NotFound("User not found");
            useDto.createdat = data.createdat;
            await _userService.Update(useDto);
            return Ok();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <remarks>
        /// Enter user id
        /// </remarks>
        /// <param name="id">User</param>
        /// <returns></returns>
        // Delete api/<UsersController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return Ok();
        }
    }
}