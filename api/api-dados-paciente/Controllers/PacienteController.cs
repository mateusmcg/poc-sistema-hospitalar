using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Amazon.DynamoDBv2.DocumentModel;

namespace api_dados_paciente.Controllers
{
    [Route("api/paciente")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        public static IConfiguration Configuration;
        public static BasicAWSCredentials AwsCredenciais;
        public static AmazonDynamoDBClient client;
        public static DynamoDBContext context;

        public PacienteController(IConfiguration configuration)
        {
            Configuration = configuration;
            AwsCredenciais = new BasicAWSCredentials(Configuration["AWS:DynamoDb:accessKey"], Configuration["AWS:DynamoDb:secretKey"]);
            client = new AmazonDynamoDBClient(AwsCredenciais, RegionEndpoint.SAEast1);
            context  = new DynamoDBContext(client);
        }

        [HttpGet]
        public async Task<object> GetAllData()
        {
            var conditions = new List<ScanCondition>();
            var pacientes = await context.ScanAsync<Paciente>(conditions).GetRemainingAsync();

            return Ok(pacientes);
        }

        [HttpGet("{pacienteId}")]
        public async Task<object> GetDataFromPatient(Guid pacienteId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("PacienteId", ScanOperator.Equal, pacienteId)
            };

            var pacientes = await context.ScanAsync<Paciente>(conditions).GetRemainingAsync();

            return Ok(pacientes);
        }

        [HttpGet("pressao-alta")]
        public async Task<object> GetPatientsWithHighPressure()
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("PressaoSistolica", ScanOperator.GreaterThanOrEqual, 150),
                new ScanCondition("PressaoDiastolica", ScanOperator.GreaterThanOrEqual, 100)
            };
            
            var pacientes = await context.ScanAsync<Paciente>(conditions).GetRemainingAsync();

            return Ok(pacientes);
        }
    }
}
