using Hangfire;
using Hangfire.MySql;
using OpenDeepSpace.NetCore.Hangfire;
using OpenDeepSpace.NetCore.Hangfire.Demo.Jobs;
using OpenDeepSpace.NetCore.Hangfire.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Hangfire的使用
builder.Services.AddHangfire(
		opt =>
		{
			opt.UseStorage(new MySqlStorage("Data Source=127.0.0.1;Initial Catalog=ods;User ID=root;Password=wy.023;Charset=utf8;Port=3306;Allow User Variables=true;", new MySqlStorageOptions()
			{

				TablesPrefix = "hangfire"
			}));

			//注册使用了RecurringJobAttribute特性的周期性Job
			opt.RegisterRecurringJobs();

		}
	);

//添加HangfireServer
builder.Services.AddHangfireServer(opt =>
{
    //添加Hangfire服务的配置

    opt.Queues =new[] { "default", "local", "recurringjobqueue" };//队列一定要指定 如果Job指定了队列 这里没加入 Job将无法执行 建议至少指定前两个队列即default local

});

//注册参数化Job
builder.Services.RegisterParametricJobs();
//添加JobState状态监控 用于成功或失败执行结果的处理
GlobalJobFilters.Filters.Add(new JobStateFilter(builder.Services));
//注入Job成功失败的处理实现
builder.Services.AddTransient<IJobExecuteResultHandler, JobExecuteResultHandler>();
//设置成功执行的Job持久化时间
GlobalStateHandlers.Handlers.Add(new SucceededJobExpiredHandler());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//使用控制面板
app.UseHangfireDashboard();

app.UseAuthorization();

app.MapControllers();

app.Run();
