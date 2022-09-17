using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public EmployeeController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]

        public async Task<ActionResult<List<Employee>>>GetAllEmployees()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Employee> Emp = await SelectAllEmployes(connection);
            return Ok(Emp);
        }

        [HttpGet("{EmpId}")]

        public async Task<ActionResult<Employee>> GetEmp(int EmpId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var Empl = await connection.QueryFirstAsync<Employee>("select * from Employees where empid = @EmpID",
                new {EmpID = EmpId});
            return Ok(Empl);
        }
        [HttpPost]

        public async Task<ActionResult<List<Employee>>> CreateEmployee(Employee emp)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into employees (empname, email, age) values (@EmpName, @Email, @Age)", emp);
            return Ok(await SelectAllEmployes(connection));
        }

        [HttpPut]

        public async Task<ActionResult<List<Employee>>> UpdateEmployee(Employee emp)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update employees set empname = @EmpName, email = @Email, age = @Age where empid = @EmpID", emp);
            return Ok(await SelectAllEmployes(connection));
        }

        [HttpDelete("{EmpId}")]

        public async Task<ActionResult<List<Employee>>> eleteEmployee(int EmpId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from employees where empid = @EmpID", new { EmpID = EmpId});
            return Ok(await SelectAllEmployes(connection));
        }



        private static async Task<IEnumerable<Employee>> SelectAllEmployes(SqlConnection connection)
        {
            return await connection.QueryAsync<Employee>("select * from Employees");
        }

    }
}
