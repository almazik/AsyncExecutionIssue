using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;

namespace MvcApplication1.Controllers
{
	public class HomeController : Controller
	{
		private static int _count;

		public ActionResult Index()
		{
			RunRequest(); //I don't want to wait on this task
			return View(_count);
		}

		public async Task RunRequest()
		{
			CloudStorageAccount account = CloudStorageAccount.DevelopmentStorageAccount;
			var cloudTable = account.CreateCloudTableClient().GetTableReference("test");

			Interlocked.Increment(ref _count);
			await Task.Factory.FromAsync<bool>(cloudTable.BeginCreateIfNotExists, cloudTable.EndCreateIfNotExists, null);

			Trace.WriteLine("This part of task after await is never executed");
			Interlocked.Decrement(ref _count);
		}
	}
}