using DotvvmBootstrapApplication.Models;

namespace DotvvmBootstrapApplication.WebAPI.Controllers
{
    public class ExampleRecordController : BaseController<ExampleRecord>
    {
        public ExampleRecordController(BootstrapDbContext context) : base(context)
        {
        }
    }
}
